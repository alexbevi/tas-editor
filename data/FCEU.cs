using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
/***************************************************************************************************
 * Read and parse an FCE Ultra FCM file
 * 
 * Process for parsing the controller data (read) modified from the Nesmock source  
 * Process for parsing the controller data (write) modified from FCEU source
 **************************************************************************************************/
namespace MovieSplicer.Data
{ 
    public class FCEU
    {
        private byte[] fileContents;
        
        public string            Filename;        
        public FCMHeader         Header;
        public FCMControllerData ControllerData;

        static Functions fn = new Functions();
        static int[] offsets = {
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
    
        /// <summary>
        /// Create a new FCM object from the passed file.
        /// </summary>        
        public FCEU(string FCMFile)
        {
            Filename = FCMFile;
            
            FileStream   fs = File.OpenRead(FCMFile);
            BinaryReader br = new BinaryReader(fs);

            // read the FCM file into a byte array
            fileContents = br.ReadBytes((int)fs.Length);

            br.Close(); br = null; fs.Dispose();

            Header         = new FCMHeader(ref fileContents);
            ControllerData = new FCMControllerData(ref fileContents,
                (int)Header.ControllerDataOffset,
                (int)Header.ControllerDataLength);
        }

    #region "Structure"

        /// <summary>
        /// FCM Header Data
        /// 
        /// TODO::I don't like the structure anymore ... clean and optimize
        /// </summary>
        public class FCMHeader
        {                                
            public string Signature;
            public uint   Version;
            public byte   OptionFlags;
            public uint   FrameCount;
            public uint   ReRecordCount;
            public uint   EmulatorVersion;
            public uint   ControllerDataLength;
            public uint   ControllerDataOffset;
            public uint   SaveStateOffset;            
            public string Author;
            public string ROMName = "";
            public string ROMCRC  = "";                                    
            public bool   StartFromReset = false;
            public bool   StartFromSave  = false;
            public bool   NTSC           = false;
            public bool   PAL            = false;            

            public FCMHeader(ref byte[] byteArray)
            {
                Signature            = fn.ReadHEX(fn.readBytes(ref byteArray, offsets[0], 4));
                Version              = fn.Read32(fn.readBytes(ref byteArray, offsets[1], 4));
                OptionFlags          = fn.readBytes(ref byteArray, offsets[2], 1)[0];
                FrameCount           = fn.Read32(fn.readBytes(ref byteArray, offsets[6], 4));
                ReRecordCount        = fn.Read32(fn.readBytes(ref byteArray, offsets[7], 4));
                ControllerDataLength = fn.Read32(fn.readBytes(ref byteArray, offsets[8], 4));
                SaveStateOffset      = fn.Read32(fn.readBytes(ref byteArray, offsets[9], 4));
                ControllerDataOffset = fn.Read32(fn.readBytes(ref byteArray, offsets[10], 4));                     
              
                for (int i = 0; i < 16; i++)
                    ROMCRC += byteArray[offsets[11] + i].ToString("X");

                EmulatorVersion = fn.Read32(fn.readBytes(ref byteArray, offsets[12], 4));

                int length      = fn.seekNullPosition(byteArray, offsets[13]);
                ROMName         = fn.ReadChars(fn.readBytes(ref byteArray, offsets[13], length));
              
                int startPos    = offsets[13] + ROMName.Length + 1;
                Author          = fn.ReadChars(fn.readBytes(ref byteArray, startPos, (int)SaveStateOffset - startPos));

                if ((OptionFlags >> 1) == 1) StartFromReset = true;
                if ((OptionFlags >> 1) == 0) StartFromSave  = true;
                if ((OptionFlags >> 2) == 1) PAL            = true;
                if ((OptionFlags >> 2) == 0) NTSC           = true;
            }                        
        }

        /// <summary>
        /// FCM Controller Data
        /// 
        /// TODO::Optimize (see SMV controller section)
        /// </summary>
        public class FCMControllerData
        {
            public bool[]    Controller = { false, false, false, false };
            public ArrayList ControllerInput;
           
            public FCMControllerData(ref byte[] byteArray, int controllerDataOffset, int controllerDataLength)
            {
                ControllerInput  = new ArrayList();                                
                                               
                int   position   = controllerDataOffset;
                int   frameCount = 0;
                int[] joop       = {0,0,0,0};                

                while (controllerDataLength > 0)
                {
                    int updateType = byteArray[position] >> 7;
                    int NDelta     = (byteArray[position] >> 5 ) & 3;
                    int delta      = 0;
                    int data       = byteArray[position] & 0x1F;
                    
                    ++position; --controllerDataLength;

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
                    string[] parsedControllerData = parsedControllerData = parseControllerData(joop);

                    while(delta > 0)
                    {
                        // Save the controlled data                                                                                                                                  
                        ControllerInput.Add(parsedControllerData);
                        ++frameCount; --delta;
                    }

                    if (controllerDataLength > NDelta)
                        controllerDataLength -= NDelta;
                    else
                        controllerDataLength = 0;

                    if (updateType == 0) // Controller data
                    {
                        int ctrlno = (data >> 3);
                        joop[ctrlno] ^= (1 << (data & 7));
                        if (ctrlno == 0) { Controller[0] = true; }
                        if (ctrlno == 1) { Controller[1] = true; }
                        if (ctrlno == 2) { Controller[2] = true; }
                        if (ctrlno == 3) { Controller[3] = true; }                       
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
            private string[] parseControllerData(int[] joop)
            {
                // return the string representation of the controller inputs
                string[] frameData = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    if ((1 & (joop[i] >> 0)) == 1) frameData[i] += "A";
                    if ((1 & (joop[i] >> 1)) == 1) frameData[i] += "B";
                    if ((1 & (joop[i] >> 2)) == 1) frameData[i] += "s";
                    if ((1 & (joop[i] >> 3)) == 1) frameData[i] += "S";
                    if ((1 & (joop[i] >> 4)) == 1) frameData[i] += "^";
                    if ((1 & (joop[i] >> 5)) == 1) frameData[i] += "v";
                    if ((1 & (joop[i] >> 6)) == 1) frameData[i] += "<";
                    if ((1 & (joop[i] >> 7)) == 1) frameData[i] += ">";
                }
                return frameData;
            }
        }

    #endregion

    #region "Methods"

        /// <summary>
        /// Save changes to the currently buffered FCM file
        /// 
        /// TODO::CompressedData length seems to be off by 1byte (even after
        /// adding the initial reset and final null command)
        /// </summary>        
        public void Save(string filename, ref ArrayList input)
        {
            ArrayList CompressedData = new ArrayList();                        
            
            int      buffer = 0;
            int[]    joop   = { 0, 0, 0, 0 };
            string[] Cdata  = null ;
            
            CompressedData.Add(((0x81) & 0x9F) | (0 << 5)); // start from reset

            for (int i = 0; i < input.Count; i++)
            {
                // load the current frame's input
                Cdata = (string[])input[i];
                
                //// cycle through the controllers
                for (int j = 0; j < 4; j++)
                {
                    // HACK::Copy-Pasting doesn't check source-target controller counts so
                    // this just makes sure the index doesn't go out of bounds.
                    if (j >= Cdata.Length) break;

                    int current = parseFrameData(Cdata[j]);
                    if (current != joop[j])
                    {
                        // cycle through each bit to see if it's changed
                        for (int k = 0; k < 8; k++)
                        {
                            // if there is a difference, write it out as a command
                            if (((current ^ joop[j]) & (1 << k)) > 0)
                                DoEncode(j, k, ref buffer, ref CompressedData);
                        }
                    }
                    joop[j] = current;
                }             
                buffer++;
            }
            DoEncode(0, 0x80, ref buffer, ref CompressedData);
            //CompressedData.Add(((0x80) & 0x9F) | (0 << 5)); // null command

            // copy original file up to the beginning of the controller input
            ArrayList outputFile = new ArrayList();
            fn.bytesToArray(ref outputFile, this.fileContents, 0, (int)this.Header.ControllerDataOffset);


            byte[] controllerDataLength = fn.Write32(CompressedData.Count); // new controller data length
            byte[] frameCount = fn.Write32(input.Count); // new frame count
            // write new values
            for (int i = 0; i < 4; i++)
            {
                outputFile[0x0C + i] = frameCount[i];
                outputFile[0x14 + i] = controllerDataLength[i];
            }

            if (filename == "") filename = this.Filename;

            FileStream   fs     = File.Open(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);

            foreach (byte b in outputFile)    writer.Write(b);
            foreach (int b in CompressedData) writer.Write((byte)b);

            writer.Close(); writer = null; fs.Dispose();                       
        }
        
        /// <summary>
        /// Encode FCM controller data
        /// 
        /// NOTE::This is almost verbatum from the FCEU source
        /// </summary>
        private void DoEncode(int joy, int button, ref int buffer, ref ArrayList CompressedData)
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

            CompressedData.Add(d);            
            while (buffer > 0)
            {
                CompressedData.Add((buffer & 0xff));                
                buffer >>= 8;
            }
        }

        /// <summary>
        /// Convert frame input from string values to their binary representation
        /// </summary>
        private byte parseFrameData(string frameData)
        {
            byte joop = 0x00;
            if (frameData != null)
            {
                if (frameData.Contains("A")) joop |= (1 << 0);
                if (frameData.Contains("B")) joop |= (1 << 1);
                if (frameData.Contains("s")) joop |= (1 << 2);
                if (frameData.Contains("S")) joop |= (1 << 3);
                if (frameData.Contains("^")) joop |= (1 << 4);
                if (frameData.Contains("v")) joop |= (1 << 5);
                if (frameData.Contains("<")) joop |= (1 << 6);
                if (frameData.Contains(">")) joop |= (1 << 7);
            }
            return joop;
        }             

    #endregion

    }
}