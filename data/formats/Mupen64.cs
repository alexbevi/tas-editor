/******************************************************************************** 
 * TAS Movie Editor                                                             *
 *                                                                              *
 * Copyright notice for this file:                                              *
 *  Copyright (C) 2006-7 Maximus                                                *
 *                                                                              *
 * This program is free software; you can redistribute it and/or modify         *
 * it under the terms of the GNU General Public License as published by         *
 * the Free Software Foundation; either version 2 of the License, or            *
 * (at your option) any later version.                                          *
 *                                                                              *
 * This program is distributed in the hope that it will be useful,              *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
 * GNU General Public License for more details.                                 *
 *                                                                              *
 * You should have received a copy of the GNU General Public License            *
 * along with this program; if not, write to the Free Software                  *
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA    *
 *******************************************************************************/
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
            public string VideoPlugin;
            public string AudioPlugin;
            public string InputPlugin;
            public string RSPPlugin;            

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
                RSPPlugin = null;
            }
        }

        /// <summary>
        /// The Option (bool array) contains configuration information about a controller                
        /// </summary>
        public struct ControllerConfig
        {
            public bool[] Option;
        }

        const short HEADER_SIZE     = 1024;
        const byte  BYTES_PER_FRAME = 4;

        public FormatSpecific M64Specific;

        private string[] InputValues = { "D", "S", "Z", "B", "A", "C", "R", "L", "X", "Y" };
        private string[] InputDir    = { ">", "<", "v", "^" };
        private int      ControllerInputs;
        private int[]    Offsets = {
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
            
            Header = new TASHeader();
            Header.Signature     = ReadHEX(ref FileContents, Offsets[0], 4);
            Header.Version       = Read32(ref FileContents, Offsets[1]);
            Header.UID           = ConvertUNIXTime(Read32(ref FileContents, Offsets[2]));
            Header.FrameCount    = Read32(ref FileContents, Offsets[3]);
            Header.RerecordCount = Read32(ref FileContents, Offsets[4]);

            // Though ControllerInputs is stored in the header, it can be determined 
            // mathematically, so we may as well do it once instead of twice ;)
            //ControllerInputs     = Read32(ref FileContents, Offsets[8]);

            if (Header.Version >= 3)
            {
                ControllerInputs = (FileContents.Length - 0x400) / BYTES_PER_FRAME;
                ControllerDataOffset = 0x400;
            }
            else
            {
                ControllerInputs = (FileContents.Length - 0x200) / BYTES_PER_FRAME;
                ControllerDataOffset = 0x200;
            }

            Options = new TASOptions(true);
            Options.MovieStartFlag[0] = (FileContents[Offsets[9]] | FileContents[Offsets[9] + 1]) == 2 ? true : false;
            Options.MovieStartFlag[1] = (FileContents[Offsets[9]] | FileContents[Offsets[9] + 1]) == 1 ? true : false;
            Options.FPS = FileContents[Offsets[5]];

            Extra = new TASExtra();
            Extra.ROM         = ReadChars(ref FileContents, Offsets[13], 64);
            Extra.CRC         = ReadHEXUnicode(ref FileContents, Offsets[14], 4);
            Extra.Country     = ReadChars(ref FileContents, Offsets[15], 2);
            Extra.Author      = ReadChars(ref FileContents, Offsets[21], 222);
            Extra.Description = ReadChars(ref FileContents, Offsets[22], 256);

            M64Specific = new FormatSpecific(ReadBytes(ref FileContents, Offsets[11], 4));
            M64Specific.VideoPlugin = ReadChars(ref FileContents, Offsets[17], 64);
            M64Specific.AudioPlugin = ReadChars(ref FileContents, Offsets[18], 64);
            M64Specific.InputPlugin = ReadChars(ref FileContents, Offsets[19], 64);
            M64Specific.RSPPlugin   = ReadChars(ref FileContents, Offsets[20], 64);

            Input = new TASInput(4, false);
            for (int i = 0; i < 4; i++)
                Input.Controllers[i] = M64Specific.Controller[i].Option[0];                       

            getFrameInput(ref FileContents);
        }

        private void getFrameInput(ref byte[] byteArray)
        {
            // NOTE::ControllerInputs seems to always be 1/2 of the FrameCount
            Input.FrameData = new TASMovieInput[ControllerInputs];                        

            // parse frame data
            for (int i = 0; i < ControllerInputs; i++)            
            {
                Input.FrameData[i] = new TASMovieInput();
               
                // cycle through the controller data for the current frame
                for (int j = 0; j < Input.ControllerCount; j++)
                {
                    byte[] frame = ReadBytes(ref byteArray,
                        ControllerDataOffset + (i * BYTES_PER_FRAME) + (j * BYTES_PER_FRAME),
                        BYTES_PER_FRAME);

                    Input.FrameData[i].Controller[j] = parseControllerData(frame);
                }                   
            }                       
        } 
     
        /// <summary>
        /// Convert the binary representation of input to meaningful values 
        /// </summary>
        private string parseControllerData(byte[] byteArray)
        {
            bool[] D = { false, false, false, false };
            bool[] C = { false, false, false, false };
            Int32  x = 0;
            Int32  y = 0;

            string frame = "";

            D[0] = (byteArray[0] & (0x01)) != 0;
            D[1] = (byteArray[0] & (0x02)) != 0;
            D[2] = (byteArray[0] & (0x04)) != 0;
            D[3] = (byteArray[0] & (0x08)) != 0;
            if ((byteArray[0] & (0x10)) != 0) frame += InputValues[1];
            if ((byteArray[0] & (0x20)) != 0) frame += InputValues[2];
            if ((byteArray[0] & (0x40)) != 0) frame += InputValues[3];
            if ((byteArray[0] & (0x80)) != 0) frame += InputValues[4];
            C[0] = (byteArray[1] & (0x01)) != 0;
            C[1] = (byteArray[1] & (0x02)) != 0;
            C[2] = (byteArray[1] & (0x04)) != 0;
            C[3] = (byteArray[1] & (0x08)) != 0;
            if ((byteArray[1] & (0x10)) != 0) frame += InputValues[6];
            if ((byteArray[1] & (0x20)) != 0) frame += InputValues[7];
            x = byteArray[2] & (0xFF);
            y = byteArray[3] & (0xFF);

            if (D[0] || D[1] || D[2] || D[3])
            {
                frame += "(" + InputValues[0];
                for (int i = 0; i < 4; i++)
                    if (D[i]) frame += InputDir[i];
                frame += ")";
            }
            if (C[0] || C[1] || C[2] || C[3])
            {
                frame += "(" + InputValues[5];
                for (int i = 0; i < 4; i++)
                    if (C[i]) frame += InputDir[i];
                frame += ")";
            }
            
            int xamt = (x < 0 ? -x : x) * 99 / 127; if (xamt == 0 && x != 0) xamt = 1;
            int yamt = (y < 0 ? -y : y) * 99 / 127; if (yamt == 0 && y != 0) yamt = 1;
                        
            if (x != 0)
            {
                frame += InputValues[8];
                frame += (x < 0) ? InputDir[0] : InputDir[1];
                frame += (y < 0) ? InputDir[2] : InputDir[3];
                frame += x.ToString(); // DEBUGGING (append value)
            }
            if (y != 0)
            {
                frame += InputValues[9];                
                frame += (y < 0) ? InputDir[2] : InputDir[3];
                frame += y.ToString(); // DEBUGGING (append value)
            }
            
            return frame;

        }
    }    
}
