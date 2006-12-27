using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

using MovieSplicer.Data;

namespace MovieSplicer.UI.Methods
{
    /// <summary>
    /// Handle saving/loading of buffered movie data
    /// </summary>
    class MovieBufferIO
    {
        /// <summary>
        /// Load the contents of an external file to the copy buffer
        /// Returns the column count
        /// </summary>        
        public int Load(string filename, ref string bufferType, ref TASMovieInput[] buffer)
        {
            StreamReader reader = File.OpenText(filename);
            bufferType = reader.ReadLine();            

            int columns = 0;

            // TODO::This is a weak validation routine
            if (bufferType.Length != 3)
            {
                MessageBox.Show("Buffer file appears to be invalid", "Oops");
                return 0;
            }

            string lineItem = null;            
            while ((lineItem = reader.ReadLine()) != null)
            {
                TASMovieInput frame = new TASMovieInput();
                string[]      split = lineItem.Split('|');
                for (int i = 0; i < split.Length; i++)                
                    frame.Controller[i] = split[i];
                                    
                TASMovieInput.Insert(ref buffer, frame, buffer.Length, 1);
                columns = split.Length;
            }

            reader.Close(); reader.Dispose();

            return columns;
        }

        /// <summary>
        /// Save the contents of the copy buffer out to file
        /// </summary>        
        public static void Save(string filename, string bufferType, ref TASMovieInput[] buffer, int columns)
        {
            TextWriter writer = File.CreateText(filename);

            writer.WriteLine(bufferType);

            for (int i = 0; i < buffer.Length; i++)
            {
                string lineItem = "";
                for (int j = 0; j < columns; j++)
                {
                    lineItem += buffer[i].Controller[j] + "|";
                }
                lineItem = lineItem.Remove(lineItem.Length - 1); // trim last char
                writer.WriteLine(lineItem);
            }            
            writer.Close(); writer.Dispose();
        }

    }
}
