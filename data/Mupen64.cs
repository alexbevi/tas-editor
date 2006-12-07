using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSplicer.Data
{
    class Mupen64
    {
        const short HEADER_SIZE = 1024;
        
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
    }
}
