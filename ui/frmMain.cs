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
/***************************************************************************************************
 * TAS Movie Splicer
 * Author: Alex Bevilacqua (aka Maximus)
 * Revision: 0-8
 **************************************************************************************************/
namespace MovieSplicer.UI
{
    public partial class frmMain : Form
    {
        Functions      fn        = new Functions();
        MovieType      movieType = MovieType.None;
        OpenFileDialog dlgOpen;             
        
        static ArrayList arrInput;  // this array will hold the input from the loaded movie file                        
        MovieType        bufferMovieType = MovieType.None;
        static ArrayList arrBuffer; // this array will hold the buffered frame data for copy-pasting

        public frmMain()
        {
            InitializeComponent();         
        }

    #region "Misc Form Control Event Handlers"
       
        /// <summary>
        /// Keep the user locked into the current movie format when a movie is loaded.
        /// This is a hack since tabPages don't have direct access to an Enabled property
        /// </summary>        
        private void tabsEmulators_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (movieType == MovieType.SMV && tabsEmulators.SelectedTab == tabSMV) e.Cancel = true;
            if (movieType == MovieType.FMV && tabsEmulators.SelectedTab == tabFMV) e.Cancel = true;
            if (movieType == MovieType.GMV && tabsEmulators.SelectedTab == tabGMV) e.Cancel = true;
            if (movieType == MovieType.FCM && tabsEmulators.SelectedTab == tabFCM) e.Cancel = true;
            if (movieType == MovieType.VBM && tabsEmulators.SelectedTab == tabVBM) e.Cancel = true;
        }

    #endregion 

    #region "SMV Specific"

        static SNES9x SMV;

        /// <summary>
        /// Instantiates the SNES9x object with a validated SMV file read in from
        /// an OpenDialog call
        /// </summary>
        private void loadSMVFile(string filename)
        {
            tabsEmulators.SelectedTab = tabSMV;
            clearSMVTabControls();

            SMV      = new SNES9x(filename);
            arrInput = SMV.ControllerData.ControllerInput;

            txtFrameCount.Text    = String.Format("{0:0,0}", SMV.Header.FrameCount);            
            txtReRecordCount.Text = String.Format("{0:0,0}", SMV.Header.ReRecordCount);
            txtRomTitle.Text      = SMV.ROMInfo.Name;
            txtRomCRC.Text        = SMV.ROMInfo.CRC;
            txtRecordingDate.Text = SMV.Header.UID;
            txtMetadata.Text      = SMV.Header.Metadata;            
            sbarFrameCount.Text   = SMV.Header.FrameCount.ToString();
            
            setSyncMiscOptions();
            setSMVControllersUsed();
        }

        /// <summary>
        /// Clear the SMV tab controls
        /// </summary>
        private void clearSMVTabControls()
        {
            txtFrameCount.Text    = "";
            txtReRecordCount.Text = "";
            txtRomCRC.Text        = "";
            txtRomTitle.Text      = "";
            txtRecordingDate.Text = "";
            txtMetadata.Text      = "";

            for (int i = 0; i < 5; i++) chklstSyncOptions.SetItemChecked(i, false);
            for (int i = 0; i < 5; i++) chklstControllers.SetItemChecked(i, false);
            for (int i = 0; i < 4; i++) chklstMovieOptions.SetItemChecked(i, false);
        }

        private void setSyncMiscOptions()
        {
            if (SMV.Options.WIP1TIMING) chklstSyncOptions.SetItemChecked(0, true);
            if (SMV.Options.LEFTRIGHT)  chklstSyncOptions.SetItemChecked(1, true);
            if (SMV.Options.VOLUMEENVX) chklstSyncOptions.SetItemChecked(2, true);
            if (SMV.Options.FAKEMUTE)   chklstSyncOptions.SetItemChecked(3, true);
            if (SMV.Options.SYNCSOUND)  chklstSyncOptions.SetItemChecked(4, true);
            if (SMV.Options.RESET)
                chklstMovieOptions.SetItemChecked(1, true);
            else
                chklstMovieOptions.SetItemChecked(0, true);
            if (SMV.Options.PAL)
                chklstMovieOptions.SetItemChecked(3, true);
            else
                chklstMovieOptions.SetItemChecked(2, true);
        }

        /// <summary>
        /// Set listView columns based on the number of controllers in use and
        /// populates the checkedlistbox
        /// </summary>
        public void setSMVControllersUsed()
        {            
            for (int i = 0; i < 5; i++)
            {
                if (SMV.ControllerData.Controller[i] == true)
                {
                    lvInput.Columns.Add("Controller " + (i + 1), 75);
                    chklstControllers.SetItemChecked(i, true);
                }
            }

            // enable the input controls based on which controllers are in use
            // TODO::I don't really like this method
            txtFrameDataC1.Enabled = SMV.ControllerData.Controller[0];
            txtFrameDataC2.Enabled = SMV.ControllerData.Controller[1];
            txtFrameDataC3.Enabled = SMV.ControllerData.Controller[2];
            txtFrameDataC4.Enabled = SMV.ControllerData.Controller[3];
        }

    #endregion

    #region "FCM Specific"
        
        static FCEU FCM;

        /// <summary>
        /// Instantiates the FCEU object with a validated FCM file read in from
        /// an OpenDialog call
        /// </summary>
        private void loadFCMFile(string filename)
        {
            tabsEmulators.SelectedTab = tabFCM;
            clearFCMTabControls();

            FCM      = new FCEU(filename);
            arrInput = FCM.ControllerData.ControllerInput;                        

            txtFCMRomTitle.Text      = FCM.Header.ROMName;
            txtFCMRomCRC.Text        = FCM.Header.ROMCRC;
            txtFCMFrameCount.Text    = String.Format("{0:0,0}", FCM.Header.FrameCount);
            txtFCMRerecordCount.Text = String.Format("{0:0,0}", FCM.Header.ReRecordCount);
            txtFCMEmulatorUsed.Text  = FCM.Header.EmulatorVersion.ToString();
            txtFCMAuthor.Text        = FCM.Header.Author;            
            sbarFrameCount.Text      = FCM.Header.FrameCount.ToString();
                       
            setFCMOptions();
            setFCMControllersUsed();
        }
        
        /// <summary>
        /// Clear the FCEU tab page's controls
        /// </summary>
        private void clearFCMTabControls()
        {
            txtFCMRerecordCount.Text = "";
            txtFCMFrameCount.Text    = "";
            txtFCMRomCRC.Text        = "";
            txtFCMRomTitle.Text      = "";
            txtFCMEmulatorUsed.Text  = "";
            txtFCMAuthor.Text        = "";
            for (int i = 0; i < 4; i++) chklstFCMControllers.SetItemChecked(i, false);
            for (int i = 0; i < 2; i++) chklstFCMMovieStart.SetItemChecked(i, false);
        }
        
        private void setFCMOptions()
        {
            if (FCM.Header.StartFromReset == true)  chklstFCMMovieStart.SetItemChecked(0, true);
            if (FCM.Header.StartFromReset == false) chklstFCMMovieStart.SetItemChecked(1, true);
        }
       
        private void setFCMControllersUsed()
        {                       
            for (int i = 0; i < 4; i++)
            {
                if (FCM.ControllerData.Controller[i] == true)
                {
                    lvInput.Columns.Add("Controller " + (i + 1), 75);
                    chklstFCMControllers.SetItemChecked(i, true);
                }
            }

            // enable the input controls based on which controllers are in use
            // TODO::I don't really like this method
            txtFrameDataC1.Enabled = FCM.ControllerData.Controller[0];
            txtFrameDataC2.Enabled = FCM.ControllerData.Controller[1];
            txtFrameDataC3.Enabled = FCM.ControllerData.Controller[2];
            txtFrameDataC4.Enabled = FCM.ControllerData.Controller[3];
        }       

    #endregion

    #region "GMV Specific"

        static Gens GMV;

        /// <summary>
        /// Instantiates the Gens object with a validated GMV file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadGMVFile(string filename)
        {            
            tabsEmulators.SelectedTab = tabGMV;
            clearGMVTabControls();

            GMV      = new Gens(filename);
            arrInput = GMV.ControllerData.ControllerInput;         

            txtGMVMovieVersion.Text  = GMV.Header.Signature;
            txtGMVRerecordCount.Text = String.Format("{0:0,0}", GMV.Header.RerecordCount);            
            txtGMVFrameCount.Text    = String.Format("{0:0,0}", GMV.Header.FrameCount);            
            txtGMVAuthorNotes.Text   = GMV.Header.MovieName;            
            sbarFrameCount.Text      = GMV.Header.FrameCount.ToString();
               
            setGMVOptions();
            setGMVControllersUsed();
        }

        /// <summary>
        /// Set the GMV's options in the related form controls
        /// </summary>
        private void setGMVOptions()
        {
            if (GMV.Header.Player1Config == "3")
                optGMVC1_3Button.Checked = true;
            else
                optGMVC1_6Button.Checked = true;

            if (GMV.Header.Player2Config == "3")
                optGMVC2_3Button.Checked = true;
            else
                optGMVC2_6Button.Checked = true;

            if (GMV.Header.Version > 0x09)
            {
                txtGMVFPS.Text = GMV.Options.FPS;
                chklstGMVMovieStart.SetItemChecked(0, GMV.Options.StartFromReset);
                chklstGMVMovieStart.SetItemChecked(1, GMV.Options.StartFromSave);
                
                chklstGMVControllers.SetItemChecked(0, true);
                chklstGMVControllers.SetItemChecked(1, true);

                txtFrameDataC1.Enabled = true;
                txtFrameDataC2.Enabled = true;

                if (GMV.Options.ControllerCount == 3)
                {
                    chklstGMVControllers.SetItemChecked(2, true);
                    txtFrameDataC3.Enabled = true;
                }
                    
            }
        }

        /// <summary>
        /// Clear the GMV tab's form control values
        /// </summary>
        private void clearGMVTabControls()
        {
            txtGMVMovieVersion.Text  = "";
            txtGMVRerecordCount.Text = "";
            txtGMVFrameCount.Text    = "";
            txtGMVAuthorNotes.Text   = "";
            optGMVC1_3Button.Checked = false;
            optGMVC1_6Button.Checked = false;
            optGMVC2_3Button.Checked = false;
            optGMVC2_6Button.Checked = false;
            txtGMVFPS.Text           = "";

            // set controllers and options
            for (int i = 0; i < chklstGMVControllers.Items.Count; i++)
                chklstGMVControllers.SetItemChecked(i, false);
            for (int i = 0; i < chklstGMVMovieStart.Items.Count; i++)
                chklstGMVMovieStart.SetItemChecked(i, false);
        }  
      
        /// <summary>
        /// Set the available (if version is above 9) options used in the movie
        /// </summary>
        private void setGMVControllersUsed()
        {   
            if (GMV.Header.Version > 0x09)
                for (int i = 1; i <= GMV.Options.ControllerCount; i++)
                    lvInput.Columns.Add("Controller " + i, 75);
            else
            {
                lvInput.Columns.Add("Controller 1", 75);
                lvInput.Columns.Add("Controller 2", 75);
            }
        }        
    
    #endregion   

    #region "FMV Specific"

        static Famtasia FMV;

        /// <summary>
        /// Instantiates the Gens object with a validated GMV file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadFMVFile(string filename)
        {
            tabsEmulators.SelectedTab = tabFMV;
            clearFMVTabControls();

            FMV = new Famtasia(filename);
            arrInput = FMV.ControllerInput;

            txtFMVEmulatorID.Text    = FMV.Header.EmulatorID;
            txtFMVFrameCount.Text    = String.Format("{0:0,0}", FMV.Header.FrameCount);
            sbarFrameCount.Text      = FMV.Header.FrameCount.ToString();
            txtFMVMovieTitle.Text    = FMV.Header.MovieTitle;
            txtFMVRerecordCount.Text = String.Format("{0:0,0}", FMV.Header.ReRecordCount);

            chklstFMVOptions.SetItemChecked(0, FMV.Header.StartFromReset);
            chklstFMVOptions.SetItemChecked(1, FMV.Header.StartFromSave);

            setFMVControllersUsed();          
        }

        private void clearFMVTabControls()
        {
            txtFMVEmulatorID.Text    = "";
            txtFMVFrameCount.Text    = "";
            txtFMVMovieTitle.Text    = "";
            txtFMVRerecordCount.Text = "";
            chklstFMVControllers.SetItemChecked(0, false);
            chklstFMVControllers.SetItemChecked(1, false);
            chklstFMVOptions.SetItemChecked(0, false);
            chklstFMVOptions.SetItemChecked(1, false);
        }
        
        private void setFMVControllersUsed()
        {
           
            if (FMV.Header.Controllers[0] == true)
            {
                lvInput.Columns.Add("Controller 1", 75);
                chklstFMVControllers.SetItemChecked(0, true);
                txtFrameDataC1.Enabled = true;
            }
            if (FMV.Header.Controllers[1] == true)
            {
                lvInput.Columns.Add("Controller 2", 75);
                chklstFMVControllers.SetItemChecked(1, true);
                txtFrameDataC2.Enabled = true;
            }
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
            // if not numeric, return
            if (fn.IsNumeric(txtJumpToFrame.Text) == false) return;

            // subtract 1 since we're looking for an index
            int targetFrame = Convert.ToInt32(txtJumpToFrame.Text) - 1;                

            // check for valid range
            if(targetFrame <= Convert.ToInt32(sbarFrameCount.Text) && targetFrame > 0)
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
            bool validFile = false;

            dlgOpen        = new OpenFileDialog();
            dlgOpen.Filter = fn.ALL_FILTER + "|" + fn.SMV_FILTER + "|" + fn.FCM_FILTER + "|" + fn.GMV_FILTER + "|" + fn.FMV_FILTER;
            dlgOpen.ShowDialog();

            if (dlgOpen.FileName.Length > 0)
            {
                resetApplication();

                // load an SMV file if it verifies
                if (fn.IsValidSMV(dlgOpen.FileName))
                {
                    movieType = MovieType.SMV;
                    loadSMVFile(dlgOpen.FileName);
                    validFile = true;
                }
                // load an FCM file if it verifies                
                else if (fn.IsValidFCM(dlgOpen.FileName))
                {
                    movieType = MovieType.FCM;
                    loadFCMFile(dlgOpen.FileName);
                    validFile = true;
                }
                // load a GMV file if it verifies
                else if (fn.IsValidGMV(dlgOpen.FileName))
                {
                    movieType = MovieType.GMV;
                    loadGMVFile(dlgOpen.FileName);
                    validFile = true;
                }
                // load a FMV file if it verifies
                else if (fn.IsValidFMV(dlgOpen.FileName))
                {
                    movieType = MovieType.FMV;
                    loadFMVFile(dlgOpen.FileName);
                    validFile = true;
                }
                if (validFile == true)
                {
                    // add the opened filename to the form caption
                    sbarFrameCount.ForeColor = Color.Blue;
                    sbarMovieName.Text = fn.extractFilenameFromPath(dlgOpen.FileName);

                    // enable grayed menu options
                    mnuSave.Enabled  = true;
                    mnuSaveAs.Enabled = true;
                    mnuClose.Enabled  = true;

                    // populate the virtual listview
                    lvInput.VirtualListSource = arrInput;
                    lvInput.VirtualListSize = arrInput.Count;
                    lvInput.Columns[0].Width = 75;                    
                }
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
            SMV = null; clearSMVTabControls();
            FCM = null; clearFCMTabControls();
            GMV = null; clearGMVTabControls();
            FMV = null; clearFMVTabControls();
            
            arrInput  = null;
            movieType = MovieType.None;

            // disable menu commands that require a movie to be loaded in order to operate
            mnuSave.Enabled   = false;
            mnuSaveAs.Enabled = false;
            mnuClose.Enabled  = false;

            // reset the input list
            lvInput.Clear();            
            lvInput.Columns.Add("Frame");
            lvInput.VirtualListSize = 0;                      

            // clear the status bar info
            sbarMovieName.Text       = "No Movie Loaded";
            sbarFrameCount.Text      = "0";
            sbarFrameCount.ForeColor = Color.Blue;

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

            // remove the selected frame(2) from the input array                
            for (int i = 0; i < totalFrames; i++)
                arrInput.RemoveAt(framePosition - 1);

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
            // HACK::Not sure if FMV will be supported so just break out of the routine
            if (movieType == MovieType.FMV)
            {
                MessageBox.Show("Famtasia movies cannot be saved", "Oops");
                return;
            }
            
            DialogResult verifyOverwrite = MessageBox.Show("Are you sure you want to overwrite the existing file?", "Confirm Overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (verifyOverwrite != DialogResult.OK)
                return;

            switch (movieType)
            {
                case MovieType.FCM:
                    FCM.Save("", ref arrInput);
                    break;
                case MovieType.GMV:
                    GMV.Save("", ref arrInput);
                    break;
                case MovieType.SMV:
                    SMV.Save("", ref arrInput);
                    break;                                                    
            }

            MessageBox.Show(sbarMovieName.Text + " written successfully", " Save");
        }
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            // HACK::Not sure if FMV will be supported so just break out of the routine
            if (movieType == MovieType.FMV)
            {
                MessageBox.Show("Famtasia movies cannot be saved", "Oops");
                return;
            }

            if (movieType != MovieType.None)
            {
                SaveFileDialog dlgSave = new SaveFileDialog();

                if (movieType == MovieType.FCM) dlgSave.Filter = fn.FCM_FILTER;
                if (movieType == MovieType.GMV) dlgSave.Filter = fn.GMV_FILTER;
                if (movieType == MovieType.SMV) dlgSave.Filter = fn.SMV_FILTER;

                dlgSave.ShowDialog();

                if (dlgSave.FileName.Length > 0)
                {
                    switch (movieType)
                    {
                        case MovieType.FCM:
                            FCM.Save(dlgSave.FileName, ref arrInput);
                            break;
                        case MovieType.GMV:
                            GMV.Save(dlgSave.FileName, ref arrInput);
                            break;
                        case MovieType.SMV:
                            SMV.Save(dlgSave.FileName, ref arrInput);
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
            lvInput.Refresh();

            sbarFrameCount.ForeColor = Color.Red;
            sbarFrameCount.Text = arrInput.Count.ToString();

            lvInput.SelectedIndices.Clear();
        }

    #endregion

    #region "Copy-Pasting"

        /// <summary>
        /// Show the buffer form (pass in the buffer arrayList and the buffer's movieType)
        /// </summary>        
        private void mnuViewBuffer_Click(object sender, EventArgs e)
        {            
            frmBuffer frm = new frmBuffer(arrBuffer, bufferMovieType, lvInput.Columns.Count - 1);
            frm.ShowDialog(this);
            frm.Dispose(ref arrBuffer);
            
            // if the buffer array comes back empty, reset all the copy/paste options
            if (arrBuffer == null)            
                clearCopyBuffer(); 
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
            
            arrBuffer       = new ArrayList();
            bufferMovieType = movieType;

            int frameIndex    = lvInput.SelectedIndices[0];
            int framePosition = Convert.ToInt32(lvInput.Items[frameIndex].Text);
            int totalFrames   = lvInput.SelectedIndices.Count;
                     
            // this is a much faster way to create an arrayList, but the sourceArray (arrInput)
            // still has the max. controllers amount of elements (must parse when pasting)
            arrBuffer = arrInput.GetRange(frameIndex, totalFrames);           
            
            sbarCopyBufferType.Text     = Enum.GetName(typeof(MovieType), bufferMovieType);
            sbarCopyBufferSize.Text     = arrBuffer.Count.ToString();
            cmnuitemPasteFrames.Enabled = true;
            mnuPaste.Enabled            = true;            
        }
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            copyFrames();
        }
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
        private void mnuPaste_Click(object sender, EventArgs e)
        {
            pasteFrames();
        }
        private void cmnuitemPasteFrames_Click(object sender, EventArgs e)
        {
            pasteFrames();
        }

    #endregion

    }
}
