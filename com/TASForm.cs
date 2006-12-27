using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

using MovieSplicer.Data;

namespace MovieSplicer.Components
{
    public class TASForm : Form
    {
        public const string TAS_FILTER = ALL_FILTER + "|" + SMV_FILTER + "|" + FCM_FILTER + "|" + 
                                         GMV_FILTER + "|" + FMV_FILTER + "|" + VBM_FILTER + "|" + 
                                         M64_FILTER;
        public const string ALL_FILTER = "All Supported Movie Formats (*.*) | *.smv;*.fcm;*.gmv;*.fmv;*.vbm;*.m64";
        public const string SMV_FILTER = "SNES9x Movie (*.smv)|*.smv";
        public const string FCM_FILTER = "FCE Ultra Movie (*.fcm)|*.fcm";
        public const string GMV_FILTER = "Gens Movie (*.gmv)|*.gmv";
        public const string FMV_FILTER = "Famtasia Movie (*.fmv)|*.fmv";
        public const string VBM_FILTER = "VisualBoyAdvance Movie (*.vbm)|*.vbm";
        public const string M64_FILTER = "Mupen64 Movie (*.m64)|*.m64";

        /// <summary>
        /// The supported movie formats list
        /// </summary>
        public enum MovieType
        {
            None = 0,
            SMV = 1,
            FCM = 2,
            GMV = 3,
            FMV = 4,
            VBM = 5,
            M64 = 6
        }

        /// <summary>
        /// Lists the different kinds of splicing options that can be
        /// applied to a movie
        /// </summary>
        public enum SpliceOptions
        {
            FromStart = 1,
            Range = 2,
            ToEnd = 3
        }

        /// <summary>
        /// Check if a string value is numeric
        /// </summary>
        public bool IsNumeric(string value)
        {
            try { Int32.Parse(value); return true; }
            catch { return false; }
        }

        /// <summary>
        /// Extract the filename from a file path
        /// </summary>
        public string FilenameFromPath(string pathToFile)
        {
            char[]   splitter = { '\\' };
            string[] filePath;
            filePath = pathToFile.Split(splitter);
            return filePath[filePath.Length - 1];
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

            return UInt32.Parse((Data.TASMovie.ReadHEX(ref byteArray, 0, 4)), NumberStyles.HexNumber);
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

            switch (signature)
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
