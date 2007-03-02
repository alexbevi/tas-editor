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
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.lvSource = new MovieSplicer.Components.TASListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvTarget = new MovieSplicer.Components.TASListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btnLoadSource = new System.Windows.Forms.Button();
            this.btnLoadTarget = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox12.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.lvSource);
            this.groupBox12.Location = new System.Drawing.Point(12, 12);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(385, 299);
            this.groupBox12.TabIndex = 30;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Source";
            // 
            // lvSource
            // 
            this.lvSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSource.FullRowSelect = true;
            this.lvSource.GridLines = true;
            this.lvSource.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSource.Location = new System.Drawing.Point(8, 24);
            this.lvSource.Name = "lvSource";
            this.lvSource.Size = new System.Drawing.Size(371, 265);
            this.lvSource.TabIndex = 26;
            this.lvSource.UseCompatibleStateImageBehavior = false;
            this.lvSource.View = System.Windows.Forms.View.Details;
            this.lvSource.VirtualMode = true;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Frame";
            this.columnHeader1.Width = 367;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvTarget);
            this.groupBox1.Location = new System.Drawing.Point(403, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 299);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target";
            // 
            // lvTarget
            // 
            this.lvTarget.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTarget.FullRowSelect = true;
            this.lvTarget.GridLines = true;
            this.lvTarget.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTarget.Location = new System.Drawing.Point(8, 24);
            this.lvTarget.Name = "lvTarget";
            this.lvTarget.Scrollable = false;
            this.lvTarget.Size = new System.Drawing.Size(371, 265);
            this.lvTarget.TabIndex = 26;
            this.lvTarget.UseCompatibleStateImageBehavior = false;
            this.lvTarget.View = System.Windows.Forms.View.Details;
            this.lvTarget.VirtualMode = true;
            this.lvTarget.VertScrollValueChanged += new System.Windows.Forms.ScrollEventHandler(this.synchronizeScroll);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Frame";
            this.columnHeader2.Width = 367;
            // 
            // btnLoadSource
            // 
            this.btnLoadSource.Location = new System.Drawing.Point(12, 317);
            this.btnLoadSource.Name = "btnLoadSource";
            this.btnLoadSource.Size = new System.Drawing.Size(101, 23);
            this.btnLoadSource.TabIndex = 32;
            this.btnLoadSource.Text = "Load &Source";
            this.btnLoadSource.UseVisualStyleBackColor = true;
            this.btnLoadSource.Click += new System.EventHandler(this.btnLoadSource_Click);
            // 
            // btnLoadTarget
            // 
            this.btnLoadTarget.Location = new System.Drawing.Point(119, 317);
            this.btnLoadTarget.Name = "btnLoadTarget";
            this.btnLoadTarget.Size = new System.Drawing.Size(101, 23);
            this.btnLoadTarget.TabIndex = 32;
            this.btnLoadTarget.Text = "Load &Target";
            this.btnLoadTarget.UseVisualStyleBackColor = true;
            this.btnLoadTarget.Click += new System.EventHandler(this.btnLoadTarget_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(687, 317);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 23);
            this.button3.TabIndex = 33;
            this.button3.Text = "&Close";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // frmCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 350);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnLoadTarget);
            this.Controls.Add(this.btnLoadSource);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox12);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmCompare";
            this.Text = "Compare Input";
            this.groupBox12.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox12;
        private MovieSplicer.Components.TASListView lvSource;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox1;
        private MovieSplicer.Components.TASListView lvTarget;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnLoadSource;
        private System.Windows.Forms.Button btnLoadTarget;
        private System.Windows.Forms.Button button3;
    }
}