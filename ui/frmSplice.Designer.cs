namespace MovieSplicer.UI
{
    partial class frmSplice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplice));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkStartOfMovie = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSourceFrameCount = new System.Windows.Forms.TextBox();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.chkEndOfMovie = new System.Windows.Forms.CheckBox();
            this.txtCopyToFrame = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCopyFromFrame = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourceFileName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTargetFrameResumePostion = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSplice = new System.Windows.Forms.Button();
            this.btnLoadTarget = new System.Windows.Forms.Button();
            this.txtTargetFrameCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTargetFrameInsertPosition = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTargetFileName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkStartOfMovie);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSourceFrameCount);
            this.groupBox1.Controls.Add(this.btnLoadSource);
            this.groupBox1.Controls.Add(this.chkEndOfMovie);
            this.groupBox1.Controls.Add(this.txtCopyToFrame);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCopyFromFrame);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSourceFileName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Movie";
            // 
            // chkStartOfMovie
            // 
            this.chkStartOfMovie.AutoSize = true;
            this.chkStartOfMovie.Location = new System.Drawing.Point(181, 76);
            this.chkStartOfMovie.Name = "chkStartOfMovie";
            this.chkStartOfMovie.Size = new System.Drawing.Size(92, 17);
            this.chkStartOfMovie.TabIndex = 1;
            this.chkStartOfMovie.Text = "Start of Movie";
            this.chkStartOfMovie.UseVisualStyleBackColor = true;
            this.chkStartOfMovie.CheckedChanged += new System.EventHandler(this.chkStartOfMovie_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Frame Count";
            // 
            // txtSourceFrameCount
            // 
            this.txtSourceFrameCount.Location = new System.Drawing.Point(74, 45);
            this.txtSourceFrameCount.Name = "txtSourceFrameCount";
            this.txtSourceFrameCount.ReadOnly = true;
            this.txtSourceFrameCount.Size = new System.Drawing.Size(100, 20);
            this.txtSourceFrameCount.TabIndex = 9;
            this.txtSourceFrameCount.TabStop = false;
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Location = new System.Drawing.Point(195, 43);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(75, 23);
            this.btnLoadSource.TabIndex = 0;
            this.btnLoadSource.Text = "Load";
            this.btnLoadSource.UseVisualStyleBackColor = true;
            this.btnLoadSource.Click += new System.EventHandler(this.btnLoadSource_Click);
            // 
            // chkEndOfMovie
            // 
            this.chkEndOfMovie.AutoSize = true;
            this.chkEndOfMovie.Location = new System.Drawing.Point(181, 103);
            this.chkEndOfMovie.Name = "chkEndOfMovie";
            this.chkEndOfMovie.Size = new System.Drawing.Size(89, 17);
            this.chkEndOfMovie.TabIndex = 2;
            this.chkEndOfMovie.Text = "End of Movie";
            this.chkEndOfMovie.UseVisualStyleBackColor = true;
            this.chkEndOfMovie.CheckedChanged += new System.EventHandler(this.chkEndOfMovie_CheckedChanged);
            // 
            // txtCopyToFrame
            // 
            this.txtCopyToFrame.Location = new System.Drawing.Point(74, 100);
            this.txtCopyToFrame.Name = "txtCopyToFrame";
            this.txtCopyToFrame.Size = new System.Drawing.Size(100, 20);
            this.txtCopyToFrame.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "To Frame";
            // 
            // txtCopyFromFrame
            // 
            this.txtCopyFromFrame.Location = new System.Drawing.Point(74, 74);
            this.txtCopyFromFrame.Name = "txtCopyFromFrame";
            this.txtCopyFromFrame.Size = new System.Drawing.Size(100, 20);
            this.txtCopyFromFrame.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From Frame";
            // 
            // txtSourceFileName
            // 
            this.txtSourceFileName.Location = new System.Drawing.Point(6, 19);
            this.txtSourceFileName.Name = "txtSourceFileName";
            this.txtSourceFileName.ReadOnly = true;
            this.txtSourceFileName.Size = new System.Drawing.Size(264, 20);
            this.txtSourceFileName.TabIndex = 8;
            this.txtSourceFileName.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTargetFrameResumePostion);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnSplice);
            this.groupBox2.Controls.Add(this.btnLoadTarget);
            this.groupBox2.Controls.Add(this.txtTargetFrameCount);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtTargetFrameInsertPosition);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtTargetFileName);
            this.groupBox2.Location = new System.Drawing.Point(12, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 125);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target Movie";
            // 
            // txtTargetFrameResumePostion
            // 
            this.txtTargetFrameResumePostion.Location = new System.Drawing.Point(74, 97);
            this.txtTargetFrameResumePostion.Name = "txtTargetFrameResumePostion";
            this.txtTargetFrameResumePostion.Size = new System.Drawing.Size(100, 20);
            this.txtTargetFrameResumePostion.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Resume at";
            // 
            // btnSplice
            // 
            this.btnSplice.Location = new System.Drawing.Point(195, 94);
            this.btnSplice.Name = "btnSplice";
            this.btnSplice.Size = new System.Drawing.Size(75, 23);
            this.btnSplice.TabIndex = 6;
            this.btnSplice.Text = "&Splice";
            this.btnSplice.UseVisualStyleBackColor = true;
            this.btnSplice.Click += new System.EventHandler(this.btnSplice_Click);
            // 
            // btnLoadTarget
            // 
            this.btnLoadTarget.Location = new System.Drawing.Point(195, 67);
            this.btnLoadTarget.Name = "btnLoadTarget";
            this.btnLoadTarget.Size = new System.Drawing.Size(75, 23);
            this.btnLoadTarget.TabIndex = 5;
            this.btnLoadTarget.Text = "Load";
            this.btnLoadTarget.UseVisualStyleBackColor = true;
            this.btnLoadTarget.Click += new System.EventHandler(this.btnLoadTarget_Click);
            // 
            // txtTargetFrameCount
            // 
            this.txtTargetFrameCount.Location = new System.Drawing.Point(74, 45);
            this.txtTargetFrameCount.Name = "txtTargetFrameCount";
            this.txtTargetFrameCount.ReadOnly = true;
            this.txtTargetFrameCount.Size = new System.Drawing.Size(100, 20);
            this.txtTargetFrameCount.TabIndex = 11;
            this.txtTargetFrameCount.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Frame Count";
            // 
            // txtTargetFrameInsertPosition
            // 
            this.txtTargetFrameInsertPosition.Location = new System.Drawing.Point(74, 71);
            this.txtTargetFrameInsertPosition.Name = "txtTargetFrameInsertPosition";
            this.txtTargetFrameInsertPosition.Size = new System.Drawing.Size(100, 20);
            this.txtTargetFrameInsertPosition.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Insert at";
            // 
            // txtTargetFileName
            // 
            this.txtTargetFileName.Location = new System.Drawing.Point(6, 19);
            this.txtTargetFileName.Name = "txtTargetFileName";
            this.txtTargetFileName.ReadOnly = true;
            this.txtTargetFileName.Size = new System.Drawing.Size(264, 20);
            this.txtTargetFileName.TabIndex = 10;
            // 
            // frmSplice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 279);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplice";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Spliced Movie File";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCopyFromFrame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourceFileName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTargetFileName;
        private System.Windows.Forms.CheckBox chkEndOfMovie;
        private System.Windows.Forms.TextBox txtCopyToFrame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoadTarget;
        private System.Windows.Forms.TextBox txtTargetFrameCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSplice;
        private System.Windows.Forms.TextBox txtTargetFrameInsertPosition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSourceFrameCount;
        private System.Windows.Forms.TextBox txtTargetFrameResumePostion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkStartOfMovie;

    }
}