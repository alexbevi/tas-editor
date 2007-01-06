using System;
using System.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Data;
using MovieSplicer.Data.Formats;
using MovieSplicer.Components;

/***************************************************************************************************
 * TAS Movie Editor
 * Author: Alex Bevilacqua (aka Maximus)
 * Revision: 0-9-1
 * 
 * Conventions:
 * - Class level variables, Methods and Properties are denoted with each word appearing with the 
 *   first letter capitalized: ie. Frame Data, Save()
 * - Private class variables and methods are denoted with the first word being lowercase, then the
 *   subsequent words having their first letter capitalized: ie. openDlg, parseControllerData()
 **************************************************************************************************/
namespace MovieSplicer.UI
{
    public partial class frmMain : TASForm
    {                       
        MovieType MovieFormat       = MovieType.None;
        MovieType BufferMovieFormat = MovieType.None;        
              
        private TASMovieInput[] FrameData;
        private TASMovieInput[] FrameBuffer;

        public frmMain()
        {
            InitializeComponent();
        }

    #region Instantiate Movie Objects

        static SNES9x SMV;

        /// <summary>
        /// Instantiates the SNES9x object with a validated SMV file read in from
        /// an OpenDialog call
        /// </summary>
        private void loadSMVFile(string filename)
        {
            SMV = new SNES9x(filename);
            FrameData = SMV.Input.FrameData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.SMV(ref tvInfo, ref SMV);

            lvInput.SetColumns(SMV.Input.ControllerCount);
            txtFrameDataC1.Enabled = SMV.Input.Controllers[0];
            txtFrameDataC2.Enabled = SMV.Input.Controllers[1];
            txtFrameDataC3.Enabled = SMV.Input.Controllers[2];
            txtFrameDataC4.Enabled = SMV.Input.Controllers[3];
        }

        static FCEU FCM;

        /// <summary>
        /// Instantiates the FCEU object with a validated FCM file read in from
        /// an OpenDialog call
        /// </summary>
        private void loadFCMFile(string filename)
        {
            FCM = new FCEU(filename);
            FrameData = FCM.Input.FrameData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.FCM(ref tvInfo, ref FCM);

            lvInput.SetColumns(FCM.Input.ControllerCount);
            txtFrameDataC1.Enabled = FCM.Input.Controllers[0];
            txtFrameDataC2.Enabled = FCM.Input.Controllers[1];
            txtFrameDataC3.Enabled = FCM.Input.Controllers[2];
            txtFrameDataC4.Enabled = FCM.Input.Controllers[3];
        }

        static Gens GMV;

        /// <summary>
        /// Instantiates the Gens object with a validated GMV file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadGMVFile(string filename)
        {
            GMV = new Gens(filename);
            FrameData = GMV.Input.FrameData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.GMV(ref tvInfo, ref GMV);

            lvInput.SetColumns(GMV.Input.ControllerCount);
            txtFrameDataC1.Enabled = GMV.Input.Controllers[0];
            txtFrameDataC2.Enabled = GMV.Input.Controllers[1];
            if (GMV.Input.Controllers.Length == 3)
                txtFrameDataC3.Enabled = GMV.Input.Controllers[2];
        }

        static Famtasia FMV;

        /// <summary>
        /// Instantiates the Gens object with a validated GMV file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadFMVFile(string filename)
        {
            FMV = new Famtasia(filename);
            FrameData = FMV.Input.FrameData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.FMV(ref tvInfo, ref FMV);

            lvInput.SetColumns(FMV.Input.ControllerCount);
            txtFrameDataC1.Enabled = FMV.Input.Controllers[0];
            txtFrameDataC2.Enabled = FMV.Input.Controllers[1];
        }

        static VisualBoyAdvance VBM;

        /// <summary>
        /// Instantiates the Gens object with a validated VBM file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadVBMFile(string filename)
        {
            VBM = new VisualBoyAdvance(filename);
            FrameData = VBM.Input.FrameData;

            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.VBM(ref tvInfo, ref VBM);

            lvInput.SetColumns(VBM.Input.ControllerCount);
            txtFrameDataC1.Enabled = VBM.Input.Controllers[0];
            txtFrameDataC2.Enabled = VBM.Input.Controllers[1];
            txtFrameDataC3.Enabled = VBM.Input.Controllers[2];
            txtFrameDataC4.Enabled = VBM.Input.Controllers[3];
        }

        static Mupen64 M64;

        /// <summary>
        /// Instantiates the Mupen64 object with a validated M64 file read in from
        /// an OpenDialog call
        /// </summary>        
        private void loadM64File(string filename)
        {
            M64 = new Mupen64(filename);


            tvInfo.Nodes.Clear();
            Methods.PopulateMovieInfo.M64(ref tvInfo, ref M64);
        }  

    #endregion

    #region "Frame Data Handlers"

        /// <summary>
        /// Populate edit controls with frame data        
        /// </summary>
        private void lvInput_Clicked(object sender, EventArgs e)
        {
            if (lvInput.SelectedIndices.Count > 0 && MovieFormat != MovieType.None)
            {
                ListViewItem lvi = lvInput.Items[lvInput.SelectedIndices[0]];
                
                txtFrameDataC1.Text = lvi.SubItems[1].Text;
                if (lvi.SubItems.Count > 2) txtFrameDataC2.Text = lvi.SubItems[2].Text;
                if (lvi.SubItems.Count > 3) txtFrameDataC3.Text = lvi.SubItems[3].Text;
                if (lvi.SubItems.Count > 4) txtFrameDataC4.Text = lvi.SubItems[4].Text;
            }
        }                

        /// <summary>
        /// Jump to a selected frame
        /// </summary>        
        private void btnGo_Click(object sender, EventArgs e)
        {                        
            // if not numeric or no movie loaded
            if (IsNumeric(txtJumpToFrame.Text) == false || FrameData == null) return;

            // subtract 1 since we're looking for an index
            int targetFrame = Convert.ToInt32(txtJumpToFrame.Text) - 1;                

            // check for valid range
            if(targetFrame <= FrameData.Length && targetFrame > 0)
            {
                lvInput.Items[targetFrame].Selected = true;
                lvInput.Focus();
                lvInput.EnsureVisible(targetFrame);
            }
        }

        /// <summary>
        /// Locate the selected substring in the current TASMovieInput[] object
        /// </summary>        
        private void btnFindInput_Click(object sender, EventArgs e)
        {
            if (FrameData == null || txtJumpToFrame.Text.Length == 0) return;

            int start = (lvInput.SelectedIndices.Count > 0) ? lvInput.SelectedIndices[0] : 0;
            int position = TASMovieInput.Search(ref FrameData, txtJumpToFrame.Text, start);

            if (position > 0)
            {
                lvInput.Items[position].Selected = true;
                lvInput.Focus();
                lvInput.EnsureVisible(position);
            }
            else
                MessageBox.Show("Input pattern not found between selected position and end of movie", "Sorry");
        }

        // NOTE::These routines are just a quick hack to ensure that the edit field checkboxes
        // maintain the same enabled state as their corresponding text fields
        private void txtFrameDataC1_EnabledChanged(object sender, EventArgs e)
        {
            chkFrameDataC1.Enabled = txtFrameDataC1.Enabled;
        }
        private void txtFrameDataC2_EnabledChanged(object sender, EventArgs e)
        {
            chkFrameDataC2.Enabled = txtFrameDataC2.Enabled;
        }
        private void txtFrameDataC3_EnabledChanged(object sender, EventArgs e)
        {
            chkFrameDataC3.Enabled = txtFrameDataC3.Enabled;
        }
        private void txtFrameDataC4_EnabledChanged(object sender, EventArgs e)
        {
            chkFrameDataC4.Enabled = txtFrameDataC4.Enabled;
        }
    
    #endregion

    #region "Menu Actions"

        /// <summary>
        /// Show an OpenFileDialog to allow a file to be selected and loaded
        /// </summary>        
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            openDlg        = new OpenFileDialog();
            openDlg.Filter = TAS_FILTER;
            
            openDlg.ShowDialog();

            if (openDlg.FileName.Length > 0)
            {
                resetApplication();

                // make sure the file isn't locked before we do anything else
                try { System.IO.File.OpenRead(openDlg.FileName); }
                catch
                {
                    MessageBox.Show(openDlg.FileName + " cannot be accessed at the moment", "File Possibly Locked");
                    return;
                }

                MovieFormat = IsValid(openDlg.FileName);
                switch (MovieFormat)
                {
                    case MovieType.SMV:
                        loadSMVFile(openDlg.FileName); break;
                    case MovieType.FCM:
                        loadFCMFile(openDlg.FileName); break;
                    case MovieType.GMV:
                        loadGMVFile(openDlg.FileName); break;
                    case MovieType.FMV:
                        loadFMVFile(openDlg.FileName); break;
                    case MovieType.VBM:
                        loadVBMFile(openDlg.FileName); break;
                    case MovieType.M64:
                        loadM64File(openDlg.FileName); return; // DEBUG
                    case MovieType.None:
                        openDlg.Dispose(); return;
                }
                
                txtMovieFilename.Text = FilenameFromPath(openDlg.FileName);

                // enable grayed menu options
                mnuSave.Enabled   = true;
                mnuSaveAs.Enabled = true;
                mnuClose.Enabled  = true;

                // populate the virtual listview                
                lvInput.VirtualListSource = FrameData;
                lvInput.VirtualListSize   = FrameData.Length;                

                // add frame count to statusbar
                sbarFrameCount.Text = FrameData.Length.ToString();
            }

            openDlg.Dispose();
        }

        /// <summary>
        /// Save the current movie, overwriting the original
        /// </summary>
        private void mnuSave_Click(object sender, EventArgs e)
        {
            DialogResult verifyOverwrite = MessageBox.Show("Are you sure you want to overwrite the existing file?", "Confirm Overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (verifyOverwrite != DialogResult.OK)
                return;

            switch (MovieFormat)
            {
                case MovieType.FCM:
                    FCM.Save("", ref FrameData);
                    break;
                case MovieType.FMV:
                    FMV.Save("", ref FrameData);
                    break;
                case MovieType.GMV:
                    GMV.Save("", ref FrameData);
                    break;
                case MovieType.SMV:
                    SMV.Save("", ref FrameData);
                    break;
                case MovieType.VBM:
                    VBM.Save("", ref FrameData);
                    break;
            }

            MessageBox.Show(txtMovieFilename.Text + " written successfully", " Save");
        }
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (MovieFormat != MovieType.None)
            {
                saveDlg = new SaveFileDialog();

                // set the save dialog's file filter type according to the current format
                if (MovieFormat == MovieType.FCM) saveDlg.Filter = FCM_FILTER;
                if (MovieFormat == MovieType.FMV) saveDlg.Filter = FMV_FILTER;
                if (MovieFormat == MovieType.GMV) saveDlg.Filter = GMV_FILTER;
                if (MovieFormat == MovieType.SMV) saveDlg.Filter = SMV_FILTER;
                if (MovieFormat == MovieType.VBM) saveDlg.Filter = VBM_FILTER;

                saveDlg.ShowDialog();

                if (saveDlg.FileName.Length > 0)
                {
                    switch (MovieFormat)
                    {
                        case MovieType.FMV:
                            FMV.Save(saveDlg.FileName, ref FrameData);
                            break;
                        case MovieType.FCM:
                            FCM.Save(saveDlg.FileName, ref FrameData);
                            break;
                        case MovieType.GMV:
                            GMV.Save(saveDlg.FileName, ref FrameData);
                            break;
                        case MovieType.SMV:
                            SMV.Save(saveDlg.FileName, ref FrameData);
                            break;
                        case MovieType.VBM:
                            VBM.Save(saveDlg.FileName, ref FrameData);
                            break;
                    }
                    MessageBox.Show(saveDlg.FileName + " written successfully", " Save As");
                }
            }
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
            //SMV = null; FCM = null; GMV = null; FMV = null; VBM = null;
            tvInfo.Nodes.Clear();
                                    
            // disable menu commands that require a movie to be loaded in order to operate
            mnuSave.Enabled   = false;
            mnuSaveAs.Enabled = false;
            mnuClose.Enabled  = false;
            
            // reset filename
            txtMovieFilename.Text = "";
            sbarFrameCount.Text   = "0";

            // reset the input list
            lvInput.SetColumns(0);            
            lvInput.VirtualListSize = 0;            

            FrameData   = null;
            MovieFormat = MovieType.None;
            
            // clear and disable the frame editing fields
            txtFrameDataC1.Enabled = false; txtFrameDataC1.Text = "";
            txtFrameDataC2.Enabled = false; txtFrameDataC2.Text = "";
            txtFrameDataC3.Enabled = false; txtFrameDataC3.Text = "";
            txtFrameDataC4.Enabled = false; txtFrameDataC4.Text = "";
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

            TASMovieInput.Insert(ref FrameData, framePosition, totalFrames);
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

            TASMovieInput.Remove(ref FrameData, frameIndex, totalFrames);

            // ensures that the virtual list doesn't try to access an element that no longer exists
            // after a block of frames is deleted
            int selected = lvInput.SelectedIndices[0];            
            
            updateControlsAfterEdit();

            // make sure we don't try to access a selected row that's out of range
            if (selected <= FrameData.Length)
            {
                // if we've removed all frames up to this point, the index == selected, so decrement
                if (selected == FrameData.Length) selected--;
                
                lvInput.Items[selected].Selected = true;
                lvInput.Focus();
                lvInput.EnsureVisible(selected);
            }
        }

        /// <summary>
        /// Update the input array with the changed frame data
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvInput.SelectedIndices.Count > 0)
            {
                int frameIndex        = lvInput.SelectedIndices[0];
                int framePosition     = Convert.ToInt32(lvInput.Items[frameIndex].Text);
                int totalFrames       = lvInput.SelectedIndices[lvInput.SelectedIndices.Count - 1] - frameIndex + 1;
                bool[] updateFlag     = { false, false, false, false };
                
                TASMovieInput updated = new TASMovieInput();
                for (int i = 0; i < updated.Controller.Length; i++)
                    updated.Controller[i] = FrameData[frameIndex].Controller[i];
                
                if (chkFrameDataC1.Checked)
                {
                    updated.Controller[0] = txtFrameDataC1.Text;
                    updateFlag[0] = true;
                }
                if (chkFrameDataC2.Checked)
                {
                    updated.Controller[1] = txtFrameDataC2.Text;
                    updateFlag[1] = true;
                }
                if (chkFrameDataC3.Checked)
                {
                    updated.Controller[2] = txtFrameDataC3.Text;
                    updateFlag[2] = true;
                }
                if (chkFrameDataC4.Checked)
                {
                    updated.Controller[3] = txtFrameDataC4.Text;
                    updateFlag[3] = true;
                }

                // if no controllers were set, return
                if (updateFlag[0] == false && updateFlag[1] == false && updateFlag[2] == false && updateFlag[3] == false)
                    return;

                // prompt for multiple frame insertion
                if (lvInput.SelectedIndices.Count > 1)
                {
                    DialogResult confirmUpdate = MessageBox.Show("Are you sure you want to update the " + totalFrames + " frames with the same input?", "Confirm Multiple Frame Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (confirmUpdate != DialogResult.OK) return;
                }
                bool autoFireUpdateFlag = mnuAutoFireOption.Checked;

                TASMovieInput.Insert(ref FrameData, updated, updateFlag, autoFireUpdateFlag, frameIndex, totalFrames);                
                updateControlsAfterEdit();
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
            lvInput.VirtualListSize = 0;
            lvInput.VirtualListSource = FrameData;
            lvInput.VirtualListSize   = FrameData.Length;           
            lvInput.Refresh();            

            sbarFrameCount.Text = FrameData.Length.ToString();            
        }

    #endregion

    #region "Copy-Pasting"

        /// <summary>
        /// Show the buffer form (pass in the buffer arrayList and the buffer's MovieType)
        /// </summary>        
        private void mnuViewBuffer_Click(object sender, EventArgs e)
        {
            frmBuffer frm = new frmBuffer(ref FrameBuffer, MovieFormat, lvInput.Columns.Count - 1);
            frm.ShowDialog(this);
            frm.Dispose(ref FrameBuffer, ref BufferMovieFormat);
            
            // if the buffer array comes back empty, reset all the copy/paste options
            if (FrameBuffer == null)
                resetPasteControls();
            else
                enablePasteControls();
        }

        /// <summary>
        /// Reset the paste-specific controls to their default values
        /// </summary>
        private void resetPasteControls()
        {            
            sbarCopyBufferSize.Text     = "0";
            BufferMovieFormat           = MovieType.None;
            sbarCopyBufferType.Text     = Enum.GetName(typeof(MovieType), BufferMovieFormat);
            cmnuitemPasteFrames.Enabled = false;
            mnuPaste.Enabled            = false;            
        }

        /// <summary>
        /// Copy the selected frames to the buffer arraylist
        /// </summary>
        private void copyFrames()
        {
            // make sure something is selected
            if (lvInput.SelectedIndices.Count == 0) return;
                                    
            int frameIndex    = lvInput.SelectedIndices[0];            
            int totalFrames   = lvInput.SelectedIndices.Count;

            BufferMovieFormat = MovieFormat;
            FrameBuffer       = TASMovieInput.Copy(ref FrameData, frameIndex, totalFrames);            
            
            enablePasteControls();   
        }

        /// <summary>
        /// Set various controls when the copy buffer is filled
        /// </summary>
        private void enablePasteControls()
        {
            sbarCopyBufferType.Text     = Enum.GetName(typeof(MovieType), BufferMovieFormat);
            sbarCopyBufferSize.Text     = FrameBuffer.Length.ToString();
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
                        
            int frameIndex    = lvInput.SelectedIndices[0];
            int framePosition = Convert.ToInt32(lvInput.Items[frameIndex].Text);            

            // confirm that the paste should occur
            DialogResult confirmPaste = MessageBox.Show("Are you sure you want to paste " + FrameBuffer.Length + " frames after frame " + framePosition, "Confirm Paste", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirmPaste != DialogResult.OK) return;

            TASMovieInput.Paste(ref FrameData, ref FrameBuffer, frameIndex);
            
            int selected = lvInput.SelectedIndices[0];                        
            updateControlsAfterEdit(); 
            
            // keep focus on the last selected row            
            lvInput.Items[selected].Selected = true;
            lvInput.Focus();
            lvInput.EnsureVisible(selected);
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
