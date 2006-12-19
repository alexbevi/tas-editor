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
using MovieSplicer.Data.Structures;
/*****************************************************************************************************
 * Copy a range of frames from the source movie to the target movie
 ****************************************************************************************************/
namespace MovieSplicer.UI
{        
    public partial class frmSplice : Form
    {      
        static Functions fn     = new Functions();
        static string    filter = fn.ALL_FILTER + "|" + fn.SMV_FILTER + "|" + fn.FCM_FILTER + "|" + 
                                  fn.GMV_FILTER + "|" + fn.FMV_FILTER + "|" + fn.VBM_FILTER;
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
                fileName.Text = fn.extractFilenameFromPath(((Gens)movie).Filename);            
                frameCount.Text = String.Format("{0:0,0}", ((Gens)movie).Header.FrameCount);
            }
            else if (movie is SNES9x)
            {
                fileName.Text = fn.extractFilenameFromPath(((SNES9x)movie).Filename);
                frameCount.Text = String.Format("{0:0,0}", ((SNES9x)movie).Header.FrameCount);
            }
            else if (movie is FCEU)
            {
                fileName.Text = fn.extractFilenameFromPath(((FCEU)movie).Filename);            
                frameCount.Text = String.Format("{0:0,0}", ((FCEU)movie).Header.FrameCount);
            }
            else if (movie is VisualBoyAdvance)
            {
                fileName.Text = fn.extractFilenameFromPath(((VisualBoyAdvance)movie).Filename);
                frameCount.Text = String.Format("{0:0,0}", ((VisualBoyAdvance)movie).Header.FrameCount);
            }
            else if (movie is Famtasia)
            {
                fileName.Text = fn.extractFilenameFromPath(((Famtasia)movie).Filename);
                frameCount.Text = String.Format("{0:0,0}", ((Famtasia)movie).Header.FrameCount);
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
            dlg.Filter = filter;
            dlg.ShowDialog();

            // if no file loaded, exit
            if (dlg.FileName.Length == 0) { dlg.Dispose(); return; }

            MovieType movieType = fn.IsValid(dlg.FileName);
            
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
            ArrayList source;
            ArrayList target;            
            MovieType movieType = MovieType.None;

            // make sure loaded movies are of the same type
            if ((sourceMovie as SNES9x) != null && (targetMovie as SNES9x) != null)            
            {
                source    = ((SNES9x)sourceMovie).ControllerData.ControllerInput;
                target    = ((SNES9x)targetMovie).ControllerData.ControllerInput;
                movieType = MovieType.SMV;
            }
            else if ((sourceMovie as Gens) != null && (targetMovie as Gens) != null)
            {
                source    = ((Gens)sourceMovie).ControllerData.ControllerInput;
                target    = ((Gens)targetMovie).ControllerData.ControllerInput;
                movieType = MovieType.GMV;
            }
            else if ((sourceMovie as FCEU) != null && (targetMovie as FCEU) != null)
            {
                source    = ((FCEU)sourceMovie).ControllerData.ControllerInput;
                target    = ((FCEU)targetMovie).ControllerData.ControllerInput;
                movieType = MovieType.FCM;                                       
            }
            else if ((sourceMovie as Famtasia) != null && (targetMovie as Famtasia) != null)
            {
                source = ((Famtasia)sourceMovie).ControllerInput;
                target = ((Famtasia)targetMovie).ControllerInput;
                movieType = MovieType.FMV;
            }
            else if ((sourceMovie as VisualBoyAdvance) != null && (targetMovie as VisualBoyAdvance) != null)
            {
                source = ((VisualBoyAdvance)sourceMovie).ControllerData;
                target = ((VisualBoyAdvance)targetMovie).ControllerData;
                movieType = MovieType.VBM;
            }
            else
            {
                MessageBox.Show("Movie Types Don't Match", "Error");
                return;
            }

            // if movies loaded successfully, create the container for the spliced input
            ArrayList spliced = new ArrayList();

            // convert input field values to numbers
            int sourceFrom   = (fn.IsNumeric(txtCopyFromFrame.Text)) ? Convert.ToInt32(txtCopyFromFrame.Text) : 0;
            int sourceTo     = (fn.IsNumeric(txtCopyToFrame.Text)) ? Convert.ToInt32(txtCopyToFrame.Text) : 0;
            int targetInsert = (fn.IsNumeric(txtTargetFrameInsertPosition.Text)) ? Convert.ToInt32(txtTargetFrameInsertPosition.Text) : 0;
            int targetResume = (fn.IsNumeric(txtTargetFrameResumePostion.Text)) ? Convert.ToInt32(txtTargetFrameResumePostion.Text) : 0;

            // ensure frame positions aren't out of bounds
            if (sourceFrom < 0 || sourceFrom > source.Count || sourceTo < 0 || sourceTo > source.Count)
            {
                MessageBox.Show("Source frame position out of bounds", "Error");
                return;
            }
            else if (targetInsert < 0 || targetInsert > target.Count || targetResume < 0 || targetResume > target.Count)
            {
                MessageBox.Show("Target frame position out of bounds", "Error");
                return;
            }            

            // write the source movie (start -> position) and the target movie (position -> end) into a new array
            if (chkStartOfMovie.Checked)
            {
                // make sure the values needed for the loops exist
                if (sourceTo == 0 || targetResume == 0) return;

                for (int i = 0; i < sourceTo; i++)
                    spliced.Add(source[i]);
                for (int j = targetResume; j < target.Count; j++)
                    spliced.Add(target[j]);

            }
            // write the target movie (start -> position) and the source movie (position -> end) into a new array
            else if (chkEndOfMovie.Checked)
            {
                // make sure the values needed for the loops exist
                if (targetInsert == 0 || sourceFrom == 0) return;

                for (int i = 0; i < targetInsert; i++)
                    spliced.Add(target[i]);
                for (int j = sourceFrom; j < source.Count; j++)
                    spliced.Add(source[j]);
            }
            // write the target movie (start -> insertPos), add the source range (sourceTo -> sourceFrom), then
            // add the target movie again (resumePos -> end)
            else
            {
                // make sure the values needed for the loops exist
                if (sourceTo == 0 || targetResume == 0 || targetInsert == 0 || sourceFrom == 0) return;

                for (int i = 0; i < targetInsert; i++)
                    spliced.Add(target[i]);
                for (int j = sourceFrom; j < sourceTo - 1; j ++)
                    spliced.Add(source[j]);
                for (int k = targetResume; k < target.Count; k++)
                    spliced.Add(target[k]);
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