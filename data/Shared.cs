using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

using MovieSplicer.Data.Structures;
/***************************************************************************************************
 * Shared Functions, Collections and Properties
 **************************************************************************************************/
namespace MovieSplicer
{                
    /// <summary>
    /// General Purpose Functions
    /// 
    /// TODO::If all functions are delcared as static, I won't have to instantiate a Functions object,
    /// but then all calls would be longer (line length-wise). Not sure if it's worth it.
    /// </summary>      
    public class Functions
    {
        // OpenFileDialog and SaveFileDialog filters (this isn't the most elegant way to share data)
        public string ALL_FILTER = "All Supported Movie Formats (*.*) | *.smv;*.fcm;*.gmv;*.fmv;*.vbm;*.m64";
        public string SMV_FILTER = "SNES9x Movie (*.smv)|*.smv";
        public string FCM_FILTER = "FCE Ultra Movie (*.fcm)|*.fcm";
        public string GMV_FILTER = "Gens Movie (*.gmv)|*.gmv";
        public string FMV_FILTER = "Famtasia Movie (*.fmv)|*.fmv";
        public string VBM_FILTER = "VisualBoyAdvance Movie (*.vbm)|*.vbm";
        public string M64_FILTER = "Mupen64 Movie (*.m64)|*.m64";
            
        /// <summary>
        /// Convert a 2-byte little endian integer
        /// </summary>
        public UInt16 Read16(byte[] byteArray)
        {           
           return Convert.ToUInt16(byteArray[0] | (byteArray[1] << 8));            
        }

        /// <summary>
        /// Convert a 4-byte little endian integer
        /// </summary>
        public UInt32 Read32(byte[] byteArray)
        {
            UInt32 flippedBytes = Convert.ToUInt32((byteArray[0] | (byteArray[1] << 8) |
                (byteArray[2] << 16) | (byteArray[3] << 24)));
            
            return flippedBytes;
        }

        /// <summary>
        /// Convert an integer to a 4-byte little endian byte array
        /// </summary>
        public byte[] Write32(int value)
        {
            byte[] flippedBytes = new byte[4];
            flippedBytes[0] = (byte)(value & 0xFF);
            flippedBytes[1] = (byte)((value >> 8)  & 0xFF);
            flippedBytes[2] = (byte)((value >> 16) & 0xFF);
            flippedBytes[3] = (byte)((value >> 24) & 0xFF);
            return flippedBytes;
        }

        /// <summary>
        /// Convert a byte array into a string of HEX characters
        /// </summary>
        public string ReadHEX(byte[] byteArray)
        {
            string s = "";
            for (int i = 0; i < byteArray.Length; i++)
                s += byteArray[i].ToString("X");
            return s;
        }

        /// <summary>
        /// Convert a byte array into a string of unicode HEX characters
        /// </summary>
        public string ReadHEXUnicode(byte[] byteArray)
        {
            string s = "";
            for (int i = byteArray.Length - 1; i >= 0; i--)
                s += byteArray[i].ToString("X");
            return s;
        }

        /// <summary>
        /// Convert a byte array to UTF-8 encoded character array to a string
        /// </summary>
        public string ReadChars(byte[] byteArray)
        {
            char[]  c = new char[byteArray.Length];            
            Decoder d = Encoding.UTF8.GetDecoder();
            d.GetChars(byteArray, 0, byteArray.Length, c, 0);
            return (new string(c));
        }

        /// <summary>
        /// Convert byte arry to UTF-16 encoded character array to a string
        /// </summary>
        public string ReadChars16(byte[] byteArray)
        {
            char[]  c = new char[byteArray.Length];
            Decoder d = Encoding.Unicode.GetDecoder();
            d.GetChars(byteArray, 0, byteArray.Length, c, 0);
            return (new string(c));
        }

        /// <summary>
        /// Read a fixed number of bytes from the byte array
        /// </summary>
        public byte[] readBytes(ref byte[] byteArray, int offset, int length)
        {
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
                bytes[i] = byteArray[i + offset];
            return bytes;
        }

        /// <summary>
        /// Read a fixed number of bytes from an array and add them to an arraylist
        /// </summary>
        public void bytesToArray(ref ArrayList targetArray, byte[] sourceByteArray, int position, int length)
        {
            for (int i = 0; i < length; i++)
                targetArray.Add(sourceByteArray[position + i]);
        } 
   
        /// <summary>
        /// Iterates through a byte array and returns the distance from the offset of the 
        /// first null (0x00) positon (length of string)
        /// </summary>
        public int seekNullPosition(byte[] byteArray, int offset)
        {
            const byte NULL_TERMINATOR = 0x00;
            int i = 0;
            while (byteArray[offset + i] != NULL_TERMINATOR) i++;
            return i;
        }

        /// <summary>
        /// Check if a string value is numeric
        /// </summary>
        public bool IsNumeric(string value)
        {
            try   { Int32.Parse(value); return true; }
            catch { return false; }
        }

        /// <summary>
        /// Extract the filename from a file path
        /// </summary>
        public string extractFilenameFromPath(string pathToFile)
        {
            char[]   splitter = { '\\' };
            string[] filePath;
            filePath = pathToFile.Split(splitter);
            return filePath[filePath.Length - 1];
        }

        /// <summary>
        /// Convert a UTC file timestamp to a local string representation
        /// 
        /// DEBUG::I'm not sure that this is actually working properly
        /// </summary>
        public string ConvertUNIXTime(uint timeStamp)
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
        /// Read first 4-bytes from file
        /// </summary>
        public UInt32 ReadHeader(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);
            byte[] byteArray = br.ReadBytes(4);

            br.Close(); br = null; fs.Dispose();

            return UInt32.Parse((ReadHEX(byteArray)), NumberStyles.HexNumber);
        }

        /// <summary>
        /// Verify that a file is a valid movie
        /// </summary>
        public MovieType IsValid(string filename)
        {
            const uint SMV = 0x534D561A;
            const uint FCM = 0x46434D1A;
            const uint GMV = 0x47656E73;
            const uint FMV = 0x464D561A;
            const uint VBM = 0x56424D1A;
            const uint M64 = 0x4D36341A;

            uint signature = ReadHeader(filename);

            switch(signature)
            {
                case SMV:
                    return MovieType.SMV; 
                case FCM:
                    return MovieType.FCM;
                case GMV:
                    return MovieType.GMV; 
                case FMV:
                    return MovieType.FMV; 
                case VBM:
                    return MovieType.VBM;
                case M64:
                    return MovieType.M64; 
                default:
                    return MovieType.None; 
            }
        }

    }   
}
