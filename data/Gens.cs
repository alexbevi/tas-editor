using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
/***************************************************************************************************
 * Read and parse a Gens GMV file
 **************************************************************************************************/
namespace MovieSplicer.Data
{
    public class Gens
    {        
        const byte HEADER_SIZE = 64;
        
        private byte[] fileContents;
        
        public string            Filename;
        public GMVHeader         Header;
        public GMVOptions        Options;
        public GMVControllerData ControllerData;

        static Functions fn = new Functions();
        static int[] offsets = {
            0x00, // 16-byte signature and format version: "Gens Movie TEST9"
            0x0F, // ASCII-encoded GMV file format version. The most recent is 'A'. (?)
            0x10, // 4-byte little-endian unsigned int: rerecord count
            0x14, // ASCII-encoded controller config for player 1. '3' or '6'.
            0x15, // ASCII-encoded controller config for player 2. '3' or '6'.
            0x16, // has some special flags (Version A and up only):
                  //    bit 7 (most significant) Frames per second. 1 for 50 Hz, 0 for 60Hz.
                  //    bit 6 is savestate required. 1 = savestate required.
                  //    bit 5 is 3 player movie. 1 = 3 players, 0 = 2 players.
            0x18, // 40-byte zero-terminated ascii movie name string
            0x40  // frame data begins here           
        };
     
        /// <summary>
        /// Create a fully instantiated GMV object from the file that is passed in
        /// </summary>        
        public Gens(string GMVFile)
        {
            Filename = GMVFile;

            FileStream   fs = File.OpenRead(GMVFile);
            BinaryReader br = new BinaryReader(fs);

            // read the GMV file into a byte array
            fileContents = br.ReadBytes((int)fs.Length);

            br.Close(); br = null; fs.Dispose();

            Header = new GMVHeader(ref fileContents);
            
            // options only set if version A or above
            if (Header.Version > 0x09)
            {
                Options = new GMVOptions(Header.Version, Header.Options);
            }

            ControllerData = new GMVControllerData(ref fileContents, Header.FrameCount);
        }    

    #region "Structure"

        /// <summary>
        /// Parse the GMV file's header      
        /// </summary>
        public class GMVHeader
        {
            public string Signature;
            public int    Version;            
            public uint   RerecordCount;
            public string Player1Config;
            public string Player2Config;            
            public string MovieName;            
            public int    FrameCount;
            public byte   Options;            
        
            public GMVHeader(ref byte[] byteArray)
            {
                Signature      = fn.ReadChars(fn.readBytes(ref byteArray, offsets[0], 16));             
                
                // hack to convert ASCII Ucase value to HEX string equivalent (ASCII A == 65)
                Version        = fn.readBytes(ref byteArray, offsets[1], 1)[0] - 55;
                
                RerecordCount  = fn.Read32(fn.readBytes(ref byteArray, offsets[2], 4));
                Player1Config  = fn.ReadChars(fn.readBytes(ref byteArray, offsets[3], 1));
                Player2Config  = fn.ReadChars(fn.readBytes(ref byteArray, offsets[4], 1)); 
                
                int length;
                // if the movie name is the max length of allowable input, assign it as such
                try   { length = fn.seekNullPosition(byteArray, offsets[6]); }
                catch { length = offsets[7] - offsets[6]; }
                MovieName      = fn.ReadChars(fn.readBytes(ref byteArray, offsets[6], length));

                FrameCount     = (byteArray.Length - HEADER_SIZE) / 3;
                Options        = fn.readBytes(ref byteArray, offsets[5], 1)[0]; 
            }
        }
        
        /// <summary>
        /// Parse the GMV file's Options (if available)
        /// </summary>
        public class GMVOptions
        {           
            public string FPS;           
            public bool   StartFromReset;           
            public bool   StartFromSave;
            public int    ControllerCount;
            
            public GMVOptions(int version, byte options)
            {
                // options only supported by GMV version A and above
                if (version > 0x09)
                {
                    FPS             = (1 & (options >> 7)) == 1 ? "50" : "60";
                    StartFromSave   = (1 & (options >> 6)) == 1 ? true : false;
                    StartFromReset  = (1 & (options >> 6)) == 0 ? true : false;
                    ControllerCount = (1 & (options >> 5)) == 1 ? 3 : 2;
                }
            }

        }

        /// <summary>
        /// Parse the GMV file's input by frame
        /// 
        /// TODO::Doesn't parse XYZMode data for 2-player 6-button data
        /// </summary>
        public class GMVControllerData
        {
            public ArrayList ControllerInput;            
            
            public GMVControllerData(ref byte[] byteArray, int frameCount)
            {
                ControllerInput = new ArrayList();
                string[] input;

                // there are 3 bytes per frame of input
                for (int i = 0; i < frameCount * 3; i++)
                {
                    input = new string[3];
                    
                    // there are only 3 bytes to deal with, so no need to 
                    // run the array through a loop to be populated
                    input[0] = parseControllerData(byteArray[offsets[7] + i]);
                    input[1] = parseControllerData(byteArray[offsets[7] + i + 1]);
                    input[2] = parseControllerData(byteArray[offsets[7] + i + 2]);

                    ControllerInput.Add(input);
                    i += 2;
                }
            }

            /// <summary>
            /// Convert byte values to string input
            /// </summary>
            private string parseControllerData(byte frameInput)
            {                
                string input = "";
             
                if (((1 & frameInput >> 0)) == 0) input += "^";
                if (((1 & frameInput >> 1)) == 0) input += "v";
                if (((1 & frameInput >> 2)) == 0) input += "<";
                if (((1 & frameInput >> 3)) == 0) input += ">";
                if (((1 & frameInput >> 4)) == 0) input += "A";
                if (((1 & frameInput >> 5)) == 0) input += "B";
                if (((1 & frameInput >> 6)) == 0) input += "C";
                if (((1 & frameInput >> 7)) == 0) input += "S";

                return input;
            }
        }

    #endregion

    #region "Methods"

        /// <summary>
        /// Save a new GMV file (if a string null is passed, prompt for overwrite of the source file)
        /// </summary>        
        public void Save(string filename, ref ArrayList input)
        {
            ArrayList outputFile = new ArrayList();

            fn.bytesToArray(ref outputFile, this.fileContents, 0, 64);
            string[] currentFrameInput = new string[3];

            for (int i = 0; i < input.Count; i++)
            {
                currentFrameInput = (string[])input[i];
                
                outputFile.Add(parseControllerData(currentFrameInput[0]));
                outputFile.Add(parseControllerData(currentFrameInput[1]));
                
                if (this.Options.ControllerCount < 3)
                    outputFile.Add(parseControllerData(currentFrameInput[2]));
                else                
                    outputFile.Add(0xFF);                                 
            }

            if (filename == "") filename = this.Filename;

            FileStream   fs     = File.Open(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);
            
            foreach (byte b in outputFile) writer.Write(b);            
            
            writer.Close(); writer = null; fs.Dispose();            
        }
        
        /// <summary>
        /// Convert string input to a byte value
        /// </summary> 
        private byte parseControllerData(string frameInput)
        {            
            byte input = 0xFF;
            if (frameInput != null)
            {
                if (frameInput.Contains("^")) input -= 0x01;
                if (frameInput.Contains("v")) input -= 0x02;
                if (frameInput.Contains("<")) input -= 0x04;
                if (frameInput.Contains(">")) input -= 0x08;
                if (frameInput.Contains("A")) input -= 0x10;
                if (frameInput.Contains("B")) input -= 0x20;
                if (frameInput.Contains("C")) input -= 0x40;
                if (frameInput.Contains("S")) input -= 0x80;
            }
            return input;
        }

    #endregion

    }
}
