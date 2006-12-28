using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSplicer.Data.Formats
{
    public class FCEU : TASMovie
    {
        public Header  FCMHeader;
        public Options FCMOptions;
        public Extra   FCMExtra;
        public Input   FCMInput;

        private int      ControllerDataLength;
        private string[] InputValues = { "A", "B", "s", "S", "^", "v", "<", ">" };
        private int[] Offsets = {
            0x00, // 4-byte signature: 46 43 4D 1A "FCM\x1A"
            0x04, // 4-byte little-endian unsigned int: version number, must be 2
            0x08, // 1-byte flags:
                  //    bit 0: reserved, set to 0
                  //    bit 1:
                  //        if "0", movie begins from an embedded "quicksave" snapshot
                  //        if "1", movie begins from reset 
                  //    bit 2:
                  //        if "0", NTSC timing
                  //        if "1", PAL timing           
                  //    other: reserved, set to 0
            0x09, // 1-byte flags: reserved, set to 0
            0x0A, // 1-byte flags: reserved, set to 0
            0x0B, // 1-byte flags: reserved, set to 0
            0x0C, // 4-byte little-endian unsigned int: number of frames
            0x10, // 4-byte little-endian unsigned int: rerecord count
            0x14, // 4-byte little-endian unsigned int: length of controller 
            0x18, // 4-byte little-endian unsigned int: offset to the savestate inside file
            0x1C, // 4-byte little-endian unsigned int: offset to the controller data inside file
            0x20, // 16-byte md5sum of the ROM used
            0x30, // 4-byte little-endian unsigned int: version of the emulator used
            0x34, // name of the ROM used - UTF8 encoded nul-terminated string.
        };

        public FCEU(string FCMFile)
        {
            Filename = FCMFile;
            FillByteArrayFromFile(FCMFile, ref FileContents);

            byte options = FileContents[Offsets[2]];
            SaveStateOffset = Read32(ref FileContents, Offsets[9]);
            ControllerDataOffset = Read32(ref FileContents, Offsets[10]);
            ControllerDataLength = Read32(ref FileContents, Offsets[8]);

            FCMHeader = new Header();
            FCMHeader.Signature = ReadHEX(ref FileContents, Offsets[0], 4);
            FCMHeader.Version = Read32(ref FileContents, Offsets[1]);
            FCMHeader.FrameCount = Read32(ref FileContents, Offsets[6]);
            FCMHeader.RerecordCount = Read32(ref FileContents, Offsets[7]);
            FCMHeader.EmulatorID = Read32(ref FileContents, Offsets[12]).ToString();

            FCMExtra = new Extra();
            FCMExtra.CRC = ReadHEX(ref FileContents, Offsets[11], 16);
            FCMExtra.ROM = ReadCharsNullTerminated(ref FileContents, Offsets[13]);

            int startPos = Offsets[13] + FCMExtra.ROM.Length + 1;
            FCMExtra.Author = ReadChars(ref FileContents, startPos, SaveStateOffset - startPos);

            FCMOptions = new Options(true);
            FCMOptions.MovieStartFlag[0] = ((options >> 1) == 1) ? true : false;
            FCMOptions.MovieStartFlag[1] = ((options >> 1) == 0) ? true : false;
            FCMOptions.MovieTimingFlag[0] = ((options >> 2) == 0) ? true : false;
            FCMOptions.MovieTimingFlag[1] = ((options >> 2) == 1) ? true : false;

            FCMInput = new Input(4, false);
            getFrameInput(ref FileContents);
        }

        private void getFrameInput(ref byte[] byteArray)
        {
            FCMInput.FrameData = new TASMovieInput[FCMHeader.FrameCount];

            int position = ControllerDataOffset;
            int frameCount = 0;
            int[] joop = { 0, 0, 0, 0 };

            while (ControllerDataLength > 0)
            {
                int updateType = byteArray[position] >> 7;
                int NDelta = (byteArray[position] >> 5) & 3;
                int delta = 0;
                int data = byteArray[position] & 0x1F;

                ++position; --ControllerDataLength;

                switch (NDelta)
                {
                    case 0:
                        break;
                    case 1:
                        delta |= byteArray[position++];
                        break;
                    case 2:
                        delta |= byteArray[position++];
                        delta |= byteArray[position++] << 8;
                        break;
                    case 3:
                        delta |= byteArray[position++];
                        delta |= byteArray[position++] << 8;
                        delta |= byteArray[position++] << 16;
                        break;
                }

                // populate parsed data once (since it doesn't change until delta == 0,
                // there's no need to process each time the loop iterates)
                TASMovieInput parsedControllerData = parseControllerData(joop);

                while (delta > 0)
                {
                    // Save the controlled data                                                                                                                                  
                    FCMInput.FrameData[frameCount] = new TASMovieInput();
                    FCMInput.FrameData[frameCount] = parsedControllerData;
                    ++frameCount; --delta;
                }

                if (ControllerDataLength > NDelta)
                    ControllerDataLength -= NDelta;
                else
                    ControllerDataLength = 0;

                if (updateType == 0) // Controller data
                {
                    int ctrlno = (data >> 3);
                    joop[ctrlno] ^= (1 << (data & 7));
                    if (ctrlno == 0) { FCMInput.Controllers[0] = true; }
                    if (ctrlno == 1) { FCMInput.Controllers[1] = true; }
                    if (ctrlno == 2) { FCMInput.Controllers[2] = true; }
                    if (ctrlno == 3) { FCMInput.Controllers[3] = true; }
                }

                // Exerpt from Nesmock 1.6.0 source.
                //else // Control data
                //    switch (data)
                //    {
                //        case 0: break; // nothing
                //        case 1: Save = false; break; // reset
                //        case 2: Save = false; break; // power cycle
                //        case 7: break; // VS coin
                //        case 8: break; // VS dip0
                //        case 24: FDS = true; Cdata[frame].FDS |= 1; break; /* FDS insert, FIXME */
                //        case 25: FDS = true; Cdata[frame].FDS |= 2; break; /* FDS eject, FIXME */
                //        case 26: FDS = true; Cdata[frame].FDS |= 4; break; /* FDS swap, FIXME */
                //    }

            }
        }
        /// <summary>
        /// Convert frame input from binary values to their string representation
        /// </summary>
        private TASMovieInput parseControllerData(int[] joop)
        {
            TASMovieInput frameData = new TASMovieInput();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 8; j++)
                    if ((1 & (joop[i] >> j)) == 1) frameData.Controller[i] += InputValues[j];

            return frameData;
        }

        /// <summary>
        /// Convert frame input from string values to their binary representation
        /// </summary>
        private byte parseControllerData(string frameValue)
        {
            byte joop = 0x00;

            if (frameValue != null && frameValue != "")
                for (int i = 0; i < 8; i++)
                    if (frameValue.Contains(InputValues[i])) joop |= (byte)(1 << i);

            return joop;
        }

        /// <summary>
        /// Save changes to the currently buffered FCM file        
        /// </summary>        
        public void Save(string filename, ref TASMovieInput[] input)
        {
            byte[] head = ReadBytes(ref FileContents, 0, ControllerDataOffset);
            int cdataLength = GetRLELength(ref input);
            int controllers = FCMInput.ControllerCount;

            int   buffer   = 0;
            int[] joop     = { 0, 0, 0, 0 };
            int   position = ControllerDataOffset;

            byte[] outputFile = new byte[head.Length + cdataLength + 2];
            head.CopyTo(outputFile, 0);

            outputFile[position++] = ((0x81) & 0x9F) | (0 << 5); // start from reset            

            int frame = 0;
            while (frame < input.Length)
            {
                //// cycle through the controllers
                for (int j = 0; j < controllers; j++)
                {
                    int current = parseControllerData(input[frame].Controller[j]);
                    if (current != joop[j])
                    {
                        // cycle through each bit to see if it's changed
                        for (int k = 0; k < 8; k++)
                        {
                            // if there is a difference, write it out as a command
                            if (((current ^ joop[j]) & (1 << k)) > 0)
                                DoEncode(j, k, ref buffer, ref outputFile, ref position);
                        }
                    }
                    joop[j] = current;
                }
                buffer++; frame++;
            }                                    

            DoEncode(0, 0x80, ref buffer, ref outputFile, ref position);
            //outputFile[position++] = ((0x80) & 0x9F) | (0 << 5); // null command

            // write the new controllerDataLength
            // NOTE::The RLE logic seems to be slightly off and isn't adding the last update
            byte[] controllerDataLength = Write32(cdataLength + 1);
            for (int i = 0; i < 4; i++)          
                outputFile[Offsets[8] + i] = controllerDataLength[i];            

            WriteByteArrayToFile(ref outputFile, filename, input.Length, Offsets[6]);  
        }

        
        /// <summary>
        /// Encode FCM controller data
        /// 
        /// NOTE::This is almost verbatum from the FCEU source
        /// </summary>
        private void DoEncode(int joy, int button, ref int buffer, ref byte[] CompressedData, ref int position)
        {
            int d = 0;

            if (buffer >= ((1 << 24) - 1)) // if the buffer's gonna overflow, write a dummy command
                d = 0x80;
            else if (buffer >= 65536)
                d = 3 << 5;
            else if (buffer >= 256)
                d = 2 << 5;
            else if (buffer > 0)
                d = 1 << 5;

            d |= joy << 3; d |= button;

            CompressedData[position++] = (byte)d;            
            while (buffer > 0)
            {
                CompressedData[position++] = (byte)(buffer & 0xff);
                buffer >>= 8;
            }
        }

        /// <summary>
        /// Get the RLE length of the input so we can size the output array accordingly
        /// </summary>   
        public int GetRLELength(ref TASMovieInput[] input)
        {
            int buffer = 0;
            int[] joop = { 0, 0, 0, 0 };     
            int length = 0;
            for (int i = 0; i < input.Length; i++)
            {                
                // cycle through the controllers
                for (int j = 0; j < 4; j++)
                {                    
                    int current = parseControllerData(input[i].Controller[j]);
                    if (current != joop[j])
                    {
                        // cycle through each bit to see if it's changed
                        for (int k = 0; k < 8; k++)
                        {
                            // if there is a difference, write it out as a command
                            if (((current ^ joop[j]) & (1 << k)) > 0)                                
                            {
                                length++;
                                while (buffer > 0)
                                {
                                    length++;
                                    buffer >>= 8;
                                }
                            }
                        }
                    }
                    joop[j] = current;
                }
                buffer++;
            }
            // handle additional buffered values that may have escaped the initial loop
            // TODO::Functional, but inefficent
            while (buffer > 0)
            {
                length++;
                buffer >>= 8;
            }
            return length;
        }

    }
}
