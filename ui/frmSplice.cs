using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MovieSplicer.Data;
using MovieSplicer.Data.Formats;
using MovieSplicer.Components;
/*****************************************************************************************************
 * Copy a range of frames from the source movie to the target movie
 ****************************************************************************************************/
namespace MovieSplicer.UI
{        
    public partial class frmSplice : TASForm
    {              
        OpenFileDialog   dlg;
                
        static object sourceMovie = null;
        static object targetMovie = null;        
        
        public frmSplice()
        {
            InitializeComponent();                                   
        }

        /// <summary>
        /// Load the a movie from an OpenFileDialog, validate and assign the loaded movie's
        /// type to the static appropriate object (source/target) and populate the necessary
        /// values into the passed textboxes
        /// </summary>        
        private void loadMovieObject(ref object movie, TextBox fileName, TextBox frameCount)
        {           
            if (movie is Gens)
            {
                fileName.Text = FilenameFromPath(((Gens)movie).Filename);            
                frameCount.Text = String.Format("{0:0,0}", ((Gens)movie).GMVHeader.FrameCount);
            }
            else if (movie is SNES9x)
            {
                fileName.Text = FilenameFromPath(((SNES9x)movie).Filename);
                frameCount.Text = String.Format("{0:0,0}", ((SNES9x)movie).SMVHeader.FrameCount);
            }
            else if (movie is FCEU)
            {
                fileName.Text = FilenameFromPath(((FCEU)movie).Filename);            
                frameCount.Text = String.Format("{0:0,0}", ((FCEU)movie).FCMHeader.FrameCount);
            }
            else if (movie is VisualBoyAdvance)
            {
                fileName.Text = FilenameFromPath(((VisualBoyAdvance)movie).Filename);
                frameCount.Text = String.Format("{0:0,0}", ((VisualBoyAdvance)movie).VBMHeader.FrameCount);
            }
            else if (movie is Famtasia)
            {
                fileName.Text = FilenameFromPath(((Famtasia)movie).Filename);
                frameCount.Text = String.Format("{0:0,0}", ((Famtasia)movie).FMVHeader.FrameCount);
            }
        }   
     
        /// <summary>
        /// Load a source movie file
        /// </summary>        
        private void btnLoadSource_Click(object sender, EventArgs e)
        {
            loadFile(true);
            loadMovieObject(ref sourceMovie, txtSourceFileName, txtSourceFrameCount);            
        }   
    
        /// <summary>
        /// Load a target movie file
        /// </summary>        
        private void btnLoadTarget_Click(object sender, EventArgs e)
        {
            loadFile(false);
            loadMovieObject(ref targetMovie, txtTargetFileName, txtTargetFrameCount);            
        }
      
        /// <summary>
        /// Validate what kind of file has been loaded and what position it is to occupy (source/target)
        /// </summary>        
        private void loadFile(bool sourceFile)
        {
            dlg = new OpenFileDialog();
            dlg.Filter = TAS_FILTER;

            dlg.ShowDialog();

            // if no file loaded, exit
            if (dlg.FileName.Length == 0) { dlg.Dispose(); return; }

            MovieType movieType = IsValid(dlg.FileName);
            
            switch(sourceFile)
            {
                case true:
                    if      (movieType == MovieType.FCM)  sourceMovie = new FCEU(dlg.FileName);
                    else if (movieType == MovieType.SMV)  sourceMovie = new SNES9x(dlg.FileName);
                    else if (movieType == MovieType.GMV)  sourceMovie = new Gens(dlg.FileName);
                    else if (movieType == MovieType.FMV)  sourceMovie = new Famtasia(dlg.FileName);
                    else if (movieType == MovieType.VBM)  sourceMovie = new VisualBoyAdvance(dlg.FileName);
                    else if (movieType == MovieType.None) sourceMovie = null;
                    break;

                case false:
                    if      (movieType == MovieType.FCM)  targetMovie = new FCEU(dlg.FileName);
                    else if (movieType == MovieType.SMV)  targetMovie = new SNES9x(dlg.FileName);
                    else if (movieType == MovieType.GMV)  targetMovie = new Gens(dlg.FileName);
                    else if (movieType == MovieType.FMV)  targetMovie = new Famtasia(dlg.FileName);
                    else if (movieType == MovieType.VBM)  targetMovie = new VisualBoyAdvance(dlg.FileName);
                    else if (movieType == MovieType.None) targetMovie = null;
                    break;
            }
            
            dlg.Dispose();
        }

        private void chkEndOfMovie_CheckedChanged(object sender, EventArgs e)
        {
            // input enabled if not checked
            txtCopyToFrame.Enabled              = !chkEndOfMovie.Checked;
            chkStartOfMovie.Enabled             = !chkEndOfMovie.Checked;
            chkStartOfMovie.Checked             = false;
            txtTargetFrameResumePostion.Enabled = !chkEndOfMovie.Checked;
        }

        private void chkStartOfMovie_CheckedChanged(object sender, EventArgs e)
        {
            // input enabled if not checked
            txtCopyFromFrame.Enabled             = !chkStartOfMovie.Checked;
            txtTargetFrameInsertPosition.Enabled = !chkStartOfMovie.Checked;
            chkEndOfMovie.Checked                = false;
            chkEndOfMovie.Enabled                = !chkStartOfMovie.Checked;
        }   
        
        /// <summary>
        /// Splice two movies' input arrays into a new array, then pass this to 
        /// the appropriate sourceMovie's Save method
        /// </summary>        
        private void btnSplice_Click(object sender, EventArgs e)
        {
            TASMovieInput[] source;
            TASMovieInput[] target;                        
            MovieType movieType = MovieType.None;

            // make sure loaded movies are of the same type
            if ((sourceMovie as SNES9x) != null && (targetMovie as SNES9x) != null)
            {
                source = ((SNES9x)sourceMovie).SMVInput.FrameData;
                target = ((SNES9x)targetMovie).SMVInput.FrameData;
                movieType = MovieType.SMV;
            }
            else if ((sourceMovie as Gens) != null && (targetMovie as Gens) != null)
            {
                source = ((Gens)sourceMovie).GMVInput.FrameData;
                target = ((Gens)targetMovie).GMVInput.FrameData;
                movieType = MovieType.GMV;
            }
            else if ((sourceMovie as FCEU) != null && (targetMovie as FCEU) != null)
            {
                source = ((FCEU)sourceMovie).FCMInput.FrameData;
                target = ((FCEU)targetMovie).FCMInput.FrameData;
                movieType = MovieType.FCM;
            }
            else if ((sourceMovie as Famtasia) != null && (targetMovie as Famtasia) != null)
            {
                source = ((Famtasia)sourceMovie).FMVInput.FrameData;
                target = ((Famtasia)targetMovie).FMVInput.FrameData;
                movieType = MovieType.FMV;
            }
            else if ((sourceMovie as VisualBoyAdvance) != null && (targetMovie as VisualBoyAdvance) != null)
            {
                source = ((VisualBoyAdvance)sourceMovie).VBMInput.FrameData;
                target = ((VisualBoyAdvance)targetMovie).VBMInput.FrameData;
                movieType = MovieType.VBM;
            }
            else
            {
                MessageBox.Show("Movie Types Don't Match", "Error");
                return;
            }

            // if movies loaded successfully, create the container for the spliced input
            TASMovieInput[] spliced;

            // convert input field values to numbers
            int sourceFrom   = (IsNumeric(txtCopyFromFrame.Text)) ? Convert.ToInt32(txtCopyFromFrame.Text) : 0;
            int sourceTo     = (IsNumeric(txtCopyToFrame.Text)) ? Convert.ToInt32(txtCopyToFrame.Text) : 0;
            int targetInsert = (IsNumeric(txtTargetFrameInsertPosition.Text)) ? Convert.ToInt32(txtTargetFrameInsertPosition.Text) : 0;
            int targetResume = (IsNumeric(txtTargetFrameResumePostion.Text)) ? Convert.ToInt32(txtTargetFrameResumePostion.Text) : 0;

            // ensure frame positions aren't out of bounds
            if (sourceFrom < 0 || sourceFrom > source.Length || sourceTo < 0 || sourceTo > source.Length)
            {
                MessageBox.Show("Source frame position out of bounds", "Error");
                return;
            }
            else if (targetInsert < 0 || targetInsert > target.Length || targetResume < 0 || targetResume > target.Length)
            {
                MessageBox.Show("Target frame position out of bounds", "Error");
                return;
            }            

            // write the source movie (start -> position) and the target movie (position -> end) into a new array
            if (chkStartOfMovie.Checked)
            {
                // make sure the values needed for the loops exist
                if (sourceTo == 0 || targetResume == 0) return;
                spliced = TASMovieInput.Splice(ref source, ref target, 0, sourceTo, targetResume, target.Length);                
            }
            // write the target movie (start -> position) and the source movie (position -> end) into a new array
            else if (chkEndOfMovie.Checked)
            {
                // make sure the values needed for the loops exist
                if (targetInsert == 0 || sourceFrom == 0) return;
                spliced = TASMovieInput.Splice(ref target, ref source, 0, targetInsert, sourceFrom, source.Length);                
            }
            // write the target movie (start -> insertPos), add the source range (sourceTo -> sourceFrom), then
            // add the target movie again (resumePos -> end)
            else
            {
                // make sure the values needed for the loops exist
                if (sourceTo == 0 || targetResume == 0 || targetInsert == 0 || sourceFrom == 0) return;
                TASMovieInput[] temp = TASMovieInput.Splice(ref target, ref source, 0 , targetInsert, sourceFrom, sourceTo);
                spliced = TASMovieInput.Splice(ref temp, ref target, 0, temp.Length, targetResume, target.Length);                
            }
            
            // prepend "spliced-" to the current filename
            string filename = "spliced-" + txtTargetFileName.Text;
            
            // depending on what type of movie file we're dealing with, cast
            // the target and call its Save method
            switch (movieType)
            {
                case MovieType.SMV:
                    ((SNES9x)targetMovie).Save(filename, ref spliced);
                    break;
                case MovieType.GMV:
                    ((Gens)targetMovie).Save(filename, ref spliced);
                    break;
                case MovieType.FCM:
                    ((FCEU)targetMovie).Save(filename, ref spliced);
                    break;
                case MovieType.FMV:
                    ((Famtasia)targetMovie).Save(filename, ref spliced);
                    break;
                case MovieType.VBM:
                    ((VisualBoyAdvance)targetMovie).Save(filename, ref spliced);
                    break;
            }
        
            MessageBox.Show("Successfully wrote " + filename, "Jolly Good");
            this.Close();                                 
        }
    }
}