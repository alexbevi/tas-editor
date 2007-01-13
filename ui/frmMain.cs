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
 * Revision: 0-x-x
 * 
 * Conventions:
 * - Class Level Variables, Methods and Properties are denoted with each word appearing with the 
 *   first letter capitalized: ie. FrameData, Save()
 * - private Class Variables and methods are denoted with the first word being lowercase, then the
 *   subsequent words having their first letter capitalized: ie. openDlg, parseControllerData()
 **************************************************************************************************/
namespace MovieSplicer.UI
{
    public partial class frmMain : TASForm
    {
        // minimum form dimensions
        const int BASE_WIDTH  = 850;
        const int BASE_HEIGHT = 435;

        MovieType MovieFormat       = MovieType.None;
        MovieType BufferMovieFormat = MovieType.None;

        private TASMovie        Movie;
        private TASMovieInput[] FrameData;
        private TASMovieInput[] FrameBuffer;
        private UndoBuffer[]    UndoHistory;
        
        // will contain the message history for this session
        frmMessages Msg = new frmMessages();
        frmEditing  Editor;

        public frmMain()
        {
            InitializeComponent();
            initializeControls();            
        }                

        /// <summary>
        /// General form initialization
        /// </summary>
        private void initializeControls()
        {
            this.Text = APP_TITLE + " v" + VERSION;
            this.MinimumSize = new Size(BASE_WIDTH, BASE_HEIGHT);            
        }

        /// <summary>
        /// Resize/reposition the controls when the main form size changes
        /// </summary>
        private void resizeControls()
        {
            int diffW = this.Width - BASE_WIDTH;
            int diffH = this.Height - BASE_HEIGHT;
            
            int screenW = Screen.PrimaryScreen.WorkingArea.Width;
            int screenH = Screen.PrimaryScreen.WorkingArea.Height;
            
            grpMovieInfo.Size = new Size(417, 347 + diffH);
            tvInfo.Size = new Size(400, 283 + diffH);           
            
            grpFrameData.Size = new Size(410 + diffW, 347 + diffH);
            lvInput.Size = new Size(395 + diffW, 321 + diffH);
        }

        /// <summary>
        /// Form sizing handler
        /// </summary>
        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            resizeControls();
            lvInput.Refresh();
        }   


    #region Frame Data Handlers

        /// <summary>
        /// Populate edit controls with frame data        
        /// </summary>
        private void lvInput_Clicked(object sender, EventArgs e)
        {
            if (lvInput.SelectedIndices.Count > 0 && MovieFormat != MovieType.None)
            {
                ListViewItem lvi = lvInput.Items[lvInput.SelectedIndices[0]];
                Editor.PopulateEditFields(lvi);                
            }
        }                
     
        /// <summary>
        /// Change the drag cursor
        /// </summary>        
        private void lvInput_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Load the move that's dropped
        /// </summary>        
        private void lvInput_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // we can only load 1 file at a time, so take the first in the collection
            // and ignore the rest
            loadMovie(files[0]);
        }
    
    #endregion

   
    #region Menu Actions

        /// <summary>
        /// Loads a TAS into the editor
        /// </summary>        
        private void loadMovie(string filename)
        {            
            resetApplication();
            
            // initialize the editor
            Editor = new frmEditing();
            mnuEditing.Enabled = true;

            // copy the clean frame data to the undo buffer as the first item
            mnuUndoChange.Enabled = true;
            UndoHistory = new UndoBuffer[0];

            // make sure the file isn't locked before we do anything else
            try { System.IO.File.OpenRead(filename); }
            catch
            {
                MessageBox.Show(filename + " cannot be accessed at the moment", "File Possibly Locked");
                return;
            }

            MovieFormat = IsValid(filename);            
            ResourceManager rm = new ResourceManager("MovieSplicer.Properties.Resources", GetType().Assembly);
            
            switch (MovieFormat)
            {                
                case MovieType.SMV:
                    Movie = new SNES9x(filename);
                    Methods.PopulateMovieInfo.SMV(ref tvInfo, ref Movie);
                    pbFormat.Image = ((System.Drawing.Icon)(rm.GetObject("icon_smv"))).ToBitmap();
                    break;
                case MovieType.FCM:
                    Movie = new FCEU(filename);
                    Methods.PopulateMovieInfo.FCM(ref tvInfo, ref Movie);
                    pbFormat.Image = ((System.Drawing.Icon)(rm.GetObject("icon_fcm"))).ToBitmap();
                    break;
                case MovieType.GMV:
                    Movie = new Gens(filename);
                    Methods.PopulateMovieInfo.GMV(ref tvInfo, ref Movie);
                    pbFormat.Image = ((System.Drawing.Icon)(rm.GetObject("icon_gmv"))).ToBitmap();
                    break;
                case MovieType.FMV:
                    Movie = new Famtasia(filename);
                    Methods.PopulateMovieInfo.FMV(ref tvInfo, ref Movie);
                    pbFormat.Image = ((System.Drawing.Icon)(rm.GetObject("icon_fmv"))).ToBitmap();
                    break;
                case MovieType.VBM:
                    Movie = new VisualBoyAdvance(filename);
                    Methods.PopulateMovieInfo.VBM(ref tvInfo, ref Movie);
                    pbFormat.Image = ((System.Drawing.Icon)(rm.GetObject("icon_vbm"))).ToBitmap();
                    break;
                case MovieType.M64:
                    Movie = new Mupen64(filename);
                    Methods.PopulateMovieInfo.M64(ref tvInfo, ref Movie);
                    pbFormat.Image = ((System.Drawing.Icon)(rm.GetObject("icon_m64"))).ToBitmap();
                    break;
                case MovieType.None:
                    resetApplication();
                    return;
            }

            // destroy the resource manager instance
            rm = null;

            // assign the shared input collection to the current movie's            
            FrameData = Movie.Input.FrameData;

            // set the number of controller columns
            lvInput.SetColumns(Movie.Input.ControllerCount);            
            
            // initialize editing fields            
            bool[] activeControllers = { false, false, false, false };
            for (int i = 0; i < Movie.Input.ControllerCount; i++)
                activeControllers[i] = Movie.Input.Controllers[i];            
            Editor.ToggleInputBoxes(activeControllers); 

            // trim the filename and throw it into a text field
            txtMovieFilename.Text = FilenameFromPath(filename);

            // enable grayed menu options
            mnuSave.Enabled   = true;
            mnuSaveAs.Enabled = true;
            mnuClose.Enabled  = true;

            // populate the virtual listview                
            lvInput.VirtualListSource = FrameData;
            lvInput.VirtualListSize   = FrameData.Length;

            // add frame count to statusbar
            sbarFrameCount.Text = FrameData.Length.ToString();

            Editor.LoadSharedObjects(ref lvInput, ref FrameData, ref UndoHistory, ref Msg);            
            Msg.AddMsg("Successfully loaded " + FilenameFromPath(filename));                       
        }

        /// <summary>
        /// Reset all objects and controls to their default state
        /// </summary>
        private void resetApplication()
        {
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

            // clear the movie treeview
            tvInfo.Nodes.Clear();

            // nullify the input data reference
            FrameData   = null;
            MovieFormat = MovieType.None;

            // reset the editor            
            Editor = null;
            mnuEditing.Enabled = false;

            // clear the undo history
            UndoHistory = null;
            mnuUndoChange.Enabled = false;

            // clear the icon
            pbFormat.Image = null;
            
        }

        /// <summary>
        /// Show an OpenFileDialog to allow a file to be selected and loaded
        /// </summary>        
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            openDlg = new OpenFileDialog();
            openDlg.Filter = TAS_FILTER;
            openDlg.ShowDialog();

            if (openDlg.FileName.Length > 0)
                loadMovie(openDlg.FileName);
            
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

            Movie.Save("", ref FrameData);
            
            MessageBox.Show(txtMovieFilename.Text + " written successfully", " Save");
        }

        /// <summary>
        /// Save the current movie out to a new file
        /// </summary>        
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (MovieFormat != MovieType.None)
            {                
                //saveDlg = new SaveFileDialog();

                //// set the save dialog's file filter type according to the current format
                //if (MovieFormat == MovieType.FCM) saveDlg.Filter = FCM_FILTER;
                //if (MovieFormat == MovieType.FMV) saveDlg.Filter = FMV_FILTER;
                //if (MovieFormat == MovieType.GMV) saveDlg.Filter = GMV_FILTER;
                //if (MovieFormat == MovieType.SMV) saveDlg.Filter = SMV_FILTER;
                //if (MovieFormat == MovieType.VBM) saveDlg.Filter = VBM_FILTER;

                //saveDlg.FileName = "new-" + txtMovieFilename.Text;
                //DialogResult save = saveDlg.ShowDialog();

                //if (saveDlg.FileName.Length > 0 && save != DialogResult.Cancel)
                //{
                //    Movie.Save(saveDlg.FileName, ref FrameData);                
                //    MessageBox.Show(saveDlg.FileName + " written successfully", " Save As");
                //}  

                frmSaveAs frm = new frmSaveAs(ref Movie, ref FrameData, MovieFormat);
                frm.ShowDialog();
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
        /// Show the message history window
        /// </summary>        
        private void mnuMessageHistory_Click(object sender, EventArgs e)
        {
            Msg.ShowDialog(this);
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

        /// <summary>
        /// Show the splicing form
        /// </summary>        
        private void mnuSplice_Click(object sender, EventArgs e)
        {
            frmSplice frm = new frmSplice();
            frm.ShowDialog();
        }

        /// <summary>
        ///  Display the editor windows
        /// </summary>        
        private void mnuEditing_Click(object sender, EventArgs e)
        {
            if (!Editor.Visible)
                Editor.Show(this);
            else
                Editor.Focus();
        }

    #endregion   

   
    #region Editing
               
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
            if (lvInput.SelectedIndices.Count > 1 && mnuEditingPrompt.Checked)
            {
                DialogResult confirmAdd = MessageBox.Show("Are you sure you want to insert " + totalFrames + " frames after frame " + framePosition, "Confirm Multiple Frame Insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmAdd != DialogResult.OK) return;
            }

            UndoBuffer.Add(ref UndoHistory, ref FrameData);
            TASMovieInput.Insert(ref FrameData, framePosition, totalFrames);

            updateControlsAfterEdit();
            Msg.AddMsg("Added " + totalFrames + " frame(s) after position " + framePosition);
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
            if (lvInput.SelectedIndices.Count > 1 && mnuEditingPrompt.Checked)
            {
                DialogResult confirmDelete = MessageBox.Show("Are you sure you want to remove the selected " + totalFrames + " frames", "Confirm Multiple Frame Removal", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmDelete != DialogResult.OK) return;
            }

            UndoBuffer.Add(ref UndoHistory, ref FrameData);
            TASMovieInput.Remove(ref FrameData, frameIndex, totalFrames);

            // ensures that the virtual list doesn't try to access an element that no longer exists
            // after a block of frames is deleted
            int selected = lvInput.SelectedIndices[0];
            if (selected <= FrameData.Length)
            {
                // if we've removed all frames up to this point, the index == selected, so decrement
                if (selected == FrameData.Length) selected--;

                lvInput.Items[selected].Selected = true;
                lvInput.Focus();
                lvInput.EnsureVisible(selected);
            }

            updateControlsAfterEdit();
            Msg.AddMsg("Removed " + totalFrames + " frame(s) after frame " + framePosition);
        }

        /// <summary>
        /// Update the listview virtualListSize and the frame count in the statusbar
        /// </summary>
        private void updateControlsAfterEdit()
        {
            lvInput.VirtualListSource = FrameData;
            lvInput.VirtualListSize   = FrameData.Length;
            lvInput.Refresh();

            sbarFrameCount.Text = FrameData.Length.ToString();
        }

    #endregion

    
    #region Copy-Pasting

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
            if (mnuEditingPrompt.Checked)
            {
                DialogResult confirmPaste = MessageBox.Show("Are you sure you want to paste " + FrameBuffer.Length + " frames after frame " + framePosition, "Confirm Paste", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmPaste != DialogResult.OK) return;
            }

            UndoBuffer.Add(ref UndoHistory, ref FrameData);  
            TASMovieInput.Paste(ref FrameData, ref FrameBuffer, frameIndex + 1);                        
            updateControlsAfterEdit();
            
            Msg.AddMsg("Pasted " + FrameBuffer.Length + " frame(s) after frame " + framePosition);
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

        /// <summary>
        /// Undo a change
        /// </summary>        
        private void mnuUndoChange_Click(object sender, EventArgs e)
        {
            if (UndoHistory.Length > 0)
            {
                FrameData = UndoHistory[UndoHistory.Length - 1].Changes;
                UndoBuffer.Undo(ref UndoHistory);                
                
                Msg.AddMsg("Undid last change");
                updateControlsAfterEdit();
            }        
        }

    #endregion                                                              
     
    }
}
