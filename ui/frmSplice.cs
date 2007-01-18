/******************************************************************************** 
 * TAS Movie Editor                                                             *
 *                                                                              *
 * Copyright notice for this file:                                              *
 *  Copyright (C) 2006-7 Maximus                                                *
 *                                                                              *
 * This program is free software; you can redistribute it and/or modify         *
 * it under the terms of the GNU General Public License as published by         *
 * the Free Software Foundation; either version 2 of the License, or            *
 * (at your option) any later version.                                          *
 *                                                                              *
 * This program is distributed in the hope that it will be useful,              *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
 * GNU General Public License for more details.                                 *
 *                                                                              *
 * You should have received a copy of the GNU General Public License            *
 * along with this program; if not, write to the Free Software                  *
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA    *
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Components;
using MovieSplicer.Data;
using MovieSplicer.Data.Formats;

namespace MovieSplicer.UI
{
    public partial class frmSplice : TASForm
    {
        public struct DroppedMovie
        {
            public TASMovie  Movie;
            public MovieType MovieType;
        }

        DroppedMovie[] Movies;

        /// <summary>
        /// Initialize
        /// </summary>
        public frmSplice()
        {
            InitializeComponent();
            
            // initialize the array (save the grief of hitting a null reference error)
            Movies = new DroppedMovie[0];
        }
        
        /// <summary>
        /// Populate the start/end values in the edit fields from the selected row
        /// </summary>        
        private void lvSplice_Click(object sender, EventArgs e)
        {
            // make sure an item is selected
            if (lvSplice.SelectedIndices.Count == 0) return;

            int position  = lvSplice.SelectedIndices[0];
            txtStart.Text = lvSplice.Items[position].SubItems[3].Text;
            txtEnd.Text   = lvSplice.Items[position].SubItems[4].Text;
        }

        /// <summary>
        /// Check for a valid TAS file and add it to the DroppedMovie array
        /// </summary>        
        private DroppedMovie[] populateMovieList(string filename)
        {
            MovieType fileType = IsValid(filename);
            if (fileType != MovieType.None)
            {
                DroppedMovie[] temp = new DroppedMovie[Movies.Length + 1];                
                Movies.CopyTo(temp, 0);

                temp[Movies.Length].MovieType = fileType;
                temp[Movies.Length].Movie = LoadMovie(filename, fileType);                
                return temp;
            }
            return null;
        }

       
        // Change the drag cursor type
        private void lvSplice_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Validate dropped files and add them to the list
        /// </summary>        
        private void lvSplice_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
                Movies = populateMovieList(file);

            // clear the control
            // DEBUG::Not efficient, find a way to just update new movies into the list
            // intead of redrawing on each drop
            lvSplice.Items.Clear();

            foreach (DroppedMovie movie in Movies)
            {
                ListViewItem lvi = new ListViewItem(movie.MovieType.ToString());
                lvi.SubItems.Add(FilenameFromPath(movie.Movie.Filename));
                lvi.SubItems.Add(String.Format("{0:0,0}", movie.Movie.Header.FrameCount));
                lvi.SubItems.Add(""); // start position placeholder
                lvi.SubItems.Add(""); // end position placeholder
                lvSplice.Items.Add(lvi);
            }            
        }

        /// <summary>
        /// Add the frame range to the selected row
        /// </summary>        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // make sure an item is selected
            if (lvSplice.SelectedIndices.Count == 0) return;
            
            // range has to be numeric to proceed
            if(!IsNumeric(txtStart.Text) || !IsNumeric(txtEnd.Text)) return;
                        
            int position = lvSplice.SelectedIndices[0];
            int frames = Movies[position].Movie.Header.FrameCount;
            int start = Convert.ToInt32(txtStart.Text);
            int end = Convert.ToInt32(txtEnd.Text);

            // check for valid range
            if (start < 0 || start > frames) return;
            if (end < start || end > frames) return;

            lvSplice.Items[position].SubItems[3].Text = start.ToString();
            lvSplice.Items[position].SubItems[4].Text = end.ToString();
        }

        /// <summary>
        /// Remove an entry from the list
        /// </summary>        
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // make sure an item is selected
            if (lvSplice.SelectedIndices.Count == 0) return;
            
            // remove the item entry from the list
            int position = lvSplice.SelectedIndices[0];
            lvSplice.Items.RemoveAt(position);

            DroppedMovie[] temp = new DroppedMovie[Movies.Length - 1];

            // remove the item from the array
            for (int i = 0; i < position; i++)
                temp[i] = Movies[i];
            for (int j = position; j < temp.Length; j++)
                temp[j] = Movies[j + 1];

            Movies = temp;                
        }

        private void btnSplice_Click(object sender, EventArgs e)
        {
            // exit if not enough movies are opened
            if (Movies.Length < 2)
            {
                MessageBox.Show("At least two (2) movies must be loaded for splicing to be performed", "Movie Count Too Low");
                return;
            }

            // ensure all loaded movies are of the same type
            for (int i = 0; i < Movies.Length - 1; i++)
            {
                if (Movies[i].MovieType != Movies[i + 1].MovieType)
                {
                    MessageBox.Show("Movies aren't all of the same type", "Type Mismatch");
                    return;
                }
            }

            TASMovieInput[] spliced = new TASMovieInput[0];

            for(int i = 0; i < Movies.Length; i++)
            {
                int start = (lvSplice.Items[i].SubItems[3].Text.Length > 0) ? Convert.ToInt32(lvSplice.Items[i].SubItems[3].Text) : 0;
                int end   = (lvSplice.Items[i].SubItems[4].Text.Length > 0) ? Convert.ToInt32(lvSplice.Items[i].SubItems[4].Text) : 0;

                // perform a range check on the current movie
                if (start == 0 || end == 0)
                {
                    MessageBox.Show("Splice could not be completed.\nMovie at position " + (i + 1) + " has an invalid range", "Splice Error");
                    spliced = null;  return;
                }

                spliced = TASMovieInput.Splice(ref spliced, ref Movies[i].Movie.Input.FrameData, 0, spliced.Length, start, end);
            }
            
            // DEBUG::output's to the application directory
            string filename = "spliced-" + FilenameFromPath(Movies[0].Movie.Filename);
            Movies[0].Movie.Save(filename, ref spliced);

            MessageBox.Show("Successfully wrote " + filename, "YAY!!!");           
        }

        


    }
}