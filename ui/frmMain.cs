using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Data;
using MovieSplicer.Data.Structures;
/***************************************************************************************************
 * TAS Movie Editor
 * Author: Alex Bevilacqua (aka Maximus)
 * Revision: 0-8
 **************************************************************************************************/
namespace MovieSplicer.UI
{
    public partial class frmMain : Form
    {
        Functions      fn = new Functions();
        OpenFileDialog dlgOpen;             
        
        MovieType movieType       = MovieType.None;
        MovieType bufferMovieType = MovieType.None;        
        
        static ArrayList arrInput;  // this array will hold the input from the loaded movie file                                
        static ArrayList arrBuffer; // this array will hold the buffered frame data for copy-pasting

        public frmMain()
        {
            InitializeComponent();
        }

    #region "Instantiate Movie Objects"

        static SNES9x SMV;

        /// <summary>
        /// Instantiates the SNES9x object with a validated SMV file read in from
        /// an OpenDialog call
        /// </summary>
        private void loadSMVFile(string filename)
        {            
            SMV      = new SNES9x(filename);
            arrInput = SMV.ControllerData.ControllerInput;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.SMV(ref tvInfo, ref SMV);
            
            for (int i = 0; i < 5; i++)
            {
                if (SMV.ControllerData.Controller[i] == true)                
                    lvInput.Columns.Add("Controller " + (i + 1), 75);                                    
            }

            txtFrameDataC1.Enabled = SMV.ControllerData.Controller[0];
            txtFrameDataC2.Enabled = SMV.ControllerData.Controller[1];
            txtFrameDataC3.Enabled = SMV.ControllerData.Controller[2];
            txtFrameDataC4.Enabled = SMV.ControllerData.Controller[3];            
        }

        static FCEU FCM;

        /// <summary>
        /// Instantiates the FCEU object with a validated FCM file read in from
        /// an OpenDialog call
        /// </summary>
        private void loadFCMFile(string filename)
        {                        
            FCM      = new FCEU(filename);
            arrInput = FCM.ControllerData.ControllerInput;            
            
            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.FCM(ref tvInfo, ref FCM);

            // set the controller columns and enable the editing fields
            for (int i = 0; i < 4; i++)
            {
                if (FCM.ControllerData.Controller[i] == true)                
                    lvInput.Columns.Add("Controller " + (i + 1), 75);                                    
            }

            txtFrameDataC1.Enabled = FCM.ControllerData.Controller[0];
            txtFrameDataC2.Enabled = FCM.ControllerData.Controller[1];
            txtFrameDataC3.Enabled = FCM.ControllerData.Controller[2];
            txtFrameDataC4.Enabled = FCM.ControllerData.Controller[3];           
        }    

        static Gens GMV;

        /// <summary>
        /// Instantiates the Gens object with a validated GMV file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadGMVFile(string filename)
        {                        
            GMV      = new Gens(filename);
            arrInput = GMV.ControllerData.ControllerInput;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.GMV(ref tvInfo, ref GMV);

            txtFrameDataC1.Enabled = true;
            txtFrameDataC2.Enabled = true;

            if (GMV.Header.Version > 0x09)
            {
                for (int i = 1; i <= GMV.Options.ControllerCount; i++)
                    lvInput.Columns.Add("Controller " + i, 75);
                txtFrameDataC3.Enabled = true;
            }
            else
            {
                lvInput.Columns.Add("Controller 1", 75);
                lvInput.Columns.Add("Controller 2", 75);
            }
        }                  

        static Famtasia FMV;

        /// <summary>
        /// Instantiates the Gens object with a validated GMV file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadFMVFile(string filename)
        {            
            FMV = new Famtasia(filename);
            arrInput = FMV.ControllerInput;            
            
            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.FMV(ref tvInfo, ref FMV);  
            
            if (FMV.Header.Controllers[0] == true)
            {
                lvInput.Columns.Add("Controller 1", 75);                
                txtFrameDataC1.Enabled = true;
            }
            if (FMV.Header.Controllers[1] == true)
            {
                lvInput.Columns.Add("Controller 2", 75);               
                txtFrameDataC2.Enabled = true;
            }
        }
   
        static VisualBoyAdvance VBM;

        /// <summary>
        /// Instantiates the Gens object with a validated VBM file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadVBMFile(string filename)
        {            
            VBM = new VisualBoyAdvance(filename);
            arrInput = VBM.ControllerData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.VBM(ref tvInfo, ref VBM);

            // set the controller columns and enable the editing fields
            for (int i = 0; i < 4; i++)
            {
                if (VBM.Options.Controllers[i])
                    lvInput.Columns.Add("Controller " + (i + 1), 75);
            }

            txtFrameDataC1.Enabled = VBM.Options.Controllers[0];
            txtFrameDataC2.Enabled = VBM.Options.Controllers[1];
            txtFrameDataC3.Enabled = VBM.Options.Controllers[2];
            txtFrameDataC4.Enabled = VBM.Options.Controllers[3];
        }

        static Mupen64 M64;

        /// <summary>
        /// Instantiates the Mupen64 object with a validated M64 file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadM64File(string filename)
        {
            M64 = new Mupen64(filename);
            //arrInput = VBM.ControllerData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.M64(ref tvInfo, ref M64);

            // set the controller columns and enable the editing fields
            //for (int i = 0; i < 4; i++)
            //{
            //    if (VBM.Options.Controllers[i])
            //        lvInput.Columns.Add("Controller " + (i + 1), 75);
            //}

            //txtFrameDataC1.Enabled = VBM.Options.Controllers[0];
            //txtFrameDataC2.Enabled = VBM.Options.Controllers[1];
            //txtFrameDataC3.Enabled = VBM.Options.Controllers[2];
            //txtFrameDataC4.Enabled = VBM.Options.Controllers[3];
        }

    #endregion

    #region "Frame Data Listview Handlers"

        /// <summary>
        /// Populate edit controls with frame data        
        /// </summary>
        private void lvInput_Clicked(object sender, EventArgs e)
        {            
            if (lvInput.SelectedIndices.Count > 0 && movieType != MovieType.None)
            {
                ListViewItem lvi = lvInput.Items[lvInput.SelectedIndices[0]];

                txtWorkingFrame.Text = lvi.Text;
                txtFrameDataC1.Text = lvi.SubItems[1].Text;
                if (lvi.SubItems.Count > 2) txtFrameDataC2.Text = lvi.SubItems[2].Text;
                if (lvi.SubItems.Count > 3) txtFrameDataC3.Text = lvi.SubItems[3].Text;
                if (lvi.SubItems.Count > 4) txtFrameDataC4.Text = lvi.SubItems[4].Text;
            }
        }
        
        /// <summary>
        /// If the ListView loses focus, reset the edit fields
        /// </summary>        
        private void lvInput_Leave(object sender, EventArgs e)   
        {
            // if the focus lands on one of the edit boxes after the listview,
            // don't clear the fields;
            if (txtFrameDataC1.Focused || txtFrameDataC2.Focused ||
                txtFrameDataC3.Focused || txtFrameDataC4.Focused)
                return;

            // clear the edit fields when the control loses focus
            txtFrameDataC1.Text  = "";
            txtFrameDataC2.Text  = "";
            txtFrameDataC3.Text  = "";
            txtFrameDataC4.Text  = "";
            txtWorkingFrame.Text = "";
        }               

        /// <summary>
        /// Jump to a selected frame
        /// </summary>        
        private void btnGo_Click(object sender, EventArgs e)
        {                        
            // if not numeric or no movie loaded
            if (fn.IsNumeric(txtJumpToFrame.Text) == false || arrInput == null) return;

            // subtract 1 since we're looking for an index
            int targetFrame = Convert.ToInt32(txtJumpToFrame.Text) - 1;                

            // check for valid range
            if(targetFrame <= arrInput.Count && targetFrame > 0)
            {
                lvInput.Items[targetFrame].Selected = true;
                lvInput.Focus();
                lvInput.EnsureVisible(targetFrame);
            }
        }
    
    #endregion

    #region "Menu Actions"

        /// <summary>
        /// Show an OpenFileDialog to allow a file to be selected and loaded
        /// </summary>        
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            dlgOpen        = new OpenFileDialog();
            dlgOpen.Filter = fn.ALL_FILTER + "|" + fn.SMV_FILTER + "|" + fn.FCM_FILTER + "|" + fn.GMV_FILTER + "|" +
                             fn.FMV_FILTER + "|" + fn.VBM_FILTER + "|" + fn.M64_FILTER;
            dlgOpen.ShowDialog();

            if (dlgOpen.FileName.Length > 0)
            {
                resetApplication();

                // make sure the file isn't locked before we do anything else
                try { System.IO.File.OpenRead(dlgOpen.FileName); }
                catch
                {
                    MessageBox.Show(dlgOpen.FileName + " cannot be accessed at the moment", "File Possibly Locked");
                    return;
                }

                movieType = fn.IsValid(dlgOpen.FileName);
                switch (movieType)
                {
                    case MovieType.SMV:
                        loadSMVFile(dlgOpen.FileName); break;
                    case MovieType.FCM:
                        loadFCMFile(dlgOpen.FileName); break;
                    case MovieType.GMV:
                        loadGMVFile(dlgOpen.FileName); break;
                    case MovieType.FMV:
                        loadFMVFile(dlgOpen.FileName); break;
                    case MovieType.VBM:
                        loadVBMFile(dlgOpen.FileName); break;
                    case MovieType.M64:
                        loadM64File(dlgOpen.FileName); return; // DEBUG
                    case MovieType.None:
                        dlgOpen.Dispose(); return;
                }
                
                txtMovieFilename.Text = fn.extractFilenameFromPath(dlgOpen.FileName);

                // enable grayed menu options
                mnuSave.Enabled   = true;
                mnuSaveAs.Enabled = true;
                mnuClose.Enabled  = true;

                // populate the virtual listview
                lvInput.VirtualListSource = arrInput;
                lvInput.VirtualListSize = arrInput.Count;
                lvInput.Columns[0].Width = 75;

                // add frame count to statusbar
                sbarFrameCount.Text = arrInput.Count.ToString();
            }

            dlgOpen.Dispose();
        }

        /// <summary>
        /// Close the currently open file
        /// </summary>        
        private void mnuClose_Click(object sender, EventArgs e)
        {
            resetApplication();
        }

        /// <summary>
        /// Reset all objects and controls to their default state
        /// </summary>
        private void resetApplication()
        {
            // dispose movie containers and clear controls
            SMV = null; FCM = null; GMV = null; FMV = null; VBM = null;
            tvInfo.Nodes.Clear();
                                    
            // disable menu commands that require a movie to be loaded in order to operate
            mnuSave.Enabled   = false;
            mnuSaveAs.Enabled = false;
            mnuClose.Enabled  = false;
            
            // reset filename
            txtMovieFilename.Text = "";
            sbarFrameCount.Text   = "0";

            // reset the input list
            lvInput.Clear();            
            lvInput.Columns.Add("Frame");
            lvInput.VirtualListSize = 0;            

            arrInput = null;
            movieType = MovieType.None;
            
            // clear and disable the frame editing fields
            txtFrameDataC1.Enabled = false; txtFrameDataC1.Text = "";
            txtFrameDataC2.Enabled = false; txtFrameDataC2.Text = "";
            txtFrameDataC3.Enabled = false; txtFrameDataC3.Text = "";
            txtFrameDataC4.Enabled = false; txtFrameDataC4.Text = "";
            txtWorkingFrame.Text = "";      
        }

        /// <summary>
        /// Show the About dialog
        /// </summary>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        /// <summary>
        /// Terminate the application
        /// </summary>        
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    #endregion

    #region "Editing Actions"

        /// <summary>
        /// Insert a blank row into the listview at the selectedIndex point and
        /// update the inputArray, or prompt for insertion of multiple frames based
        /// on how many frames were selected.
        /// </summary>
        private void cmnuitemAddFrame_Click(object sender, EventArgs e)
        {
            // make sure something is selected
            if (lvInput.SelectedIndices.Count == 0) return;

            int frameIndex    = lvInput.SelectedIndices[0];
            int framePosition = Convert.ToInt32(lvInput.Items[frameIndex].Text);
            int totalFrames   = lvInput.SelectedIndices[lvInput.SelectedIndices.Count - 1] - frameIndex + 1;

            // prompt for multiple frame insertion
            if (lvInput.SelectedIndices.Count > 1)
            {
                DialogResult confirmAdd = MessageBox.Show("Are you sure you want to insert " + totalFrames + " frames after frame " + framePosition, "Confirm Multiple Frame Insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmAdd != DialogResult.OK) return;
            }

            // insert a blank string[] into the input array
            string[] insertFrame = new string[((string[])(arrInput[0])).Length];
            for (int i = 0; i < totalFrames; i++)
                arrInput.Insert(framePosition + i, insertFrame);

            updateControlsAfterEdit();
        }
       
        /// <summary>
        /// Remove the row from the listview at the selectedIndex point and
        /// update the inputArray, or if multiple rows have been selected, prompt for deletion
        /// </summary>
        private void cmnuitemRemoveFrames_Click(object sender, EventArgs e)
        {
            // make sure something is selected
            if (lvInput.SelectedIndices.Count == 0) return;
            
            int frameIndex    = lvInput.SelectedIndices[0];
            int framePosition = Convert.ToInt32(lvInput.Items[frameIndex].Text);
            int totalFrames   = lvInput.SelectedIndices[lvInput.SelectedIndices.Count - 1] - frameIndex + 1;

            // prompt for multiple frame insertion
            if (lvInput.SelectedIndices.Count > 1)
            {
                DialogResult confirmDelete = MessageBox.Show("Are you sure you want to remove the selected " + totalFrames + " frames", "Confirm Multiple Frame Removal", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmDelete != DialogResult.OK) return;
            }

            // remove the selected frame(s) from the input array                
            //for (int i = 0; i < totalFrames; i++)
            //    arrInput.RemoveAt(framePosition - 1);

            arrInput.RemoveRange(framePosition - 1, totalFrames);

            updateControlsAfterEdit();
        }

        /// <summary>
        /// Update the input array with the changed frame data
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtWorkingFrame.Text.Length != 0)
            {
                int frameIndex        = lvInput.SelectedIndices[0];
                int framePosition     = Convert.ToInt32(lvInput.Items[frameIndex].Text);
                int totalFrames       = lvInput.SelectedIndices[lvInput.SelectedIndices.Count - 1] - frameIndex + 1;
                string[] updatedFrame = new string[((string[])(arrInput[0])).Length];
                ListViewItem lvi      = lvInput.Items[frameIndex];

                // prompt for multiple frame insertion
                if (lvInput.SelectedIndices.Count > 1)
                {
                    DialogResult confirmUpdate = MessageBox.Show("Are you sure you want to update the " + totalFrames + " frames with the same input?", "Confirm Multiple Frame Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (confirmUpdate != DialogResult.OK) return;
                }

                if (txtFrameDataC1.Enabled == true)
                    lvi.SubItems[1].Text = (txtFrameDataC1.Text != null) ? txtFrameDataC1.Text : "";
                if (txtFrameDataC2.Enabled == true)
                    lvi.SubItems[2].Text = (txtFrameDataC2.Text != null) ? txtFrameDataC2.Text : "";
                if (txtFrameDataC3.Enabled == true)
                    lvi.SubItems[3].Text = (txtFrameDataC3.Text != null) ? txtFrameDataC3.Text : "";
                if (txtFrameDataC4.Enabled == true)
                    lvi.SubItems[4].Text = (txtFrameDataC4.Text != null) ? txtFrameDataC4.Text : "";

                // update the string[] we're going to insert into the arrInput arraylist
                for (int i = 0; i < lvInput.Columns.Count - 1; i++)
                    updatedFrame[i] = lvi.SubItems[i + 1].Text;

                for (int j = 0; j < totalFrames; j++)
                {
                    arrInput.RemoveAt(framePosition + j - 1);
                    arrInput.Insert(framePosition - 1 + j, updatedFrame);
                }

                lvInput.Refresh();
            }
        }

        /// <summary>
        /// Save the current movie, overwriting the original
        /// </summary>
        private void mnuSave_Click(object sender, EventArgs e)
        {            
            DialogResult verifyOverwrite = MessageBox.Show("Are you sure you want to overwrite the existing file?", "Confirm Overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (verifyOverwrite != DialogResult.OK)
                return;

            switch (movieType)
            {
                case MovieType.FCM:
                    FCM.Save("", ref arrInput);
                    break;
                case MovieType.FMV:
                    FMV.Save("", ref arrInput);
                    break;
                case MovieType.GMV:
                    GMV.Save("", ref arrInput);
                    break;
                case MovieType.SMV:
                    SMV.Save("", ref arrInput);
                    break;
                case MovieType.VBM:
                    VBM.Save("", ref arrInput);
                    break;                                
            }

            MessageBox.Show(txtMovieFilename.Text + " written successfully", " Save");
        }
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {           
            if (movieType != MovieType.None)
            {
                SaveFileDialog dlgSave = new SaveFileDialog();

                if (movieType == MovieType.FCM) dlgSave.Filter = fn.FCM_FILTER;
                if (movieType == MovieType.FMV) dlgSave.Filter = fn.FMV_FILTER;
                if (movieType == MovieType.GMV) dlgSave.Filter = fn.GMV_FILTER;
                if (movieType == MovieType.SMV) dlgSave.Filter = fn.SMV_FILTER;
                if (movieType == MovieType.VBM) dlgSave.Filter = fn.VBM_FILTER;

                dlgSave.ShowDialog();

                if (dlgSave.FileName.Length > 0)
                {
                    switch (movieType)
                    {
                        case MovieType.FMV:
                            FMV.Save(dlgSave.FileName, ref arrInput);
                            break;
                        case MovieType.FCM:
                            FCM.Save(dlgSave.FileName, ref arrInput);
                            break;
                        case MovieType.GMV:
                            GMV.Save(dlgSave.FileName, ref arrInput);
                            break;
                        case MovieType.SMV:
                            SMV.Save(dlgSave.FileName, ref arrInput);
                            break;
                        case MovieType.VBM:
                            VBM.Save(dlgSave.FileName, ref arrInput);
                            break;   
                    }
                    MessageBox.Show(dlgSave.FileName + " written successfully", " Save As");
                }
            }
        }

        /// <summary>
        /// Show the splicing form
        /// </summary>        
        private void mnuSplice_Click(object sender, EventArgs e)
        {
            frmSplice frm = new frmSplice();
            frm.ShowDialog();
        }

        /// <summary>
        /// Update the listview virtualListSize and the frame count in the statusbar
        /// </summary>
        private void updateControlsAfterEdit()
        {
            lvInput.VirtualListSize = arrInput.Count;
            //lvInput.ClearVirtualCache();
            lvInput.Refresh();
          
            lvInput.SelectedIndices.Clear();

            sbarFrameCount.Text = arrInput.Count.ToString();
        }

    #endregion

    #region "Copy-Pasting"

        /// <summary>
        /// Show the buffer form (pass in the buffer arrayList and the buffer's movieType)
        /// </summary>        
        private void mnuViewBuffer_Click(object sender, EventArgs e)
        {            
            frmBuffer frm = new frmBuffer(ref arrBuffer, bufferMovieType, lvInput.Columns.Count - 1);
            frm.ShowDialog(this);
            frm.Dispose(ref arrBuffer, ref bufferMovieType);
            
            // if the buffer array comes back empty, reset all the copy/paste options
            if (arrBuffer == null)
                clearCopyBuffer();
            else
                enablePasteControls();
        }

        // DEBUG::This is only here until I can figure out why pasting a block more than
        // once crashes
        private void clearCopyBuffer()
        {
            if (arrInput == null) return;

            lvInput.VirtualListSize = arrInput.Count;
            lvInput.Refresh();

            sbarCopyBufferSize.Text = "0";
            bufferMovieType = MovieType.None;
            sbarCopyBufferType.Text = Enum.GetName(typeof(MovieType), bufferMovieType);
            cmnuitemPasteFrames.Enabled = false;
            mnuPaste.Enabled = false;            
        }

        /// <summary>
        /// Copy the selected frames to the buffer arraylist
        /// </summary>
        private void copyFrames()
        {
            // make sure something is selected
            if (lvInput.SelectedIndices.Count == 0) return;
                        
            bufferMovieType   = movieType;
            int frameIndex    = lvInput.SelectedIndices[0];
            int framePosition = Convert.ToInt32(lvInput.Items[frameIndex].Text);
            int totalFrames   = lvInput.SelectedIndices.Count;
                     
            // this is a much faster way to create an arrayList, but the sourceArray (arrInput)
            // still has the max. controllers amount of elements (must parse when pasting)            
            arrBuffer = arrInput.GetRange(frameIndex, totalFrames);
            
            enablePasteControls();   
        }

        /// <summary>
        /// Set various controls when the copy buffer is filled
        /// </summary>
        private void enablePasteControls()
        {
            sbarCopyBufferType.Text     = Enum.GetName(typeof(MovieType), bufferMovieType);
            sbarCopyBufferSize.Text     = arrBuffer.Count.ToString();
            cmnuitemPasteFrames.Enabled = true;
            mnuPaste.Enabled            = true;
        }

        /// <summary>
        /// Copy Frames (main menu)
        /// </summary>        
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            copyFrames();
        }

        /// <summary>
        /// Copy Frames (context menu)
        /// </summary>        
        private void cmnuitemCopyFrames_Click(object sender, EventArgs e)
        {
            copyFrames();
        }

        /// <summary>
        /// Insert the buffered frame input at the selected position.
        /// </summary>
        private void pasteFrames()
        {
            // check for a valid paste position
            if (lvInput.SelectedIndices.Count == 0) return;
            

            // DEBUG::If frames are loaded from file, they may not be of the same controller length
            try
            {
                if (((string[])(arrBuffer[0])).Length != lvInput.Columns.Count - 1)
                {
                    MessageBox.Show("Copy buffer doesn't contain the same number of controller columns as the current movie, or the copy buffer became corrupt\nClearing contents. (This'll be fixed soon-ish)", "Oops");
                    clearCopyBuffer();
                    return;
                }
            }
            // DEBUG::This'll catch an attempt to acces a corrupted buffer
            catch
            {
                MessageBox.Show("Copy buffer became corrupt ... Clearing contents.\nNote that this seems to happen if you copy or load a buffer, add/remove frames then attempt a paste", "Oops");
                clearCopyBuffer();
                return;
            }

            // ensure movie types are the same
            if (movieType != bufferMovieType)
            {
                MessageBox.Show("Cannot paste " + Enum.GetName(typeof(MovieType), bufferMovieType) + 
                    " frames into a " + Enum.GetName(typeof(MovieType), movieType) + " movie file", 
                    "Type Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int frameIndex    = lvInput.SelectedIndices[0];
            int framePosition = Convert.ToInt32(lvInput.Items[frameIndex].Text);            

            // confirm that the paste should occur
            DialogResult confirmPaste = MessageBox.Show("Are you sure you want to paste " + arrBuffer.Count + " frames after frame " + framePosition, "Confirm Paste", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirmPaste != DialogResult.OK) return;

            // DEBUG::this works ONCE, then dies
            // TODO::either force this to work or go back to old method of manual array population            
            arrInput.InsertRange(framePosition, arrBuffer);            

            // DEBUG::This is only here temporarily until the multi-paste crash bug is fixed
            arrBuffer = null;
            clearCopyBuffer();

            updateControlsAfterEdit();            
        }
        
        /// <summary>
        /// Paste frames (main menu)
        /// </summary>        
        private void mnuPaste_Click(object sender, EventArgs e)
        {
            pasteFrames();
        }

        /// <summary>
        /// Paste frames (context menu)
        /// </summary>
        private void cmnuitemPasteFrames_Click(object sender, EventArgs e)
        {
            pasteFrames();
        }

    #endregion      
     
    }
}
