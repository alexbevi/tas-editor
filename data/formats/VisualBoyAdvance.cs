using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

/***************************************************************************************************
 * Read and parse a VisualBoyAdvance VBM file
 **************************************************************************************************/
namespace MovieSplicer.Data
{
    public class VisualBoyAdvance
    {
        const byte HEADER_SIZE = 64;
        const byte INFO_SIZE   = 192;
        
        private byte[] fileContents;
        private uint   saveStateOffset;
        private uint   controllerDataOffset;

        public string      Filename;
        public VBMHeader   Header;
        public VBMOptions  Options;
        public VBMRomInfo  RomInfo;
        public VBMMetadata Metadata;
        public ArrayList   ControllerData;

        static Functions fn  = new Functions();
        static int[] offsets = {
            0x00, // 4-byte signature: 56 42 4D 1A "VBM\x1A"
            0x04, // 4-byte little-endian unsigned int: version number, must be 1
            0x08, // 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship,
                  //    also used as the recording time in Unix epoch format
            0x0C, // 4-byte little-endian unsigned int: number of frames
            0x10, // 4-byte little-endian unsigned int: rerecord count
            0x14, // 1-byte flags (movie start flags):
                  //    bit 0: if "1", movie starts from an embedded "quicksave" snapshot; if "0", it doesn't
                  //    bit 1: if "1", movie starts from reset with an embedded SRAM; if "0", it doesn't
                  //    (NOTE: if both bits 0 and 1 are "0", then movie begins from power on with no SRAM or save state.
                  //    if both bits 0 and 1 are "1", the movie file is invalid.)
                  //    other: reserved, set to 0
            0x15, // 1-byte flags (controller flags):
                  //    bit 0: controller 1 in use
                  //    bit 1: controller 2 in use (SGB games can be 2-player multiplayer)
                  //    bit 2: controller 3 in use (SGB games can be 3- or 4-player multiplayer with multitap)
                  //    bit 3: controller 4 in use (SGB games can be 3- or 4-player multiplayer with multitap)
                  //    other: reserved
            0x16, // 1-byte flags:  (system flags - note that no matter what these are, the game always runs at 60 frames/sec)
                  //    bit 0: if "1", movie is for the GBA system; if "0", it isn't.
                  //    bit 1: if "1", movie is for the GBC system; if "0", it isn't.
                  //    bit 2: if "1", movie is for the SGB system; if "0", it isn't. (If all 3 of these bits are "0", it's for regular GB.)
                  //    (NOTE:  The above 3 bits are mutually exclusive, although a given ROM can be playable on multiple systems.)
                  //    other: reserved, set to 0
            0x17, // 1-byte flags:  (values of some boolean emulator options)
                  //    bit 0: (useBiosFile)
                  //    if "1" and the movie is of a GBA game, the movie was made using a GBA BIOS file, otherwise it wasn't.
                  //    bit 1: (skipBiosFile)
                  //    if "0" and the movie was made with a GBA BIOS file, the BIOS intro is included in the movie, otherwise it isn't.
                  //    bit 2: (rtcEnable)
                  //    if "1", the emulator's "real time clock" feature was enabled; if "0", it wasn't.
                  //    bit 3: (unsupported)
                  //    must be "0" or the movie file is considered invalid (legacy)
                  //    bit 4: (lagReduction)
                  //    if "0" and the movie is of a GBA game, the movie was made using the old excessively laggy GBA timing, otherwise it wasn't.
                  //    bit 5: (gbcHdma5Fix)
                  //    if "0" and the movie is of a GBC game, the movie was made using the old buggy HDMA5 timing, otherwise it wasn't.
                  //    other: reserved, set to 0
            0x18, // 4-byte little-endian unsigned int: theApp.winSaveType (value of that emulator option)
            0x1C, // 4-byte little-endian unsigned int: theApp.winFlashSize (value of that emulator option)
            0x20, // 4-byte little-endian unsigned int: gbEmulatorType (value of that emulator option)
            0x24, // 12-byte character array: the internal game title of the ROM
                  //    used while recording, not necessarily null-terminated (ASCII?)
            0x30, // 1-byte: reserved, set to 0
            0x31, // 1-byte unsigned char: the internal CRC of the ROM used while recording
            0x32, // 2-byte little-endian unsigned short: the internal Checksum of the ROM used
                  //    while recording, or a calculated CRC16 of the BIOS if GBA
            0x34, // 4-byte little-endian unsigned int: the Game Code of the ROM used
                  //    while recording, or the Unit Code if not GBA
            0x38, // 4-byte little-endian unsigned int: offset to the savestate
                  //    or SRAM inside file, set to 0 if unused
            0x3C  // 4-byte little-endian unsigned int: offset to the controller data inside file
        };
        
        /// <summary>
        /// Create a fully instantiated VBM object from the passed file
        /// </summary>        
        public VisualBoyAdvance(string VBMFile)
        {
            Filename = VBMFile;

            FileStream fs = File.OpenRead(VBMFile);
            BinaryReader br = new BinaryReader(fs);
            
            fileContents = br.ReadBytes((int)fs.Length);

            br.Close(); br = null; fs.Dispose();

            Header   = new VBMHeader(ref fileContents);
            Options  = new VBMOptions(ref fileContents);
            RomInfo  = new VBMRomInfo(ref fileContents);
            Metadata = new VBMMetadata(ref fileContents);

            saveStateOffset      = fn.Read32(fn.readBytes(ref fileContents, offsets[17], 4));
            controllerDataOffset = fn.Read32(fn.readBytes(ref fileContents, offsets[18], 4));

            parseControllerData(ref fileContents);
        }

    #region "Structure"

        /// <summary>
        /// Parse basic VBM header values
        /// </summary>
        public class VBMHeader
        {
            public string Signature;
            public uint   Version;
            public string UID;
            public uint   FrameCount;
            public uint   RerecordCount;

            public VBMHeader(ref byte[] byteArray)
            {
                Signature     = fn.ReadHEX(fn.readBytes(ref byteArray, offsets[0], 4));
                Version       = fn.Read32(fn.readBytes(ref byteArray, offsets[1], 4));
                UID           = fn.ConvertUNIXTime((fn.Read32(fn.readBytes(ref byteArray, offsets[2], 4))));
                FrameCount    = fn.Read32(fn.readBytes(ref byteArray, offsets[3], 4));
                RerecordCount = fn.Read32(fn.readBytes(ref byteArray, offsets[4], 4));
            }
        }

        /// <summary>
        /// Parse the VBM Options flags out to boolean arrays.
        /// 
        /// NOTE::This is the most effective way to do this, though not the most readable
        /// TODO::These routines can all be read through for loops
        /// </summary>
        public class VBMOptions
        {
            public bool[] MovieStart  = { false, false, false };
            public bool[] Controllers = { false, false, false, false };
            public bool[] SystemType  = { false, false, false, false };
            public bool[] BIOSFlags   = { false, false, false, false, false, false };

            public VBMOptions(ref byte[] byteArray)
            {
                MovieStart[0]  = ((byteArray[offsets[5]] >> 0) == 1) ? true : false;
                MovieStart[1]  = ((byteArray[offsets[5]] >> 1) == 1) ? true : false;
                
                Controllers[0] = ((byteArray[offsets[6]] >> 0) == 1) ? true : false;
                Controllers[1] = ((byteArray[offsets[6]] >> 1) == 1) ? true : false;
                Controllers[2] = ((byteArray[offsets[6]] >> 2) == 1) ? true : false;
                Controllers[3] = ((byteArray[offsets[6]] >> 3) == 1) ? true : false;
                
                SystemType[0]  = ((byteArray[offsets[7]] >> 0) == 1) ? true : false;
                SystemType[1]  = ((byteArray[offsets[7]] >> 1) == 1) ? true : false;
                SystemType[2]  = ((byteArray[offsets[7]] >> 2) == 1) ? true : false;
                
                BIOSFlags[0]   = ((byteArray[offsets[8]] >> 0) == 1) ? true : false;
                BIOSFlags[1]   = ((byteArray[offsets[8]] >> 1) == 1) ? true : false;
                BIOSFlags[2]   = ((byteArray[offsets[8]] >> 2) == 1) ? true : false;
                BIOSFlags[3]   = ((byteArray[offsets[8]] >> 3) == 1) ? true : false;
                BIOSFlags[4]   = ((byteArray[offsets[8]] >> 4) == 1) ? true : false;
                BIOSFlags[5]   = ((byteArray[offsets[8]] >> 5) == 1) ? true : false;

                // if no other start flag is set, start from poweron
                if (MovieStart[0] == false && MovieStart[1] == false)
                    MovieStart[2] = true;

                // if no other system is set, system type is GB
                if (SystemType[0] == false && SystemType[1] == false && SystemType[2] == false)
                    SystemType[3] = true;
            }
        }

        /// <summary>
        /// Parse the VBM ROM Info section
        /// 
        /// TODO::CRC/Checksum not really working
        /// </summary>
        public class VBMRomInfo
        {             
            public string Name; 
            public int    CRC;
            public int    Checksum;
            public int    GameCode;

            public VBMRomInfo(ref byte[] byteArray)
            {
                Name     = fn.ReadChars(fn.readBytes(ref byteArray, offsets[12], 12));
                CRC      = (int)(fn.readBytes(ref byteArray, offsets[14], 1)[0]);
                Checksum = fn.Read16(fn.readBytes(ref byteArray, offsets[15], 2));
            }
        }

        /// <summary>
        /// Parse VBM Extended Author information
        /// </summary>
        public class VBMMetadata
        {
            public string Author;
            public string Description;

            public VBMMetadata(ref byte[] byteArray)
            {
                byte[] author = fn.readBytes(ref byteArray, HEADER_SIZE, 64);
                byte[] description = fn.readBytes(ref byteArray, HEADER_SIZE + 64, 128);

                Author = fn.ReadChars(fn.readBytes(ref author, 0, fn.seekNullPosition(author, 0)));
                Description = fn.ReadChars(fn.readBytes(ref description, 0, fn.seekNullPosition(description, 0)));
            }
        }

        private void parseControllerData(ref byte[] byteArray)
        {
            ControllerData = new ArrayList();

            for (int i = 0; i < byteArray.Length - (int)controllerDataOffset; i++)
            {
                string[] frameData = new string[4];
                for (int j = 0; j < 4; j++)
                {
                    if (Options.Controllers[j])
                        frameData[j] = parseInputToString(fn.readBytes(ref byteArray, (int)controllerDataOffset + i + j, 2));
                }
                ControllerData.Add(frameData);
                i++;
            }
         }

        /// <summary>
        /// Parse the 2-byte controller state value to a string
        /// </summary>        
        private string parseInputToString(byte[] bytes)
        {
            string   input = "";
            
            int value = bytes[0] | bytes[1];

            string[] inputValues = { "A", "B", "s", "S", ">", "<", "^", "v", "R", "L", "(reset)", "", "(left)", "(right)", "(down)", "(up)" };
            
            for (int i = 0; i < 16; i++)
                if((1 & (value >> i)) == 1)
                    input += inputValues[i];

            
            return input;
        }

    #endregion

    #region "Methods"

        public void Save(string filename, ref ArrayList input)
        {
            ArrayList outputFile = new ArrayList();

            fn.bytesToArray(ref outputFile, this.fileContents, 0, (int)controllerDataOffset);

            string[] currentFrameInput = new string[3];

            for (int i = 0; i < input.Count; i++)
            {
                currentFrameInput = (string[])input[i];

                for (int j = 0; j < currentFrameInput.Length; j++)
                {
                    // check if the controller we're about to process is used
                    if (this.Options.Controllers[j])
                    {
                        byte[] parsed = parseControllerInput(currentFrameInput[j]);
                        outputFile.Add(parsed[0]); outputFile.Add(parsed[1]);
                    }
                }
            }
            
            if (filename == "") filename = this.Filename;

            FileStream fs = File.Open(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);

            // convert to 4-byte little-endian
            byte[] newFrameCount = fn.Write32(input.Count);

            // position in the target stream to insert the new values
            outputFile[0x0C] = newFrameCount[0];
            outputFile[0x0D] = newFrameCount[1];
            outputFile[0x0E] = newFrameCount[2];
            outputFile[0x0F] = newFrameCount[3];

            foreach (byte b in outputFile) writer.Write(b);

            writer.Close(); writer = null; fs.Dispose();           
        }

        private byte[] parseControllerInput(string frameInput)
        {                                      
            byte[] input = { 0x00, 0x00 };

            // frameInput will be null if a blank, inserted frame is being processed
            if (frameInput == null) return input;

            if (frameInput.Contains("A")) input[0] |= (1 << 0);
            if (frameInput.Contains("B")) input[0] |= (1 << 1);
            if (frameInput.Contains("s")) input[0] |= (1 << 2);
            if (frameInput.Contains("S")) input[0] |= (1 << 3);
            if (frameInput.Contains(">")) input[0] |= (1 << 4);
            if (frameInput.Contains("<")) input[0] |= (1 << 5);
            if (frameInput.Contains("^")) input[0] |= (1 << 6);
            if (frameInput.Contains("v")) input[0] |= (1 << 7);
            if (frameInput.Contains("R")) input[1] |= (1 << 0);
            if (frameInput.Contains("L")) input[1] |= (1 << 1);
            if (frameInput.Contains("(reset)")) input[1] |= (1 << 2);
            if (frameInput.Contains("(left)"))  input[1] |= (1 << 4);
            if (frameInput.Contains("(right)")) input[1] |= (1 << 5);
            if (frameInput.Contains("(down)"))  input[1] |= (1 << 6);
            if (frameInput.Contains("(up)"))    input[1] |= (1 << 7);
                    
            return input;
        }

    #endregion

    }
}
