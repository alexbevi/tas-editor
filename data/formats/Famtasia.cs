using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
/***************************************************************************************************
 * Read and parse a Famtasia FMV movie file
 **************************************************************************************************/
namespace MovieSplicer.Data
{
    public class Famtasia
    {
        const byte HEADER_SIZE = 144;
                
        private byte[] fileContents;
        
        public string    Filename;
        public FMVHeader Header;
        public ArrayList ControllerInput;

        static byte      bytesPerFrame; // populated when FMVHeader() is created
        static Functions fn = new Functions();
        static int[] offsets = {
            0x00, // 4-byte signature: 46 4D 56 1A "FMV\x1A"
            0x04, // 1-byte flags:
                  //      bit 7: 0=reset-based, 1=savestate-based
                  //      other bits: unknown, set to 0
            0x05, // 1-byte flags:
                  //      bit 7: uses controller 1
                  //      bit 6: uses controller 2
                  //      bit 5: is a FDS recording
                  //      other bits: unknown, set to 0
            0x06, // 4-byte little-endian unsigned int: unknown, set to 00000000
            0x0A, // 4-byte little-endian unsigned int: rerecord count minus 1
            0x0E, // 2-byte little-endian unsigned int: unknown, set to 0000
            0x10, // 64-byte zero-terminated emulator identifier string
            0x50, // 64-byte zero-terminated movie title string
            0x90  // frame data begins here
        };

        public Famtasia(string FMVFile)
        {
            Filename = FMVFile;

            FileStream   fs = File.OpenRead(FMVFile);
            BinaryReader br = new BinaryReader(fs);

            fileContents = br.ReadBytes((int)fs.Length);

            br.Close(); br = null; fs.Dispose();

            Header = new FMVHeader(ref fileContents);
                        
            populateControllerData(ref fileContents);
        }

        /// <summary>
        /// Encapsulates all FMV Header information
        /// </summary>
        public class FMVHeader
        {
            public string Signature;
            public bool   StartFromReset;
            public bool   StartFromSave;           
            public bool   FDS;
            public int    FrameCount;
            public uint   ReRecordCount;
            public string EmulatorID;
            public string MovieTitle;
            public bool[] Controllers = { false, false };

            public FMVHeader(ref byte[] byteArray)
            {
                Signature      = fn.ReadHEX(fn.readBytes(ref byteArray, offsets[0], 4));
                StartFromReset = (1 & (byteArray[offsets[1]]) >> 7) == 0 ? true : false;
                StartFromSave  = (1 & (byteArray[offsets[1]]) >> 7) == 1 ? true : false;
                ReRecordCount  = fn.Read32(fn.readBytes(ref byteArray, offsets[4], 4)) - 1;
                Controllers[0] = (1 & (byteArray[offsets[2]]) >> 7) == 1 ? true : false;
                Controllers[1] = (1 & (byteArray[offsets[2]]) >> 6) == 1 ? true : false;
                FDS            = (1 & (byteArray[offsets[2]]) >> 5) == 1 ? true : false;
                MovieTitle     = fn.ReadChars(fn.readBytes(ref byteArray, offsets[7], 64));
                EmulatorID     = fn.ReadChars(fn.readBytes(ref byteArray, offsets[6], 64));

                bytesPerFrame = 0;
                if (Controllers[0] == true) bytesPerFrame++;
                if (Controllers[1] == true) bytesPerFrame++;
                if (FDS == true) bytesPerFrame++;
                
                FrameCount = byteArray.Length - HEADER_SIZE / bytesPerFrame;                
            }
        }

        /// <summary>
        /// Convert FMV binary data to a readable string representation
        /// </summary>
        private void populateControllerData(ref byte[] byteArray)
        {
            ControllerInput = new ArrayList();
                        
            for (int i = HEADER_SIZE; i < byteArray.Length; i++)
            {
                string[] frameData = new string[3];
                
                for (int j = 0; j < bytesPerFrame; j++)
                {
                    frameData[j] = parseControllerData(byteArray[i]);
                    if(j > 0) i++;
                }
                ControllerInput.Add(frameData);
            }
        }

        /// <summary>
        /// FMV input convertion (bit position -> string)
        /// </summary> 
        private string parseControllerData(byte frameInput)
        {
            string input = "";
             
            if((1 & (frameInput >> 0)) == 1) input += ">";
            if((1 & (frameInput >> 1)) == 1) input += "<";
            if((1 & (frameInput >> 2)) == 1) input += "^";
            if((1 & (frameInput >> 3)) == 1) input += "v";
            if((1 & (frameInput >> 4)) == 1) input += "B";
            if((1 & (frameInput >> 5)) == 1) input += "A";
            if((1 & (frameInput >> 6)) == 1) input += "s";
            if((1 & (frameInput >> 7)) == 1) input += "S";

            return input;
        }

        /// <summary>
        /// Save input changes back out to file
        /// </summary>        
        public void Save(string filename, ref ArrayList input)
        {
            ArrayList outputFile = new ArrayList();

            fn.bytesToArray(ref outputFile, this.fileContents, 0, offsets[8]);
            string[] currentFrameInput = new string[bytesPerFrame];

            for (int i = 0; i < input.Count; i++)
            {
                currentFrameInput = (string[])input[i];

                for (int j = 0; j < currentFrameInput.Length; j++)
                {
                    if (currentFrameInput[j] != null)                                            
                        outputFile.Add(parseControllerInput(currentFrameInput[j]));                    
                }
            }

            if (filename == "") filename = this.Filename;

            FileStream fs = File.Open(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);

            foreach (byte b in outputFile) writer.Write(b);

            writer.Close(); writer = null; fs.Dispose();     
        }

        /// <summary>
        /// FMV input conversion (string -> bit position)
        /// </summary> 
        private byte parseControllerInput(string frameInput)
        {
            byte input = 0x00;
            
            if (frameInput.Contains(">")) input |= (1 << 0);
            if (frameInput.Contains("<")) input |= (1 << 1);
            if (frameInput.Contains("^")) input |= (1 << 2);
            if (frameInput.Contains("v")) input |= (1 << 3);
            if (frameInput.Contains("B")) input |= (1 << 4);
            if (frameInput.Contains("A")) input |= (1 << 5);
            if (frameInput.Contains("s")) input |= (1 << 6);
            if (frameInput.Contains("S")) input |= (1 << 7);            

            return input;
        }
    }
}
