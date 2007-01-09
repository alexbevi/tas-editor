using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

using MovieSplicer.Data;
using MovieSplicer.Data.Formats;

namespace MovieSplicer.Components
{
    public class TASForm : Form
    {
        public const string APP_TITLE  = "TAS Movie Editor";
        public const string VERSION    = "0.9.2";

        public const string TAS_FILTER = ALL_FILTER + "|" + FCM_FILTER + "|" + FMV_FILTER + "|" + 
                                         GMV_FILTER + "|" + M64_FILTER + "|" + SMV_FILTER + "|" + 
                                         VBM_FILTER;
        public const string ALL_FILTER = "All Supported Movie Formats (*.*) | *.smv;*.fcm;*.gmv;*.fmv;*.vbm;*.m64";
        public const string FCM_FILTER = "FCM - FCE Ultra Movie|*.fcm";
        public const string FMV_FILTER = "FMV - Famtasia Movie|*.fmv";
        public const string GMV_FILTER = "GMV - Gens Movie|*.gmv";
        public const string M64_FILTER = "M64 - Mupen64 Movie|*.m64";
        public const string SMV_FILTER = "SMV - SNES9x Movie|*.smv";
        public const string VBM_FILTER = "VBM - VisualBoyAdvance Movie|*.vbm";

        protected OpenFileDialog openDlg;
        protected SaveFileDialog saveDlg;

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
        
        //--- Functions ----------------------------------------------------------------------------------

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

        /// <summary>
        /// Return a format specific object
        /// </summary>        
        public TASMovie LoadMovie(string filename, MovieType fileType)
        {            
            switch (fileType)
            {
                case MovieType.SMV:
                    return new SNES9x(filename);
                case MovieType.FCM:
                    return new FCEU(filename);
                case MovieType.GMV:
                    return new Gens(filename);
                case MovieType.FMV:
                    return new Famtasia(filename);
                case MovieType.VBM:
                    return new VisualBoyAdvance(filename);
                case MovieType.M64:
                    return new Mupen64(filename);                                
            }
            return null;
        }
    }
}
