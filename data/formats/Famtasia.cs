using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MovieSplicer.Data.Formats
{
    public class Famtasia : TASMovie
    {
        /// <summary>
        /// Contains Format Specific items
        /// </summary>
        public struct FormatSpecific
        {
            public bool FDS;
        }   

        const byte HEADER_SIZE = 144;

        public Header         FMVHeader;
        public Options        FMVOptions;
        public Extra          FMVExtra;
        public Input          FMVInput;
        public FormatSpecific FMVSpecific;

        private byte     BytesPerFrame = 0;
        private string[] InputValues = { ">", "<", "^", "v", "B", "A", "s", "S" };
        private int[]    Offsets = {
            0x00, // 4-byte signature: 46 4D 56 1A "FMV\x1A"
            0x04, // 1-byte flags:
                  //      bit 7: 0=reset-based, 1=savestate-based
                  //      other bits: unknown, set to 0
            0x05, // 1-byte flags:
                  //      bit 7: uses controller 1
                  //      bit 6: uses controller 2
                  //      bit 5: is a FDS recording
                  //      other bits: unknown, set to 0
            0x06, // 4-byte little-endian unsigned int: unknown, set to 00000000
            0x0A, // 4-byte little-endian unsigned int: rerecord count minus 1
            0x0E, // 2-byte little-endian unsigned int: unknown, set to 0000
            0x10, // 64-byte zero-terminated emulator identifier string
            0x50, // 64-byte zero-terminated movie title string
            0x90  // frame data begins here
        };

        public Famtasia(string FMVFile)
        {                        
            Filename = FMVFile;
            FillByteArrayFromFile(FMVFile, ref FileContents);

            FMVOptions = new Options(true);            
            FMVOptions.MovieStartFlag[0] = (1 & (FileContents[Offsets[1]]) >> 7) == 0 ? true : false;
            FMVOptions.MovieStartFlag[1] = (1 & (FileContents[Offsets[1]]) >> 7) == 1 ? true : false;
            
            FMVExtra = new Extra();
            FMVExtra.Description = ReadChars(ref FileContents, Offsets[7], 64);

            FMVInput = new Input(2, false);            
            for (int i = 0; i < 2; i++)
            {
                if ((1 & (FileContents[Offsets[2]]) >> 7 - i) == 1)
                {
                    FMVInput.Controllers[i] = true;                    
                    BytesPerFrame++;
                }
            }

            FMVSpecific = new FormatSpecific();
            if ((1 & (FileContents[Offsets[2]]) >> 5) == 1)
            {
                FMVSpecific.FDS = true;
                BytesPerFrame++;
            }
            else
                FMVSpecific.FDS = false;

            FMVHeader = new Header();
            FMVHeader.Signature     = ReadHEX(ref FileContents, Offsets[0], 4);
            FMVHeader.RerecordCount = Read32(ref FileContents, Offsets[4]);
            FMVHeader.EmulatorID    = ReadChars(ref FileContents, Offsets[6], 64);
            FMVHeader.FrameCount    = FileContents.Length - HEADER_SIZE / BytesPerFrame;

            getFrameInput(ref FileContents);
        }

        /// <summary>
        /// Convert FMV binary data to a readable string representation
        /// </summary>
        private void getFrameInput(ref byte[] byteArray)
        {
            FMVInput.FrameData = new TASMovieInput[FMVHeader.FrameCount];
            
            for (int i = 0; i < FMVHeader.FrameCount; i++)
            {
                FMVInput.FrameData[i] = new TASMovieInput();
                for (int j = 0; j < BytesPerFrame; j++)                
                    FMVInput.FrameData[i].Controller[j] = parseControllerData(byteArray[HEADER_SIZE + i + j]);                                    
                i += BytesPerFrame - 1;
            }
        }

        /// <summary>
        /// FMV input convertion (bit position -> string)
        /// </summary> 
        private string parseControllerData(byte frameInput)
        {
            string input = "";

            for (int i = 0; i < 8; i++)
                if ((1 & (frameInput >> i)) == 1) input += InputValues[i];
            
            return input;
        }
       
        /// <summary>
        /// FMV input conversion (string -> bit position)
        /// </summary> 
        private byte parseControllerData(string frameValue)
        {
            byte input = 0x00;

            for (int i = 0; i < 8; i++)
                if (frameValue.Contains(InputValues[i])) input |= (byte)(1 << i);

            return input;
        }

        /// <summary>
        /// Save input changes back out to file
        /// 
        /// TODO::Save might fail if this is an FDS movie (since FDS is factored into the BytesPerFrame)
        /// </summary>        
        public void Save(string filename, ref TASMovieInput[] input)
        {
            byte[] head = ReadBytes(ref FileContents, 0, Offsets[8]);

            // create the output array and copy in the contents
            byte[] outputFile = new byte[head.Length + input.Length * BytesPerFrame];
            head.CopyTo(outputFile, 0);
            
            int position = 0;
            int controllers = FMVInput.ControllerCount;
            for (int i = 0; i < input.Length; i++)            
                for (int j = 0; j < controllers; j++)                
                    outputFile[head.Length + position++] = parseControllerData(input[i].Controller[j]);                                                          
           
            // NOTE::FMV files calculate frameCount based on filesize
            WriteByteArrayToFile(ref outputFile, filename, 0, 0);  
        }
    }
}
