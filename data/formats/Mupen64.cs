using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MovieSplicer.Data
{
    public class Mupen64
    {
        const short HEADER_SIZE = 1024;

        private byte[] fileContents;
       
        public string       Filename;
        public M64Header    Header;
        public M64Options   Options;
        public M64RomInfo   RomInfo;
        public M64ExtraInfo ExtraInfo;

        static Functions fn  = new Functions();
        static int[] offsets = {
          0x00, // 4-byte signature: 4D 36 34 1A "M64\x1A"
          0x04, // 4-byte little-endian unsigned int: version number, should be 3
          0x08, // 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship,
                //      also used as the recording time in Unix epoch format
          0x0C, // 4-byte little-endian unsigned int: number of frames (vertical interrupts)
          0x10, // 4-byte little-endian unsigned int: rerecord count
          0x14, // 1-byte unsigned int: frames (vertical interrupts) per second
          0x15, // 1-byte unsigned int: number of controllers
          0x16, // 2-byte unsigned int: reserved, should be 0
          0x18, // 4-byte little-endian unsigned int: number of input samples for any controllers
          0x1C, // 2-byte unsigned int: movie start type
                //      value 1: movie begins from snapshot (the snapshot will be loaded from an external file
                //               with the movie's filename and a .st extension)
                //      value 2: movie begins from poweron  
                //      other values: invalid movie
          0x1E, // 2-byte unsigned int: reserved, should be 0
          0x20, // 4-byte unsigned int: controller flags
                //      bit 0: controller 1 present
                //      bit 4: controller 1 has mempak
                //      bit 8: controller 1 has rumblepak
                //      +1..3 for controllers 2..4.
          0x24, // 160 bytes: reserved, should be 0
          0xC4, // 32-byte ASCII string: internal name of ROM used when recording, directly from ROM
          0xE4, // 4-byte unsigned int: CRC32 of ROM used when recording, directly from ROM
          0xE8, // 2-byte unsigned int: country code of ROM used when recording, directly from ROM
          0xEA, // 56 bytes: reserved, should be 0
          0x122,// 64-byte ASCII string: name of video plugin used when recording, directly from plugin
          0x162,// 64-byte ASCII string: name of sound plugin used when recording, directly from plugin
          0x1A2,// 64-byte ASCII string: name of input plugin used when recording, directly from plugin
          0x1E2,// 64-byte ASCII string: name of rsp plugin used when recording, directly from plugin
          0x222,// 222-byte UTF-8 string: author's name info
          0x300 // 256-byte UTF-8 string: author's movie description info
        };

        /// <summary>
        /// Create a fully instantiated VBM object from the passed file
        /// </summary>        
        public Mupen64(string M64File)
        {
            Filename = M64File;

            FileStream fs   = File.OpenRead(M64File);
            BinaryReader br = new BinaryReader(fs);
            
            fileContents = br.ReadBytes((int)fs.Length);

            br.Close(); br = null; fs.Dispose();

            Header    = new M64Header(ref fileContents);
            Options   = new M64Options(ref fileContents);
            RomInfo   = new M64RomInfo(ref fileContents);
            ExtraInfo = new M64ExtraInfo(ref fileContents);
        }

        /// <summary>
        /// Parse basic M64 header values
        /// </summary>
        public class M64Header
        {
            public string Signature;
            public uint   Version;
            public string UID;
            public uint   FrameCount;
            public uint   RerecordCount;

            public M64Header(ref byte[] byteArray)
            {
                Signature     = fn.ReadHEX(fn.readBytes(ref byteArray, offsets[0], 4));
                Version       = fn.Read32(fn.readBytes(ref byteArray, offsets[1], 4));
                UID           = fn.ConvertUNIXTime((fn.Read32(fn.readBytes(ref byteArray, offsets[2], 4))));
                FrameCount    = fn.Read32(fn.readBytes(ref byteArray, offsets[3], 4));
                RerecordCount = fn.Read32(fn.readBytes(ref byteArray, offsets[4], 4));
            }
        }

        /// <summary>
        /// Parse basic M64 header option values
        /// </summary>
        public class M64Options
        {                                      
            public int       FPS;
            public int       ControllerCount;
            public uint      InputSampleCount;
            public bool[]    MovieStart = { false, false };
            public ArrayList Controllers;                       

            public M64Options(ref byte[] byteArray)
            {
                FPS = byteArray[offsets[5]];
                ControllerCount = byteArray[offsets[6]];
                //InputSampleCount = fn.Read32(fn.readBytes(ref byteArray, offsets[6], 4));

                int startFlag = byteArray[offsets[7]] | byteArray[offsets[7] + 1];
                if (startFlag == 1) MovieStart[0] = true;
                if (startFlag == 2) MovieStart[1] = true;

                Controllers = new ArrayList();
                byte[] controllers = fn.readBytes(ref byteArray, offsets[8], 4);
                for (int i = 0; i < 4; i++)
                {
                    bool[] ControllerFlags = { false, false, false };

                    if ((1 & (controllers[i] << 0)) == 1) ControllerFlags[0] = true;
                    if ((1 & (controllers[i] << 4)) == 1) ControllerFlags[0] = true;
                    if ((1 & (controllers[i] << 8)) == 1) ControllerFlags[0] = true;

                    Controllers.Add(ControllerFlags);
                }

            }
        }

        /// <summary>
        /// Parse basic M64 header ROM info values
        /// </summary>
        public class M64RomInfo
        {
            public string Name;
            public string CRC;
            public string Country;

            public M64RomInfo(ref byte[] byteArray)
            {
                Name    = fn.ReadChars(fn.readBytes(ref byteArray, offsets[13], 32));
                CRC     = fn.ReadHEXUnicode(fn.readBytes(ref byteArray, offsets[14], 4));
                Country = fn.ReadChars(fn.readBytes(ref byteArray, offsets[15], 2));
            }            
        }

        /// <summary>
        /// Parse additional M64 header information values
        /// </summary>
        public class M64ExtraInfo
        {
            public string VideoPlugin;
            public string AudioPlugin;
            public string InputPlugin;
            public string RSPPlugin;
            public string Author;
            public string Description;

            public M64ExtraInfo(ref byte[] byteArray)
            {
                VideoPlugin = fn.ReadChars(fn.readBytes(ref byteArray, offsets[17], 64));
                AudioPlugin = fn.ReadChars(fn.readBytes(ref byteArray, offsets[18], 64));
                InputPlugin = fn.ReadChars(fn.readBytes(ref byteArray, offsets[19], 64));
                RSPPlugin   = fn.ReadChars(fn.readBytes(ref byteArray, offsets[20], 64));
                Author      = fn.ReadChars(fn.readBytes(ref byteArray, offsets[21], 222));
                Description = fn.ReadChars(fn.readBytes(ref byteArray, offsets[22], 256));

            }
        }
    
}    
}
