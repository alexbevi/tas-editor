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
        private TASMovie        Movie;
        private MovieType       MovieFormat;
        private TASMovieInput[] FrameData;
        

        public frmSaveAs(ref TASMovie movie, ref TASMovieInput[] input, MovieType type)
        {
            InitializeComponent();

            Movie       = movie;
            MovieFormat = type;
            FrameData   = input;

            txtFilename.Text    = "new-" + FilenameFromPath(Movie.Filename);
            txtAuthor.Text      = Movie.Extra.Author;
            txtDescription.Text = Movie.Extra.Description;

            txtAuthor.Focus();
        }
        
    }
}