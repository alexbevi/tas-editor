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

using MovieSplicer.Data;
using MovieSplicer.Components;

namespace MovieSplicer.UI
{
    public partial class frmSaveAs : TASForm
    {
        private TASMovie                Movie;
        private TASMovieInputCollection MovieData;
        

        public frmSaveAs(ref TASMovie movie, ref TASMovieInputCollection movieData)
        {
            InitializeComponent();

            Movie     = movie;
            MovieData = movieData;
            

            txtFilename.Text    = "new-" + FilenameFromPath(Movie.Filename);
            txtAuthor.Text      = Movie.Extra.Author;
            txtDescription.Text = Movie.Extra.Description;

            txtAuthor.Focus();
        }
        
    }
}