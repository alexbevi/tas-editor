namespace MovieSplicer.UI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUndoChange = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewBuffer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSplice = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCompareMovieInputRange = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMessageHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAutoFireOption = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditingPrompt = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.statMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbarFrameCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbarCopyBufferSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbarCopyBufferType = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmnu_lvInput = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuitemAddFrame = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuitemRemoveFrames = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuitemCopyFrames = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuitemPasteFrames = new System.Windows.Forms.ToolStripMenuItem();
            this.grpEditing = new System.Windows.Forms.GroupBox();
            this.btnFindInput = new System.Windows.Forms.Button();
            this.btnGotoFrame = new System.Windows.Forms.Button();
            this.txtJumpToFrame = new System.Windows.Forms.TextBox();
            this.chkFrameDataC4 = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.chkFrameDataC3 = new System.Windows.Forms.CheckBox();
            this.chkFrameDataC2 = new System.Windows.Forms.CheckBox();
            this.txtFrameDataC3 = new System.Windows.Forms.TextBox();
            this.chkFrameDataC1 = new System.Windows.Forms.CheckBox();
            this.txtFrameDataC2 = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.txtFrameDataC4 = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label34 = new System.Windows.Forms.Label();
            this.txtFrameDataC1 = new System.Windows.Forms.TextBox();
            this.tvInfo = new System.Windows.Forms.TreeView();
            this.grpMovieInfo = new System.Windows.Forms.GroupBox();
            this.txtMovieFilename = new System.Windows.Forms.TextBox();
            this.grpFrameData = new System.Windows.Forms.GroupBox();
            this.lvInput = new MovieSplicer.Components.TASListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.mnuMain.SuspendLayout();
            this.statMain.SuspendLayout();
            this.cmnu_lvInput.SuspendLayout();
            this.grpEditing.SuspendLayout();
            this.grpMovieInfo.SuspendLayout();
            this.grpFrameData.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.optionsToolStripMenuItem,
            this.mnuAbout,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(842, 24);
            this.mnuMain.TabIndex = 7;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuClose,
            this.toolStripSeparator1,
            this.mnuQuit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(35, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpen.Size = new System.Drawing.Size(163, 22);
            this.mnuOpen.Text = "&Open...";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Enabled = false;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(163, 22);
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Enabled = false;
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(163, 22);
            this.mnuSaveAs.Text = "Save &As...";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // mnuClose
            // 
            this.mnuClose.Enabled = false;
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.Size = new System.Drawing.Size(163, 22);
            this.mnuClose.Text = "&Close";
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.mnuQuit.Size = new System.Drawing.Size(163, 22);
            this.mnuQuit.Text = "&Quit";
            this.mnuQuit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndoChange,
            this.toolStripSeparator4,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuViewBuffer,
            this.toolStripSeparator2,
            this.mnuSplice,
            this.mnuCompareMovieInputRange});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(37, 20);
            this.mnuEdit.Text = "&Edit";
            // 
            // mnuUndoChange
            // 
            this.mnuUndoChange.Enabled = false;
            this.mnuUndoChange.Name = "mnuUndoChange";
            this.mnuUndoChange.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnuUndoChange.Size = new System.Drawing.Size(222, 22);
            this.mnuUndoChange.Text = "&Undo";
            this.mnuUndoChange.Click += new System.EventHandler(this.mnuUndoChange_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCopy.Size = new System.Drawing.Size(222, 22);
            this.mnuCopy.Text = "&Copy Selection";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Enabled = false;
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mnuPaste.Size = new System.Drawing.Size(222, 22);
            this.mnuPaste.Text = "&Paste Selection";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuViewBuffer
            // 
            this.mnuViewBuffer.Name = "mnuViewBuffer";
            this.mnuViewBuffer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.mnuViewBuffer.Size = new System.Drawing.Size(222, 22);
            this.mnuViewBuffer.Text = "&View Copy Buffer";
            this.mnuViewBuffer.Click += new System.EventHandler(this.mnuViewBuffer_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuSplice
            // 
            this.mnuSplice.Name = "mnuSplice";
            this.mnuSplice.Size = new System.Drawing.Size(222, 22);
            this.mnuSplice.Text = "&Splice Movie";
            this.mnuSplice.Click += new System.EventHandler(this.mnuSplice_Click);
            // 
            // mnuCompareMovieInputRange
            // 
            this.mnuCompareMovieInputRange.Enabled = false;
            this.mnuCompareMovieInputRange.Name = "mnuCompareMovieInputRange";
            this.mnuCompareMovieInputRange.Size = new System.Drawing.Size(222, 22);
            this.mnuCompareMovieInputRange.Text = "&Compare Movie Input Range";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMessageHistory,
            this.toolStripSeparator5,
            this.mnuAutoFireOption,
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem,
            this.mnuEditingPrompt});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // mnuMessageHistory
            // 
            this.mnuMessageHistory.Name = "mnuMessageHistory";
            this.mnuMessageHistory.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mnuMessageHistory.Size = new System.Drawing.Size(229, 22);
            this.mnuMessageHistory.Text = "View &Message History";
            this.mnuMessageHistory.Click += new System.EventHandler(this.mnuMessageHistory_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(226, 6);
            // 
            // mnuAutoFireOption
            // 
            this.mnuAutoFireOption.CheckOnClick = true;
            this.mnuAutoFireOption.Name = "mnuAutoFireOption";
            this.mnuAutoFireOption.Size = new System.Drawing.Size(229, 22);
            this.mnuAutoFireOption.Text = "&Auto-Fire Update";
            // 
            // pasteOverwritesFromCurrentPositionToolStripMenuItem
            // 
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem.CheckOnClick = true;
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem.Enabled = false;
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem.Name = "pasteOverwritesFromCurrentPositionToolStripMenuItem";
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.pasteOverwritesFromCurrentPositionToolStripMenuItem.Text = "Overwrite on Paste";
            // 
            // mnuEditingPrompt
            // 
            this.mnuEditingPrompt.CheckOnClick = true;
            this.mnuEditingPrompt.Name = "mnuEditingPrompt";
            this.mnuEditingPrompt.Size = new System.Drawing.Size(229, 22);
            this.mnuEditingPrompt.Text = "Confirm Editing Actions";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(48, 20);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(40, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // statMain
            // 
            this.statMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.sbarFrameCount,
            this.toolStripStatusLabel3,
            this.sbarCopyBufferSize,
            this.toolStripStatusLabel4,
            this.sbarCopyBufferType});
            this.statMain.Location = new System.Drawing.Point(0, 379);
            this.statMain.Name = "statMain";
            this.statMain.Size = new System.Drawing.Size(842, 22);
            this.statMain.SizingGrip = false;
            this.statMain.TabIndex = 8;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel1.Text = "Frame Count:";
            // 
            // sbarFrameCount
            // 
            this.sbarFrameCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.sbarFrameCount.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.sbarFrameCount.ForeColor = System.Drawing.Color.Blue;
            this.sbarFrameCount.Name = "sbarFrameCount";
            this.sbarFrameCount.Size = new System.Drawing.Size(17, 17);
            this.sbarFrameCount.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(91, 17);
            this.toolStripStatusLabel3.Text = "Copy Buffer Size:";
            // 
            // sbarCopyBufferSize
            // 
            this.sbarCopyBufferSize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.sbarCopyBufferSize.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.sbarCopyBufferSize.ForeColor = System.Drawing.Color.Blue;
            this.sbarCopyBufferSize.Name = "sbarCopyBufferSize";
            this.sbarCopyBufferSize.Size = new System.Drawing.Size(17, 17);
            this.sbarCopyBufferSize.Text = "0";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(96, 17);
            this.toolStripStatusLabel4.Text = "Copy Buffer Type:";
            // 
            // sbarCopyBufferType
            // 
            this.sbarCopyBufferType.ForeColor = System.Drawing.Color.Blue;
            this.sbarCopyBufferType.Name = "sbarCopyBufferType";
            this.sbarCopyBufferType.Size = new System.Drawing.Size(32, 17);
            this.sbarCopyBufferType.Text = "None";
            // 
            // cmnu_lvInput
            // 
            this.cmnu_lvInput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuitemAddFrame,
            this.cmnuitemRemoveFrames,
            this.toolStripSeparator3,
            this.cmnuitemCopyFrames,
            this.cmnuitemPasteFrames});
            this.cmnu_lvInput.Name = "cmnuInput";
            this.cmnu_lvInput.Size = new System.Drawing.Size(210, 98);
            // 
            // cmnuitemAddFrame
            // 
            this.cmnuitemAddFrame.Name = "cmnuitemAddFrame";
            this.cmnuitemAddFrame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.cmnuitemAddFrame.Size = new System.Drawing.Size(209, 22);
            this.cmnuitemAddFrame.Text = "&Add Frame(s)";
            this.cmnuitemAddFrame.Click += new System.EventHandler(this.cmnuitemAddFrame_Click);
            // 
            // cmnuitemRemoveFrames
            // 
            this.cmnuitemRemoveFrames.Name = "cmnuitemRemoveFrames";
            this.cmnuitemRemoveFrames.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.cmnuitemRemoveFrames.Size = new System.Drawing.Size(209, 22);
            this.cmnuitemRemoveFrames.Text = "&Remove Frame(s)";
            this.cmnuitemRemoveFrames.Click += new System.EventHandler(this.cmnuitemRemoveFrames_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(206, 6);
            // 
            // cmnuitemCopyFrames
            // 
            this.cmnuitemCopyFrames.Name = "cmnuitemCopyFrames";
            this.cmnuitemCopyFrames.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.cmnuitemCopyFrames.Size = new System.Drawing.Size(209, 22);
            this.cmnuitemCopyFrames.Text = "&Copy Frames";
            this.cmnuitemCopyFrames.Click += new System.EventHandler(this.cmnuitemCopyFrames_Click);
            // 
            // cmnuitemPasteFrames
            // 
            this.cmnuitemPasteFrames.Enabled = false;
            this.cmnuitemPasteFrames.Name = "cmnuitemPasteFrames";
            this.cmnuitemPasteFrames.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.cmnuitemPasteFrames.Size = new System.Drawing.Size(209, 22);
            this.cmnuitemPasteFrames.Text = "&Paste Frames";
            this.cmnuitemPasteFrames.Click += new System.EventHandler(this.cmnuitemPasteFrames_Click);
            // 
            // grpEditing
            // 
            this.grpEditing.Controls.Add(this.btnFindInput);
            this.grpEditing.Controls.Add(this.btnGotoFrame);
            this.grpEditing.Controls.Add(this.txtJumpToFrame);
            this.grpEditing.Controls.Add(this.chkFrameDataC4);
            this.grpEditing.Controls.Add(this.label36);
            this.grpEditing.Controls.Add(this.label38);
            this.grpEditing.Controls.Add(this.chkFrameDataC3);
            this.grpEditing.Controls.Add(this.chkFrameDataC2);
            this.grpEditing.Controls.Add(this.txtFrameDataC3);
            this.grpEditing.Controls.Add(this.chkFrameDataC1);
            this.grpEditing.Controls.Add(this.txtFrameDataC2);
            this.grpEditing.Controls.Add(this.label37);
            this.grpEditing.Controls.Add(this.txtFrameDataC4);
            this.grpEditing.Controls.Add(this.btnUpdate);
            this.grpEditing.Controls.Add(this.label34);
            this.grpEditing.Controls.Add(this.txtFrameDataC1);
            this.grpEditing.Location = new System.Drawing.Point(427, 276);
            this.grpEditing.Name = "grpEditing";
            this.grpEditing.Size = new System.Drawing.Size(410, 98);
            this.grpEditing.TabIndex = 9;
            this.grpEditing.TabStop = false;
            this.grpEditing.Text = "Editing";
            // 
            // btnFindInput
            // 
            this.btnFindInput.Location = new System.Drawing.Point(6, 66);
            this.btnFindInput.Name = "btnFindInput";
            this.btnFindInput.Size = new System.Drawing.Size(81, 23);
            this.btnFindInput.TabIndex = 30;
            this.btnFindInput.Text = "&Find Input";
            this.btnFindInput.UseVisualStyleBackColor = true;
            this.btnFindInput.Click += new System.EventHandler(this.btnFindInput_Click);
            // 
            // btnGotoFrame
            // 
            this.btnGotoFrame.Location = new System.Drawing.Point(261, 63);
            this.btnGotoFrame.Name = "btnGotoFrame";
            this.btnGotoFrame.Size = new System.Drawing.Size(79, 23);
            this.btnGotoFrame.TabIndex = 28;
            this.btnGotoFrame.Text = "&Goto Frame";
            this.btnGotoFrame.UseVisualStyleBackColor = true;
            this.btnGotoFrame.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtJumpToFrame
            // 
            this.txtJumpToFrame.Location = new System.Drawing.Point(91, 66);
            this.txtJumpToFrame.MaxLength = 15;
            this.txtJumpToFrame.Name = "txtJumpToFrame";
            this.txtJumpToFrame.Size = new System.Drawing.Size(164, 20);
            this.txtJumpToFrame.TabIndex = 29;
            // 
            // chkFrameDataC4
            // 
            this.chkFrameDataC4.AutoSize = true;
            this.chkFrameDataC4.Enabled = false;
            this.chkFrameDataC4.Location = new System.Drawing.Point(327, 21);
            this.chkFrameDataC4.Name = "chkFrameDataC4";
            this.chkFrameDataC4.Size = new System.Drawing.Size(15, 14);
            this.chkFrameDataC4.TabIndex = 7;
            this.chkFrameDataC4.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(176, 21);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(60, 13);
            this.label36.TabIndex = 1;
            this.label36.Text = "Controller 3";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(91, 21);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(60, 13);
            this.label38.TabIndex = 6;
            this.label38.Text = "Controller 2";
            // 
            // chkFrameDataC3
            // 
            this.chkFrameDataC3.AutoSize = true;
            this.chkFrameDataC3.Enabled = false;
            this.chkFrameDataC3.Location = new System.Drawing.Point(242, 21);
            this.chkFrameDataC3.Name = "chkFrameDataC3";
            this.chkFrameDataC3.Size = new System.Drawing.Size(15, 14);
            this.chkFrameDataC3.TabIndex = 7;
            this.chkFrameDataC3.UseVisualStyleBackColor = true;
            // 
            // chkFrameDataC2
            // 
            this.chkFrameDataC2.AutoSize = true;
            this.chkFrameDataC2.Enabled = false;
            this.chkFrameDataC2.Location = new System.Drawing.Point(157, 21);
            this.chkFrameDataC2.Name = "chkFrameDataC2";
            this.chkFrameDataC2.Size = new System.Drawing.Size(15, 14);
            this.chkFrameDataC2.TabIndex = 7;
            this.chkFrameDataC2.UseVisualStyleBackColor = true;
            // 
            // txtFrameDataC3
            // 
            this.txtFrameDataC3.Enabled = false;
            this.txtFrameDataC3.Location = new System.Drawing.Point(176, 37);
            this.txtFrameDataC3.Name = "txtFrameDataC3";
            this.txtFrameDataC3.Size = new System.Drawing.Size(79, 20);
            this.txtFrameDataC3.TabIndex = 2;
            this.txtFrameDataC3.EnabledChanged += new System.EventHandler(this.txtFrameDataC3_EnabledChanged);
            // 
            // chkFrameDataC1
            // 
            this.chkFrameDataC1.AutoSize = true;
            this.chkFrameDataC1.Enabled = false;
            this.chkFrameDataC1.Location = new System.Drawing.Point(72, 21);
            this.chkFrameDataC1.Name = "chkFrameDataC1";
            this.chkFrameDataC1.Size = new System.Drawing.Size(15, 14);
            this.chkFrameDataC1.TabIndex = 7;
            this.chkFrameDataC1.UseVisualStyleBackColor = true;
            // 
            // txtFrameDataC2
            // 
            this.txtFrameDataC2.Enabled = false;
            this.txtFrameDataC2.Location = new System.Drawing.Point(91, 37);
            this.txtFrameDataC2.Name = "txtFrameDataC2";
            this.txtFrameDataC2.Size = new System.Drawing.Size(79, 20);
            this.txtFrameDataC2.TabIndex = 1;
            this.txtFrameDataC2.EnabledChanged += new System.EventHandler(this.txtFrameDataC2_EnabledChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(261, 21);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(60, 13);
            this.label37.TabIndex = 5;
            this.label37.Text = "Controller 4";
            // 
            // txtFrameDataC4
            // 
            this.txtFrameDataC4.Enabled = false;
            this.txtFrameDataC4.Location = new System.Drawing.Point(261, 37);
            this.txtFrameDataC4.Name = "txtFrameDataC4";
            this.txtFrameDataC4.Size = new System.Drawing.Size(79, 20);
            this.txtFrameDataC4.TabIndex = 3;
            this.txtFrameDataC4.EnabledChanged += new System.EventHandler(this.txtFrameDataC4_EnabledChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(346, 35);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(59, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 21);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(60, 13);
            this.label34.TabIndex = 1;
            this.label34.Text = "Controller 1";
            // 
            // txtFrameDataC1
            // 
            this.txtFrameDataC1.Enabled = false;
            this.txtFrameDataC1.Location = new System.Drawing.Point(6, 37);
            this.txtFrameDataC1.Name = "txtFrameDataC1";
            this.txtFrameDataC1.Size = new System.Drawing.Size(79, 20);
            this.txtFrameDataC1.TabIndex = 0;
            this.txtFrameDataC1.EnabledChanged += new System.EventHandler(this.txtFrameDataC1_EnabledChanged);
            // 
            // tvInfo
            // 
            this.tvInfo.BackColor = System.Drawing.SystemColors.Window;
            this.tvInfo.Font = new System.Drawing.Font("Courier New", 8F);
            this.tvInfo.FullRowSelect = true;
            this.tvInfo.Location = new System.Drawing.Point(7, 43);
            this.tvInfo.Name = "tvInfo";
            this.tvInfo.Size = new System.Drawing.Size(400, 295);
            this.tvInfo.TabIndex = 27;
            // 
            // grpMovieInfo
            // 
            this.grpMovieInfo.Controls.Add(this.txtMovieFilename);
            this.grpMovieInfo.Controls.Add(this.tvInfo);
            this.grpMovieInfo.Location = new System.Drawing.Point(4, 27);
            this.grpMovieInfo.Name = "grpMovieInfo";
            this.grpMovieInfo.Size = new System.Drawing.Size(417, 347);
            this.grpMovieInfo.TabIndex = 28;
            this.grpMovieInfo.TabStop = false;
            this.grpMovieInfo.Text = "Movie Information";
            // 
            // txtMovieFilename
            // 
            this.txtMovieFilename.Location = new System.Drawing.Point(8, 17);
            this.txtMovieFilename.Name = "txtMovieFilename";
            this.txtMovieFilename.ReadOnly = true;
            this.txtMovieFilename.Size = new System.Drawing.Size(399, 20);
            this.txtMovieFilename.TabIndex = 29;
            this.txtMovieFilename.TabStop = false;
            // 
            // grpFrameData
            // 
            this.grpFrameData.Controls.Add(this.lvInput);
            this.grpFrameData.Location = new System.Drawing.Point(427, 27);
            this.grpFrameData.Name = "grpFrameData";
            this.grpFrameData.Size = new System.Drawing.Size(410, 243);
            this.grpFrameData.TabIndex = 29;
            this.grpFrameData.TabStop = false;
            this.grpFrameData.Text = "Frame Data";
            // 
            // lvInput
            // 
            this.lvInput.AllowDrop = true;
            this.lvInput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvInput.ContextMenuStrip = this.cmnu_lvInput;
            this.lvInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvInput.FullRowSelect = true;
            this.lvInput.GridLines = true;
            this.lvInput.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvInput.HideSelection = false;
            this.lvInput.Location = new System.Drawing.Point(8, 17);
            this.lvInput.Name = "lvInput";
            this.lvInput.Size = new System.Drawing.Size(395, 220);
            this.lvInput.TabIndex = 26;
            this.lvInput.UseCompatibleStateImageBehavior = false;
            this.lvInput.View = System.Windows.Forms.View.Details;
            this.lvInput.VirtualMode = true;
            this.lvInput.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvInput_DragEnter);
            this.lvInput.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvInput_DragDrop);
            this.lvInput.Click += new System.EventHandler(this.lvInput_Clicked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Frame";
            this.columnHeader1.Width = 391;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 401);
            this.Controls.Add(this.statMain);
            this.Controls.Add(this.grpMovieInfo);
            this.Controls.Add(this.grpFrameData);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.grpEditing);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.statMain.ResumeLayout(false);
            this.statMain.PerformLayout();
            this.cmnu_lvInput.ResumeLayout(false);
            this.grpEditing.ResumeLayout(false);
            this.grpEditing.PerformLayout();
            this.grpMovieInfo.ResumeLayout(false);
            this.grpMovieInfo.PerformLayout();
            this.grpFrameData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuCompareMovieInputRange;
        private System.Windows.Forms.ToolStripMenuItem mnuSplice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.StatusStrip statMain;
        private System.Windows.Forms.ContextMenuStrip cmnu_lvInput;
        private System.Windows.Forms.ToolStripMenuItem cmnuitemAddFrame;
        private System.Windows.Forms.ToolStripMenuItem cmnuitemRemoveFrames;
        private System.Windows.Forms.GroupBox grpEditing;
        private System.Windows.Forms.TextBox txtFrameDataC1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtFrameDataC3;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtFrameDataC4;
        private System.Windows.Forms.TextBox txtFrameDataC2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel sbarCopyBufferSize;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel sbarCopyBufferType;
        private System.Windows.Forms.ToolStripMenuItem cmnuitemCopyFrames;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmnuitemPasteFrames;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuViewBuffer;        
        private MovieSplicer.Components.TASListView lvInput;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TreeView tvInfo;
        private System.Windows.Forms.GroupBox grpMovieInfo;
        private System.Windows.Forms.TextBox txtMovieFilename;
        private System.Windows.Forms.GroupBox grpFrameData;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuAutoFireOption;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel sbarFrameCount;
        private System.Windows.Forms.CheckBox chkFrameDataC1;
        private System.Windows.Forms.CheckBox chkFrameDataC4;
        private System.Windows.Forms.CheckBox chkFrameDataC3;
        private System.Windows.Forms.CheckBox chkFrameDataC2;
        private System.Windows.Forms.ToolStripMenuItem pasteOverwritesFromCurrentPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuMessageHistory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnuEditingPrompt;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuUndoChange;
        private System.Windows.Forms.Button btnFindInput;
        private System.Windows.Forms.Button btnGotoFrame;
        private System.Windows.Forms.TextBox txtJumpToFrame;       
    }
}

