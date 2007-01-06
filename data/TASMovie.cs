using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MovieSplicer.Data
{      

    public class TASMovie
    {
        // contains the file contents as an array of bytes
        protected byte[] FileContents;

        protected int    SaveStateOffset;
        protected int    ControllerDataOffset;
        
        public string     Filename;
        public TASHeader  Header;
        public TASOptions Options;
        public TASExtra   Extra;
        public TASInput   Input;

        //------------------------------------------------------------------------------------------------

        /// <summary>
        /// Common Header items
        /// </summary>
        public struct TASHeader
        {
            public string Signature;
            public int    Version;
            public string UID;
            public int    FrameCount;
            public int    RerecordCount;
            public string EmulatorID;           
        }
        
        /// <summary>
        /// Common Option items
        /// </summary>
        public struct TASOptions
        {
            public int    FPS;
            /// <summary>
            /// 0 = Start From Save, 1 = Start From Reset, 2 = Power On
            /// </summary>
            public bool[] MovieStartFlag;
            public string MovieStart
            {
                get
                {                    
                    if      (MovieStartFlag[0]) return "Reset";
                    else if (MovieStartFlag[1]) return "Save";
                    else if (MovieStartFlag[2]) return "Power On";
                    return null;
                }
            }
            /// <summary>
            /// 0 = NTSC, 1 = PAL
            /// </summary>
            public bool[] MovieTimingFlag;
            public string MovieTiming
            {
                get
                {
                    if (MovieTimingFlag[0]) return "NTSC";
                    if (MovieTimingFlag[1]) return "PAL";
                    return null;
                }
            }

            public TASOptions(bool defaultFlags)
            {
                FPS = 0;
                MovieStartFlag  = (defaultFlags) ? new bool[3] : null;
                MovieTimingFlag = (defaultFlags) ? new bool[2] : null;                
            }
        }

        /// <summary>
        /// Extra Information that exists in most movie formats
        /// </summary>
        public struct TASExtra
        {
            public string Author;
            public string Description;
            public string ROM;
            public string CRC;
            public string Country;
        }

        /// <summary>
        /// Information about the controller configuration and frame input
        /// </summary>
        public struct TASInput
        {            
            public bool[]          Controllers;
            public TASMovieInput[] FrameData;
            
            /// <summary>
            /// Get the number of active controllers
            /// </summary>
            public byte ControllerCount
            {
                get
                {
                    byte count = 0;
                    foreach (bool Controller in Controllers)
                        if (Controller) count++;
                    return count;
                }
            }

            /// <summary>
            /// Create a bool[] for the number of controllers specified by maxControllers
            /// enableControllers sets the starting values
            /// </summary>            
            public TASInput(int maxControllers, bool enableControllers)
            {                                
                Controllers = new bool[maxControllers];
                for (int i = 0; i < maxControllers; i++)
                    Controllers[i] = enableControllers;

                // FrameData is initialized later
                FrameData = null;
            }
        }

        /// <summary>
        /// overriden base method for saving back out to file
        /// </summary>        
        public virtual void Save(string filename, ref TASMovieInput[] input) { }

        //------------------------------------------------------------------------------------------------

        /// <summary>
        /// Populate a byte[] with the contents of an external file
        /// </summary> 
        protected void FillByteArrayFromFile(string filename, ref byte[] byteArray)
        {
            FileStream   fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);

            byteArray = br.ReadBytes((int)fs.Length);

            // close reader and reclaim memory
            br.Close(); br = null; fs.Dispose();
        }

        /// <summary>
        /// Write the contents of a byte[] out to file
        /// Perform Frame Update if values passed
        /// </summary>
        protected void WriteByteArrayToFile(ref byte[] outputFile, string filename, int frameCount, int frameCountPosition)
        {
            if (filename == "") filename = this.Filename;

            FileStream   fs     = File.Open(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);
            
            if (frameCount > 0 && frameCountPosition > 0)
            {
                // convert to 4-byte little-endian
                byte[] newFrameCount = Write32(frameCount);

                // position in the target stream to insert the new values
                // NOTE::This assumes a 4-byte little-endian integer
                for (int i = 0; i < 4; i++)
                    outputFile[frameCountPosition + i] = newFrameCount[i];
            }

            foreach (byte b in outputFile) writer.Write(b);

            writer.Close(); writer = null; fs.Dispose();
        }
        
        /// <summary>
        /// Convert a 2-byte little endian integer
        /// </summary>
        protected UInt16 Read16(byte[] byteArray)
        {
            return Convert.ToUInt16(byteArray[0] | (byteArray[1] << 8));
        }

        /// <summary>
        /// Convert a byte[] to a 4-byte little endian integer
        /// </summary>
        protected int Read32(ref byte[] byteArray, int position)
        {            
            return Convert.ToInt32(  (byteArray[position] 
                                   | (byteArray[position+1] << 8) 
                                   | (byteArray[position+2] << 16)  
                                   | (byteArray[position+3] << 24)));
        }       

        /// <summary>
        /// Convert an integer to a 4-byte little endian byte array
        /// </summary>
        protected byte[] Write32(int value)
        {
            byte[] flippedBytes = new byte[4];
            flippedBytes[0] = (byte)(value & 0xFF);
            flippedBytes[1] = (byte)((value >> 8) & 0xFF);
            flippedBytes[2] = (byte)((value >> 16) & 0xFF);
            flippedBytes[3] = (byte)((value >> 24) & 0xFF);
            return flippedBytes;
        }

        /// <summary>
        /// Convert a byte array into a string of HEX characters
        /// </summary>
        public static string ReadHEX(ref byte[] byteArray, int position, int length)
        {
            string s = "";
            for (int i = 0; i < length; i++)
                s += byteArray[position + i].ToString("X");
            return s;
        }

        /// <summary>
        /// Convert a byte array into a string of unicode HEX characters
        /// </summary>
        protected string ReadHEXUnicode(ref byte[] byteArray, int position, int length)
        {
            string s = "";
            for (int i = position + length; i >= position; i--)
                s += byteArray[i].ToString("X");
            return s;
        }

        /// <summary>
        /// Convert a byte array to UTF-8 encoded character array to a string
        /// </summary>
        protected string ReadChars(ref byte[] byteArray, int position, int length)
        {
            char[] c  = new char[length];
            Decoder d = Encoding.UTF8.GetDecoder();
            d.GetChars(byteArray, position, length, c, 0);
            return (new string(c));
        }

        /// <summary>
        /// Convert byte array to UTF-16 encoded character array to a string
        /// </summary>
        protected string ReadChars16(ref byte[] byteArray, int position, int length)
        {
            char[]  c = new char[length];
            Decoder d = Encoding.Unicode.GetDecoder();
            d.GetChars(byteArray, position, length, c, 0);
            return (new string(c));
        }

        /// <summary>
        /// Get the null terminator position in a byte array
        /// </summary>        
        protected string ReadCharsNullTerminated(ref byte[] byteArray, int position)
        {
            const byte NULL_TERMINATOR = 0x00;
            int length = 0;
            while (byteArray[position + length] != NULL_TERMINATOR) length++;
            return ReadChars(ref byteArray, position, length);
        }

        /// <summary>
        /// Convert a UTC file timestamp to a local string representation        
        /// </summary>
        protected string ConvertUNIXTime(int timeStamp)
        {
            // First make a System.DateTime equivalent to the UNIX Epoch.
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds(timeStamp);

            // The dateTime now contains the right date/time so to format the string,
            // use the standard formatting methods of the DateTime object.
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }

        /// <summary>
        /// Read a fixed number of bytes from the byte array
        /// </summary>
        protected byte[] ReadBytes(ref byte[] byteArray, int offset, int length)
        {
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
                bytes[i] = byteArray[i + offset];
            return bytes;
        }
               
        /// <summary>
        /// Add the passed value to the end of the byteArray
        /// 
        /// NOTE::This method really causes a slowdown on larger files.
        /// </summary>        
        //protected void AddBytes(ref byte[] byteArray, byte[] value)
        //{
        //    byte[] temp = new byte[byteArray.Length + value.Length];
                        
        //    byteArray.CopyTo(temp, 0);
        //    value.CopyTo(temp, byteArray.Length);

        //    byteArray = null; byteArray = temp; temp = null;
        //}
    }
}
