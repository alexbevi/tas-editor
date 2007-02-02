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
    public partial class frmSubtitles : TASForm
    {
        public frmSubtitles()
        {
            InitializeComponent();

            //Methods.SubtitleGenerator SRT = new MovieSplicer.UI.Methods.SubtitleGenerator(ref FrameData, @"F:\TEST.SRT");
            //SRT = null;
        }

        /// <summary>
        /// Close the subtitles form
        /// </summary>        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }       
    }
}