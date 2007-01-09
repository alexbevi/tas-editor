using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSplicer.Data
{
    public class UndoBuffer
    {
        public TASMovieInput[] Changes;
        
        /// <summary>
        /// Add a frame data collection to the undo buffer
        /// </summary>        
        public static void Add(ref UndoBuffer[] buffer, ref TASMovieInput[] change)
        {
            UndoBuffer[] temp = new UndoBuffer[buffer.Length + 1];            
                  
            if (buffer.Length > 0) buffer.CopyTo(temp, 0);

            temp[temp.Length - 1] = new UndoBuffer();
            temp[temp.Length - 1].Changes = new TASMovieInput[change.Length];
            change.CopyTo(temp[temp.Length - 1].Changes, 0);       
            
            buffer = temp;
        }

        /// <summary>
        /// Return the last buffer value and remove it from the collections
        /// </summary>        
        public static void Undo(ref UndoBuffer[] buffer)
        {            
            UndoBuffer[] temp = new UndoBuffer[buffer.Length - 1];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = buffer[i];

            buffer = temp;            
        }
    }
}
