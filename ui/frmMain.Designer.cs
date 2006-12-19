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
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSplice = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewBuffer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCompareMovieInputRange = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoFireUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statMain = new System.Windows.Forms.StatusStrip();
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.txtFrameDataC4 = new System.Windows.Forms.TextBox();
            this.txtFrameDataC2 = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.txtFrameDataC3 = new System.Windows.Forms.TextBox();
            this.txtWorkingFrame = new System.Windows.Forms.TextBox();
            this.txtFrameDataC1 = new System.Windows.Forms.TextBox();
            this.txtJumpToFrame = new System.Windows.Forms.TextBox();
            this.btnGotoFrame = new System.Windows.Forms.Button();
            this.tvInfo = new System.Windows.Forms.TreeView();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.txtMovieFilename = new System.Windows.Forms.TextBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.btnFindInput = new System.Windows.Forms.Button();
            this.lvInput = new MovieSplicer.Components.TASListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbarFrameCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuMain.SuspendLayout();
            this.statMain.SuspendLayout();
            this.cmnu_lvInput.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuAbout,
            this.mnuEdit,
            this.optionsToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(917, 24);
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
            this.mnuOpen.Size = new System.Drawing.Size(151, 22);
            this.mnuOpen.Text = "&Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Enabled = false;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(151, 22);
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Enabled = false;
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(151, 22);
            this.mnuSaveAs.Text = "Save &As";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // mnuClose
            // 
            this.mnuClose.Enabled = false;
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.Size = new System.Drawing.Size(151, 22);
            this.mnuClose.Text = "&Close";
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.mnuQuit.Size = new System.Drawing.Size(151, 22);
            this.mnuQuit.Text = "&Quit";
            this.mnuQuit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(48, 20);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSplice,
            this.toolStripSeparator4,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuViewBuffer,
            this.toolStripSeparator2,
            this.mnuCompareMovieInputRange});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(37, 20);
            this.mnuEdit.Text = "&Edit";
            // 
            // mnuSplice
            // 
            this.mnuSplice.Name = "mnuSplice";
            this.mnuSplice.Size = new System.Drawing.Size(222, 22);
            this.mnuSplice.Text = "&Splice Movie";
            this.mnuSplice.Click += new System.EventHandler(this.mnuSplice_Click);
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
            this.autoFireUpdateToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // autoFireUpdateToolStripMenuItem
            // 
            this.autoFireUpdateToolStripMenuItem.CheckOnClick = true;
            this.autoFireUpdateToolStripMenuItem.Name = "autoFireUpdateToolStripMenuItem";
            this.autoFireUpdateToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.autoFireUpdateToolStripMenuItem.Text = "&Auto-Fire Update";
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
            this.statMain.Location = new System.Drawing.Point(0, 362);
            this.statMain.Name = "statMain";
            this.statMain.Size = new System.Drawing.Size(917, 22);
            this.statMain.SizingGrip = false;
            this.statMain.TabIndex = 8;
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
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label37);
            this.groupBox6.Controls.Add(this.label38);
            this.groupBox6.Controls.Add(this.txtFrameDataC4);
            this.groupBox6.Controls.Add(this.txtFrameDataC2);
            this.groupBox6.Controls.Add(this.btnUpdate);
            this.groupBox6.Controls.Add(this.label35);
            this.groupBox6.Controls.Add(this.label36);
            this.groupBox6.Controls.Add(this.label34);
            this.groupBox6.Controls.Add(this.txtFrameDataC3);
            this.groupBox6.Controls.Add(this.txtWorkingFrame);
            this.groupBox6.Controls.Add(this.txtFrameDataC1);
            this.groupBox6.Location = new System.Drawing.Point(427, 27);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(95, 330);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Editing";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 141);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(60, 13);
            this.label37.TabIndex = 5;
            this.label37.Text = "Controller 4";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 63);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(60, 13);
            this.label38.TabIndex = 6;
            this.label38.Text = "Controller 2";
            // 
            // txtFrameDataC4
            // 
            this.txtFrameDataC4.Enabled = false;
            this.txtFrameDataC4.Location = new System.Drawing.Point(6, 157);
            this.txtFrameDataC4.Name = "txtFrameDataC4";
            this.txtFrameDataC4.Size = new System.Drawing.Size(76, 20);
            this.txtFrameDataC4.TabIndex = 3;
            // 
            // txtFrameDataC2
            // 
            this.txtFrameDataC2.Enabled = false;
            this.txtFrameDataC2.Location = new System.Drawing.Point(6, 79);
            this.txtFrameDataC2.Name = "txtFrameDataC2";
            this.txtFrameDataC2.Size = new System.Drawing.Size(76, 20);
            this.txtFrameDataC2.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(9, 295);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(76, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(9, 253);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(36, 13);
            this.label35.TabIndex = 1;
            this.label35.Text = "Frame";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 102);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(60, 13);
            this.label36.TabIndex = 1;
            this.label36.Text = "Controller 3";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 24);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(60, 13);
            this.label34.TabIndex = 1;
            this.label34.Text = "Controller 1";
            // 
            // txtFrameDataC3
            // 
            this.txtFrameDataC3.Enabled = false;
            this.txtFrameDataC3.Location = new System.Drawing.Point(6, 118);
            this.txtFrameDataC3.Name = "txtFrameDataC3";
            this.txtFrameDataC3.Size = new System.Drawing.Size(76, 20);
            this.txtFrameDataC3.TabIndex = 2;
            // 
            // txtWorkingFrame
            // 
            this.txtWorkingFrame.Location = new System.Drawing.Point(9, 269);
            this.txtWorkingFrame.MaxLength = 10;
            this.txtWorkingFrame.Name = "txtWorkingFrame";
            this.txtWorkingFrame.ReadOnly = true;
            this.txtWorkingFrame.Size = new System.Drawing.Size(76, 20);
            this.txtWorkingFrame.TabIndex = 0;
            this.txtWorkingFrame.TabStop = false;
            // 
            // txtFrameDataC1
            // 
            this.txtFrameDataC1.Enabled = false;
            this.txtFrameDataC1.Location = new System.Drawing.Point(6, 40);
            this.txtFrameDataC1.Name = "txtFrameDataC1";
            this.txtFrameDataC1.Size = new System.Drawing.Size(76, 20);
            this.txtFrameDataC1.TabIndex = 0;
            // 
            // txtJumpToFrame
            // 
            this.txtJumpToFrame.Location = new System.Drawing.Point(110, 298);
            this.txtJumpToFrame.MaxLength = 15;
            this.txtJumpToFrame.Name = "txtJumpToFrame";
            this.txtJumpToFrame.Size = new System.Drawing.Size(163, 20);
            this.txtJumpToFrame.TabIndex = 25;
            // 
            // btnGotoFrame
            // 
            this.btnGotoFrame.Location = new System.Drawing.Point(281, 295);
            this.btnGotoFrame.Name = "btnGotoFrame";
            this.btnGotoFrame.Size = new System.Drawing.Size(98, 23);
            this.btnGotoFrame.TabIndex = 23;
            this.btnGotoFrame.Text = "&Goto Frame";
            this.btnGotoFrame.UseVisualStyleBackColor = true;
            this.btnGotoFrame.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // tvInfo
            // 
            this.tvInfo.BackColor = System.Drawing.SystemColors.Window;
            this.tvInfo.Font = new System.Drawing.Font("Courier New", 8F);
            this.tvInfo.FullRowSelect = true;
            this.tvInfo.Location = new System.Drawing.Point(7, 43);
            this.tvInfo.Name = "tvInfo";
            this.tvInfo.Size = new System.Drawing.Size(400, 275);
            this.tvInfo.TabIndex = 27;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.txtMovieFilename);
            this.groupBox11.Controls.Add(this.tvInfo);
            this.groupBox11.Location = new System.Drawing.Point(4, 27);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(417, 330);
            this.groupBox11.TabIndex = 28;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Movie Information";
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
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.btnFindInput);
            this.groupBox12.Controls.Add(this.lvInput);
            this.groupBox12.Controls.Add(this.btnGotoFrame);
            this.groupBox12.Controls.Add(this.txtJumpToFrame);
            this.groupBox12.Location = new System.Drawing.Point(528, 27);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(385, 330);
            this.groupBox12.TabIndex = 29;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Frame Data";
            // 
            // btnFindInput
            // 
            this.btnFindInput.Enabled = false;
            this.btnFindInput.Location = new System.Drawing.Point(6, 298);
            this.btnFindInput.Name = "btnFindInput";
            this.btnFindInput.Size = new System.Drawing.Size(98, 23);
            this.btnFindInput.TabIndex = 27;
            this.btnFindInput.Text = "&Find Input";
            this.btnFindInput.UseVisualStyleBackColor = true;
            // 
            // lvInput
            // 
            this.lvInput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvInput.ContextMenuStrip = this.cmnu_lvInput;
            this.lvInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvInput.FullRowSelect = true;
            this.lvInput.GridLines = true;
            this.lvInput.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvInput.Location = new System.Drawing.Point(8, 24);
            this.lvInput.Name = "lvInput";
            this.lvInput.Size = new System.Drawing.Size(371, 265);
            this.lvInput.TabIndex = 26;
            this.lvInput.UseCompatibleStateImageBehavior = false;
            this.lvInput.View = System.Windows.Forms.View.Details;
            this.lvInput.VirtualMode = true;
            this.lvInput.Leave += new System.EventHandler(this.lvInput_Leave);
            this.lvInput.Click += new System.EventHandler(this.lvInput_Clicked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Frame";
            this.columnHeader1.Width = 367;
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
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 384);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.statMain);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.groupBox6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TAS Movie Editor v0.8.1";
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.statMain.ResumeLayout(false);
            this.statMain.PerformLayout();
            this.cmnu_lvInput.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtFrameDataC1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox txtWorkingFrame;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtFrameDataC3;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtFrameDataC4;
        private System.Windows.Forms.TextBox txtFrameDataC2;
        private System.Windows.Forms.TextBox txtJumpToFrame;
        private System.Windows.Forms.Button btnGotoFrame;
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
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TextBox txtMovieFilename;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button btnFindInput;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoFireUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel sbarFrameCount;       
    }
}

