namespace MovieSplicer.UI
{
    partial class frmCompare
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvOutput = new MovieSplicer.Components.TASCompareListView();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.btnLoadTarget = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSourceFrames = new System.Windows.Forms.TextBox();
            this.txtTargetFrames = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvOutput);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 359);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // lvOutput
            // 
            this.lvOutput.FullRowSelect = true;
            this.lvOutput.GridLines = true;
            this.lvOutput.Location = new System.Drawing.Point(6, 19);
            this.lvOutput.MultiSelect = false;
            this.lvOutput.Name = "lvOutput";
            this.lvOutput.Size = new System.Drawing.Size(764, 334);
            this.lvOutput.TabIndex = 0;
            this.lvOutput.UseCompatibleStateImageBehavior = false;
            this.lvOutput.View = System.Windows.Forms.View.Details;
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Location = new System.Drawing.Point(12, 377);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(101, 23);
            this.btnLoadSource.TabIndex = 32;
            this.btnLoadSource.Text = "Load &Source";
            this.btnLoadSource.UseVisualStyleBackColor = true;
            this.btnLoadSource.Click += new System.EventHandler(this.btnLoadSource_Click);
            // 
            // btnLoadTarget
            // 
            this.btnLoadTarget.Location = new System.Drawing.Point(12, 406);
            this.btnLoadTarget.Name = "btnLoadTarget";
            this.btnLoadTarget.Size = new System.Drawing.Size(101, 23);
            this.btnLoadTarget.TabIndex = 32;
            this.btnLoadTarget.Text = "Load &Target";
            this.btnLoadTarget.UseVisualStyleBackColor = true;
            this.btnLoadTarget.Click += new System.EventHandler(this.btnLoadTarget_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(687, 406);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(101, 23);
            this.btnClear.TabIndex = 33;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(119, 380);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(428, 20);
            this.txtSource.TabIndex = 34;
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(119, 406);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(428, 20);
            this.txtTarget.TabIndex = 35;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(687, 378);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(101, 23);
            this.btnProcess.TabIndex = 36;
            this.btnProcess.Text = "&Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(553, 383);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Frames";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(553, 411);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Frames";
            // 
            // txtSourceFrames
            // 
            this.txtSourceFrames.Location = new System.Drawing.Point(600, 380);
            this.txtSourceFrames.Name = "txtSourceFrames";
            this.txtSourceFrames.ReadOnly = true;
            this.txtSourceFrames.Size = new System.Drawing.Size(81, 20);
            this.txtSourceFrames.TabIndex = 39;
            // 
            // txtTargetFrames
            // 
            this.txtTargetFrames.Location = new System.Drawing.Point(600, 409);
            this.txtTargetFrames.Name = "txtTargetFrames";
            this.txtTargetFrames.ReadOnly = true;
            this.txtTargetFrames.Size = new System.Drawing.Size(81, 20);
            this.txtTargetFrames.TabIndex = 40;
            // 
            // frmCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 441);
            this.Controls.Add(this.txtTargetFrames);
            this.Controls.Add(this.txtSourceFrames);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnLoadTarget);
            this.Controls.Add(this.btnLoadSource);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCompare";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compare Input";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }        

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.Button btnLoadTarget;
        private System.Windows.Forms.Button btnClear;
        private MovieSplicer.Components.TASCompareListView lvOutput;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSourceFrames;
        private System.Windows.Forms.TextBox txtTargetFrames;
    }
}