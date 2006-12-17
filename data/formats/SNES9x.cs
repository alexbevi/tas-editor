using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
/***************************************************************************************************
 * Read and parse an SMV file
 **************************************************************************************************/
namespace MovieSplicer.Data
{
    public class SNES9x
    {                       
        const byte BYTES_PER_FRAME = 2;        
                
        private byte[] fileContents;
        
        public string            Filename;
        public SMVHeader         Header;
        public SMVOptions        Options;
        public SMVROMInfo        ROMInfo;
        public SMVControllerData ControllerData;

        static Functions fn = new Functions();
        static int[] offsets = {
            0x00, // 4-byte signature: 53 4D 56 1A "SMV\x1A"
            0x04, // 4-byte little-endian unsigned int: version number, must be 1
            0x08, // 4-byte little-endian integer: movie "uid" - recording time in Unix epoch format
            0x0C, // 4-byte little-endian unsigned int: rerecord count
            0x10, // 4-byte little-endian unsigned int: number of frames
            0x14, // 1-byte flags "controller mask":
                  //    bit 0: controller 1 in use
                  //    bit 1: controller 2 in use
                  //    bit 2: controller 3 in use
                  //    bit 3: controller 4 in use
                  //    bit 4: controller 5 in use
                  //    other: reserved, set to 0
            0x15, // 1-byte flags "movie options":
                  //    bit 0:
                  //    if "0", movie begins from an embedded "quicksave" snapshot
                  //    if "1", a SRAM is included instead of a quicksave; movie begins from reset
                  //    bit 1: if "0", movie is NTSC (60 fps); if "1", movie is PAL (50 fps)
                  //    other: reserved, set to 0
            0x16, // 1-byte flags: reserved, set to 0
            0x17, // 1-byte flags "sync options":
                  //    bit 0: MOVIE_SYNC_DATA_EXISTS
                  //        if "1", the following bits are defined.
                  //        if "0", the following bits have no meaning.
                  //    bit 1: MOVIE_SYNC_WIP1TIMING
                  //    bit 2: MOVIE_SYNC_LEFTRIGHT
                  //    bit 3: MOVIE_SYNC_VOLUMEENVX
                  //    bit 4: MOVIE_SYNC_FAKEMUTE
                  //    bit 5: MOVIE_SYNC_SYNCSOUND
                  //    bit 6: MOVIE_SYNC_HASROMINFO
                  //        if "1", there is extra ROM info located right in between of the metadata and the savestate.
            0x18, // 4-byte little-endian unsigned int: offset to the savestate inside file
            0x1C, // 4-byte little-endian unsigned int: offset to the controller data inside file
            0x20, // UTF16-coded movie title string (author info)   
         
            0x03, // Extra Rom Info -> Rom CRC
            0x07  // Extra Rom Info -> Rom Name
        };  

        /// <summary>
        /// Create a fully instantiated SMV object from the passed file
        /// </summary>        
        public SNES9x(string SMVFile)
        {
            Filename = SMVFile;

            FileStream   fs = File.OpenRead(SMVFile);
            BinaryReader br = new BinaryReader(fs);
            
            fileContents = br.ReadBytes((int)fs.Length);

            br.Close(); br = null; fs.Dispose();
            
            Header         = new SMVHeader(ref fileContents);
            Options        = new SMVOptions(Header.SyncOptionsFlags, Header.MovieOptionsFlags);
            ROMInfo        = new SMVROMInfo(ref fileContents, Options.HASROMINFO, Header.SaveStateOffset);
            ControllerData = new SMVControllerData(ref fileContents, Header.ControllerFlags, (int)Header.FrameCount, Header.ControllerDataOffset);
        }                      

    #region "Structure"

        /// <summary>
        /// Parse the SMV Header information
        /// </summary>
        public class SMVHeader
        {
            const byte METADATA_LENGTH = 30;            

            public string Signature;
            public uint   Version;
            public string UID;
            public uint   ReRecordCount;
            public uint   FrameCount;
            public byte   ControllerFlags;
            public byte   MovieOptionsFlags;
            public byte   SyncOptionsFlags;
            public uint   SaveStateOffset;
            public uint   ControllerDataOffset;
            public string Metadata;
            
            public SMVHeader(ref byte[] byteArray)
            {
                Signature            = fn.ReadHEX(fn.readBytes(ref byteArray, offsets[0], 4));
                Version              = fn.Read32(fn.readBytes(ref byteArray, offsets[1], 4));
                UID                  = fn.ConvertUNIXTime((fn.Read32(fn.readBytes(ref byteArray, offsets[2], 4))));
                ReRecordCount        = fn.Read32(fn.readBytes(ref byteArray, offsets[3], 4));
                FrameCount           = fn.Read32(fn.readBytes(ref byteArray, offsets[4], 4));
                ControllerFlags      = fn.readBytes(ref byteArray, offsets[5], 1)[0];
                MovieOptionsFlags    = fn.readBytes(ref byteArray, offsets[6], 1)[0];
                SyncOptionsFlags     = fn.readBytes(ref byteArray, offsets[8], 1)[0];
                SaveStateOffset      = fn.Read32(fn.readBytes(ref byteArray, offsets[9], 4));
                ControllerDataOffset = fn.Read32(fn.readBytes(ref byteArray, offsets[10], 4));
                Metadata             = fn.ReadChars16(fn.readBytes(ref byteArray, offsets[11], (int)SaveStateOffset - METADATA_LENGTH));             
            }
        }

        /// <summary>
        /// SMV synch option flags parser        
        /// </summary>
        public class SMVOptions
        {                               
            public bool HASROMINFO;
            public bool WIP1TIMING;
            public bool LEFTRIGHT;
            public bool VOLUMEENVX;
            public bool FAKEMUTE;
            public bool SYNCSOUND;

            public bool RESET = false; // check if movie starts from save or reset
            public bool PAL   = false; // check if movie is ntsc or pal
            
            public SMVOptions(byte syncOptions, byte movieOptions)
            {
                WIP1TIMING = (1 & (syncOptions >> 1)) == 1 ? true : false;
                LEFTRIGHT  = (1 & (syncOptions >> 2)) == 1 ? true : false;
                VOLUMEENVX = (1 & (syncOptions >> 3)) == 1 ? true : false;
                FAKEMUTE   = (1 & (syncOptions >> 4)) == 1 ? true : false;
                SYNCSOUND  = (1 & (syncOptions >> 5)) == 1 ? true : false;
                HASROMINFO = (1 & (syncOptions >> 6)) == 1 ? true : false;

                if (movieOptions == 0x01 || movieOptions == 0x03) RESET = true;
                if (movieOptions == 0x02 || movieOptions == 0x03) PAL   = true;                
            }                     
        }

        /// <summary>
        /// Encapsulates Extra ROM Info (if available)        
        /// </summary>        
        public class SMVROMInfo
        {
            const byte EXTRAROMINFO_SIZE = 30;

            public string CRC = "";
            public string Name = "";
            
            public SMVROMInfo(ref byte[] byteArray, bool extraInfo, uint offset)
            {
                if (!extraInfo) return;
               
                // 000 3 bytes of zero padding: 00 00 00
                // 003 4-byte integer: CRC32 of the ROM
                // 007 23-byte ascii string:
                //      the game name copied from the ROM, truncated to 23 bytes
                //      (the game name in the ROM is 21 bytes)
                
                CRC = fn.ReadHEXUnicode(fn.readBytes(ref byteArray, 
                    (0x03 + (int)offset - EXTRAROMINFO_SIZE), 4));
                Name = fn.ReadChars(fn.readBytes(ref byteArray, 
                    (0x07 + (int)offset - EXTRAROMINFO_SIZE), 23));
            
            }
        }

        /// <summary>
        /// Parse the controller data into an array, along with a textual representation of the input
        /// Also contains a bool array of the controllers in use
        /// </summary>
        public class SMVControllerData
        {            
            public bool[]    Controller = { false, false, false, false, false };                        
            public ArrayList ControllerInput = new ArrayList();
           
            public SMVControllerData(ref byte[] byteArray, byte controllerFlags, int frameCount, uint offset)
            {
                byte controllerCount = 0;
                
                // check which controllers are in use
                for (int c = 0; c < 5; c++)
                {
                    if ((1 & (controllerFlags >> c)) == 1)
                    {
                        Controller[c] = true;
                        controllerCount++;
                    }
                }                
               
                // parse frame data
                for (int i = 0; i < frameCount; i++)
                {
                    string[] input = new string[controllerCount];
                    
                    // cycle through the controller data for the current frame
                    for (int j = 0; j < controllerCount; j++)
                        input[j] = parseControllerData(fn.readBytes(ref byteArray,
                            (int)offset + (i * BYTES_PER_FRAME) + (j * BYTES_PER_FRAME),
                            BYTES_PER_FRAME));

                    ControllerInput.Add(input);
                }
            }

            /// <summary>
            /// Convert the binary representation of input to meaningful values 
            /// </summary>
            private string parseControllerData(byte[] byteArray)
            {                
                string   input = "";                
                string[] inputValues = { ">", "<", "v", "^", "S", "s", "Y", "B", "R", "L", "X", "A" };                
              
                // check the first byte of input
                for (int i = 0; i < 8; i++)
                {                    
                    if ((1 & (byteArray[1] >> i)) == 1)
                        input += inputValues[i];                    
                }
                // check the second byte of input
                for (int j = 4; j < 8; j++)
                {
                    if ((1 & (byteArray[0] >> j)) == 1)
                        input += inputValues[j+4];
                }               
                return input;
            }
            
        }

    #endregion

    #region "Methods"       
       
        /// <summary>
        /// Save an SMV file back out to disk
        /// </summary>
        public void Save(string filename, ref ArrayList input)
        {
            ArrayList outputFile = new ArrayList();            

            // get the initial file contents up to the controller data
            fn.bytesToArray(ref outputFile, this.fileContents, 0, (int)this.Header.ControllerDataOffset);

            // parse frame data
            for (int i = 0; i < input.Count; i++)
            {
                string[] frameData = (string[])input[i];
                //cycle through the controller data for the current frame
                for (int j = 0; j < frameData.Length; j++)
                {
                    byte[] convertedInput = parseControllerData(frameData[j]);
                    outputFile.Add(convertedInput[0]);
                    outputFile.Add(convertedInput[1]);
                }
            }
            
            // if no filename provided, assign current filename
            if (filename == "") filename = this.Filename;

            FileStream   fs     = File.Open(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);

            // convert to 4-byte little-endian
            byte[] newFrameCount = fn.Write32(input.Count);

            // position in the target stream to insert the new values
            outputFile[0x10] = newFrameCount[0];
            outputFile[0x11] = newFrameCount[1];
            outputFile[0x12] = newFrameCount[2];
            outputFile[0x13] = newFrameCount[3];

            // write out the new file's contents
            foreach (byte b in outputFile) writer.Write(b);

            writer.Close(); writer = null; fs.Dispose();       
        } 

        /// <summary>
        /// Convert the string representation of input back to binary values
        /// </summary>
        private byte[] parseControllerData(string inputValues)
        {
            byte[] input = { 0x00, 0x00 };
            
            if (inputValues.Contains(">")) input[1] |= (1 << 0);
            if (inputValues.Contains("<")) input[1] |= (1 << 1);
            if (inputValues.Contains("v")) input[1] |= (1 << 2);
            if (inputValues.Contains("^")) input[1] |= (1 << 3);
            if (inputValues.Contains("S")) input[1] |= (1 << 4);
            if (inputValues.Contains("s")) input[1] |= (1 << 5);
            if (inputValues.Contains("Y")) input[1] |= (1 << 6);
            if (inputValues.Contains("B")) input[1] |= (1 << 7);
            if (inputValues.Contains("R")) input[0] |= (1 << 4);
            if (inputValues.Contains("L")) input[0] |= (1 << 5);
            if (inputValues.Contains("X")) input[0] |= (1 << 6);
            if (inputValues.Contains("A")) input[0] |= (1 << 7);

            return input;        
        } 
              
    #endregion
    
    }
}
