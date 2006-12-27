using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MovieSplicer.Data
{
    public class TASMovieInput
    {
        public string[] Controller = new string[5];
        
        /// <summary>
        /// Insert a given number of blank frames (new TASMovieInput()) at the
        /// desired position
        /// </summary>        
        public static void Insert(ref TASMovieInput[] input, int position, int length)
        {
            TASMovieInput[] temp = new TASMovieInput[input.Length + length];

            for (int i = 0; i < position; i++)
                temp[i] = input[i];
            for (int j = 0; j < length; j++)
                temp[position + j] = new TASMovieInput();
            for (int k = position; k < input.Length; k++)
                temp[k + length] = input[k];

            input = temp;
        }

        /// <summary>
        /// Insert a given number of assigned frames (TASMovieInput()) at the
        /// desired position
        /// </summary>        
        public static void Insert(ref TASMovieInput[] input, TASMovieInput frame, int position, int length)
        {
            TASMovieInput[] temp = new TASMovieInput[input.Length + length];

            for (int i = 0; i < position; i++)
                temp[i] = input[i];
            for (int j = 0; j < length; j++)
                temp[position + j] = frame;
            for (int k = position; k < input.Length; k++)
                temp[k + length] = input[k];

            input = temp;
        }

        /// <summary>
        /// Insert the passed in TASMovieInput at the desired position
        /// updateFlag is a collection of bool values to indicate which controller(s) to update
        /// </summary>        
        public static void Insert(ref TASMovieInput[] input, TASMovieInput frame, bool[] updateFlag, int position, int length)
        {                                    
            for (int i = position; i < position + length; i++)
                for (int j = 0; j < updateFlag.Length; j++)
                    if (updateFlag[j]) input[i].Controller[j] = frame.Controller[j];                                    
        }

        /// <summary>
        /// Remove a given number of frames at the desired position
        /// </summary>        
        public static void Remove(ref TASMovieInput[] input, int position, int length)
        {
            TASMovieInput[] temp = new TASMovieInput[input.Length - length];

            for (int i = 0; i < position; i++)
                temp[i] = input[i];
            for (int j = position; j < temp.Length; j++)
                temp[j] = input[j + length];

            input = temp;
        }

        /// <summary>
        /// Copy a given number of frames at the desired position by extracting them to a new
        /// array
        /// </summary>        
        public static TASMovieInput[] Copy(ref TASMovieInput[] input, int position, int length)
        {
            TASMovieInput[] temp = new TASMovieInput[length];

            for (int i = 0; i < length; i++)
                temp[i] = input[position + i];

            return temp;
        }

        /// <summary>
        /// Insert a given number of blank frames (new TASMovieInput()) at the
        /// desired position
        /// </summary>        
        public static void Paste(ref TASMovieInput[] input, ref TASMovieInput[] buffer, int position)
        {
            TASMovieInput[] temp = new TASMovieInput[input.Length + buffer.Length];

            for (int i = 0; i < position; i++)
                temp[i] = input[i];
            for (int j = 0; j < buffer.Length; j++)
                temp[position + j] = buffer[j];
            for (int k = position; k < input.Length; k++)
                temp[k + buffer.Length] = input[k];

            input = temp;
        }

        public static TASMovieInput[] Splice(ref TASMovieInput[] source, ref TASMovieInput[] target, int sourceStart, int sourceEnd, int targetStart, int targetEnd)
        {            
            TASMovieInput[] spliced = new TASMovieInput[(sourceEnd - sourceStart) + (targetEnd - targetStart)];
            for (int i = sourceStart; i < sourceEnd; i++)
                spliced[i] = source[i];
            
            int position = 0;
            for (int j = targetStart; j < targetEnd; j++)
                spliced[sourceEnd + position++] = target[j];

            return spliced;
        }
    }
}
