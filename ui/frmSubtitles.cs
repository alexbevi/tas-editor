using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Data;
using MovieSplicer.Components;
using MovieSplicer.UI.Methods;

namespace MovieSplicer.UI
{
    public partial class frmSubtitles : TASForm
    {
        const string SRT_FILTER = "SRT - SubRip Subtitle|*.srt";

        TASMovieInputCollection Input;

        public frmSubtitles(ref TASMovieInputCollection input)
        {
            InitializeComponent();
            Input = input;          
        }

        /// <summary>
        /// Close the subtitles form
        /// </summary>        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Browse for an output location
        /// </summary>        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            saveDlg = new SaveFileDialog();
            saveDlg.Filter = SRT_FILTER;
            saveDlg.ShowDialog();

            txtFilename.Text = saveDlg.FileName;            
        }

        /// <summary>
        /// Export the current movie out as a subtitle file
        /// </summary>        
        private void btnExport_Click(object sender, EventArgs e)
        {
            // validate objects and values first
            if (Input.Input == null && txtFilename.Text.Length == 0) return;
            if (!IsNumeric(txtAVITiming.Text)) return;

            // create the subgenerator object
            SubtitleGenerator subGen = new SubtitleGenerator(ref Input, txtFilename.Text);
            subGen.Offset = Convert.ToInt32(txtOffset.Text);
            subGen.FPS    = Convert.ToDouble(txtAVITiming.Text);
            subGen.Export();
            subGen = null;

            MessageBox.Show("Subtitle file exported", "Congrats");
        }       
    }
}