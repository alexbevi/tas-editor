using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MovieSplicer.UI.Methods
{
    /// <summary>
    /// Handle saving/loading of buffered movie data
    /// </summary>
    class MovieBufferIO
    {
        /// <summary>
        /// Load the contents of an external file to the copy buffer
        /// </summary>        
        public static void Load(string filename, ref string bufferType, ref ArrayList buffer)
        {
            StreamReader reader = File.OpenText(filename);

            bufferType = reader.ReadLine();

            // TODO::This is a weak validation routine
            if (bufferType.Length != 3)
            {
                MessageBox.Show("Buffer file appears to be invalid", "Oops");
                return;
            }

            string lineItem = null;
            
            while ((lineItem = reader.ReadLine()) != null)
            {
                string[] frame = lineItem.Split('|');
                buffer.Add(frame);
            }

            reader.Close(); reader.Dispose();
        }

        /// <summary>
        /// Save the contents of the copy buffer out to file
        /// </summary>        
        public static void Save(string filename, string bufferType, ArrayList buffer, int columns)
        {
            TextWriter writer = File.CreateText(filename);

            writer.WriteLine(bufferType);
            
            foreach (string[] frame in buffer)
            {
                string lineItem = "";

                for (int i = 0; i < columns; i++)
                {
                    lineItem += frame[i] + "|";
                }                                
                lineItem = lineItem.Remove(lineItem.Length - 1); // trim last char
                
                writer.WriteLine(lineItem);
            }

            writer.Close(); writer.Dispose();
        }

    }
}
