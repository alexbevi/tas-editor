using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MovieSplicer.Data.Formats
{
    public class Mupen64 : TASMovie
    {
        /// <summary>
        /// Contains Format Specific items
        /// </summary>
        public struct FormatSpecific
        {
            public ControllerConfig[] Controller;
            public string             VideoPlugin;
            public string             AudioPlugin;
            public string             InputPlugin;
            public string             RSPPlugin;

            public FormatSpecific(byte[] controllers)
            {
                Controller = new ControllerConfig[4];
                for (int i = 0; i < 4; i++)
                {
                    Controller[i].Option = new bool[3];
                    if ((1 & (controllers[i] << 0)) == 1) Controller[i].Option[0] = true;
                    if ((1 & (controllers[i] << 4)) == 1) Controller[i].Option[1] = true;
                    if ((1 & (controllers[i] << 8)) == 1) Controller[i].Option[2] = true;                    
                }

                // directly initialized when instantiated
                VideoPlugin = null;
                AudioPlugin = null;
                InputPlugin = null;
                RSPPlugin   = null;
            }
        }

        /// <summary>
        /// The Option (bool array) contains configuration information about a controller                
        /// </summary>
        public struct ControllerConfig
        {
            public bool[] Option;
        }

        const short HEADER_SIZE = 1024;

        public Header         M64Header;
        public Options        M64Options;
        public Extra          M64Extra;
        public Input          M64Input;
        public FormatSpecific M64Specific;
       
        static int[] Offsets = {
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
        /// Create a fully instantiated M64 object from the passed file
        /// </summary>        
        public Mupen64(string M64File)
        {
            Filename = M64File;
            FillByteArrayFromFile(M64File, ref FileContents);

            ControllerDataOffset = Read32(ref FileContents, Offsets[10]);

            M64Header = new Header();
            M64Header.Signature     = ReadHEX(ref FileContents, Offsets[0], 4);
            M64Header.Version       = Read32(ref FileContents, Offsets[1]);
            M64Header.UID           = ConvertUNIXTime(Read32(ref FileContents, Offsets[2]));
            M64Header.FrameCount    = Read32(ref FileContents, Offsets[3]);
            M64Header.RerecordCount = Read32(ref FileContents, Offsets[4]);

            M64Options = new Options(true);
            M64Options.MovieStartFlag[0] = (FileContents[Offsets[7]] | FileContents[Offsets[7] + 1]) == 1 ? true : false;
            M64Options.MovieStartFlag[1] = (FileContents[Offsets[7]] | FileContents[Offsets[7] + 1]) == 2 ? true : false;            
            M64Options.FPS               = FileContents[Offsets[5]];

            M64Extra = new Extra();
            M64Extra.ROM         = ReadChars(ref FileContents, Offsets[13], 64);
            M64Extra.CRC         = ReadHEXUnicode(ref FileContents, Offsets[14], 4);
            M64Extra.Country     = ReadChars(ref FileContents, Offsets[15], 2);
            M64Extra.Author      = ReadChars(ref FileContents, Offsets[21], 222);
            M64Extra.Description = ReadChars(ref FileContents, Offsets[22], 256);

            M64Specific = new FormatSpecific(ReadBytes(ref FileContents, Offsets[11], 4));
            M64Specific.VideoPlugin = ReadChars(ref FileContents, Offsets[17], 64);
            M64Specific.AudioPlugin = ReadChars(ref FileContents, Offsets[18], 64);
            M64Specific.InputPlugin = ReadChars(ref FileContents, Offsets[19], 64);
            M64Specific.RSPPlugin   = ReadChars(ref FileContents, Offsets[20], 64);
            

            //M64Input = new Input(FileContents[Offsets[6]], false);            
            M64Input = new Input();
            //InputSampleCount = fn.Read32(fn.readBytes(ref byteArray, Offsets[6], 4));

        }                         
    }    
}
