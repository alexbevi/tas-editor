/******************************************************************************** 
 * TAS Movie Editor                                                             *
 *                                                                              *
 * Copyright notice for this file:                                              *
 *  Copyright (C) 2006-7 Maximus                                                *
 *                                                                              *
 * This program is free software; you can redistribute it and/or modify         *
 * it under the terms of the GNU General Public License as published by         *
 * the Free Software Foundation; either version 2 of the License, or            *
 * (at your option) any later version.                                          *
 *                                                                              *
 * This program is distributed in the hope that it will be useful,              *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
 * GNU General Public License for more details.                                 *
 *                                                                              *
 * You should have received a copy of the GNU General Public License            *
 * along with this program; if not, write to the Free Software                  *
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA    *
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Components;
using MovieSplicer.Data;

namespace MovieSplicer.UI
{
    public partial class frmEditing : TASForm
    {
        public TASListView     lvInput;
        public TASMovieInput[] FrameData;
        public UndoBuffer      UndoHistory;
        public frmMessages     Msg;

        public bool AutoFire;
        public bool EditingPrompts;

        public frmEditing()
        {
            InitializeComponent();
        }

        public void LoadSharedObjects(ref TASListView lv, ref TASMovieInput[] movie, ref UndoBuffer undo, ref frmMessages msg)
        {
            lvInput     = lv;
            FrameData   = movie;
            UndoHistory = undo;
            Msg         = msg;
        }

        // NOTE::These routines are just a quick hack to ensure that the edit field checkboxes
        // maintain the same enabled state as their corresponding text fields
        private void txtFrameDataC1_EnabledChanged(object sender, EventArgs e)
        {
            button1.Enabled = chkFrameDataC1.Enabled = txtFrameDataC1.Enabled;
        }
        private void txtFrameDataC2_EnabledChanged(object sender, EventArgs e)
        {
            button2.Enabled = chkFrameDataC2.Enabled = txtFrameDataC2.Enabled;
        }
        private void txtFrameDataC3_EnabledChanged(object sender, EventArgs e)
        {
            button3.Enabled = chkFrameDataC3.Enabled = txtFrameDataC3.Enabled;
        }
        private void txtFrameDataC4_EnabledChanged(object sender, EventArgs e)
        {
            button4.Enabled = chkFrameDataC4.Enabled = txtFrameDataC4.Enabled;
        }
        private void txtFrameDataC5_EnabledChanged(object sender, EventArgs e)
        {
            button5.Enabled = chkFrameDataC5.Enabled = txtFrameDataC5.Enabled;
        }

        /// <summary>
        /// Determine which controller editing fields to make available
        /// </summary>
        public void ToggleInputBoxes(bool[] activeControllers)
        {
            txtFrameDataC1.Enabled = activeControllers[0];
            txtFrameDataC2.Enabled = activeControllers[1];
            txtFrameDataC3.Enabled = activeControllers[2];
            txtFrameDataC4.Enabled = activeControllers[3];
            txtFrameDataC5.Enabled = activeControllers[4];
        }

        /// <summary>
        /// Fill the editing text fields with the values from the currently selected
        /// row in the main list
        /// </summary>
        public void PopulateEditFields(ListViewItem lvi)
        {
            int count = 0;
            if (txtFrameDataC1.Enabled) txtFrameDataC1.Text = lvi.SubItems[++count].Text;
            if (txtFrameDataC2.Enabled) txtFrameDataC2.Text = lvi.SubItems[++count].Text;
            if (txtFrameDataC3.Enabled) txtFrameDataC3.Text = lvi.SubItems[++count].Text;
            if (txtFrameDataC4.Enabled) txtFrameDataC4.Text = lvi.SubItems[++count].Text;
            if (txtFrameDataC5.Enabled) txtFrameDataC5.Text = lvi.SubItems[++count].Text;
        }


        /// <summary>
        /// Update the input array with the changed frame data
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvInput.SelectedIndices.Count == 0) return;

            bool[] updateFlag = { false, false, false, false, false };

            TASMovieInput updated = new TASMovieInput();

            if (sender == button1)
            {
                updated.Controller[0] = txtFrameDataC1.Text;
                updateFlag[0] = true;
            }
            else if (sender == button2)
            {
                updated.Controller[1] = txtFrameDataC2.Text;
                updateFlag[1] = true;
            }
            else if (sender == button3)
            {
                updated.Controller[2] = txtFrameDataC3.Text;
                updateFlag[2] = true;
            }
            else if (sender == button4)
            {
                updated.Controller[3] = txtFrameDataC4.Text;
                updateFlag[3] = true;
            }
            else if (sender == button5)
            {
                updated.Controller[4] = txtFrameDataC5.Text;
                updateFlag[4] = true;
            }
            else
            {
                if (chkFrameDataC1.Checked)
                {
                    updated.Controller[0] = txtFrameData.Text;
                    updateFlag[0] = true;
                }
                if (chkFrameDataC2.Checked)
                {
                    updated.Controller[1] = txtFrameData.Text;
                    updateFlag[1] = true;
                }
                if (chkFrameDataC3.Checked)
                {
                    updated.Controller[2] = txtFrameData.Text;
                    updateFlag[2] = true;
                }
                if (chkFrameDataC4.Checked)
                {
                    updated.Controller[3] = txtFrameData.Text;
                    updateFlag[3] = true;
                }
                if (chkFrameDataC5.Checked)
                {
                    updated.Controller[4] = txtFrameData.Text;
                    updateFlag[4] = true;
                }
            }

            // if no controllers were set, return
            if (!updateFlag[0] && !updateFlag[1] && !updateFlag[2] && !updateFlag[3] && !updateFlag[4])
                return;

            // prompt for multiple frame insertion
            if (lvInput.SelectedIndices.Count > 1 && EditingPrompts)
            {
                DialogResult confirmUpdate = MessageBox.Show(MovieSplicer.UI.frmMain.frm,
                    "Are you sure you want to update the " + lvInput.SelectedIndices.Count + " frames with the same input?",
                    "Confirm Multiple Frame Update",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (confirmUpdate != DialogResult.OK) return;
            }

            UndoBuffer.Add(ref UndoHistory, ref FrameData);

            int[] selectedIndices = new int[lvInput.SelectedIndices.Count];
            lvInput.SelectedIndices.CopyTo(selectedIndices, 0);

            // append or overwrite check
            if (chkAppendInput.Checked)
                TASMovieInput.UpdatePlus(ref FrameData, updated, updateFlag, AutoFire, selectedIndices);
            else
                TASMovieInput.Update(ref FrameData, updated, updateFlag, AutoFire, selectedIndices);

            lvInput.ClearVirtualCache();
            lvInput.Refresh();
            Msg.AddMsg("Updated " + lvInput.SelectedIndices.Count + " frame(s)");
        }

        public bool IsControllerChecked( int player )
        {
            if (player == 1)
                return chkFrameDataC1.Checked;
            else if (player == 2)
                return chkFrameDataC2.Checked;
            else if (player == 3)
                return chkFrameDataC3.Checked;
            else if (player == 4)
                return chkFrameDataC4.Checked;
            else if (player == 5)
                return chkFrameDataC5.Checked;

            return false;
        }

        private void frmEditing_Load(object sender, EventArgs e)
        {

        }

        private void frmEditing_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }

        private void txtFrameDataC1_DoubleClick(object sender, EventArgs e)
        {
            txtFrameData.Text = txtFrameDataC1.Text;
        }
        private void txtFrameDataC2_DoubleClick(object sender, EventArgs e)
        {
            txtFrameData.Text = txtFrameDataC2.Text;
        }
        private void txtFrameDataC3_DoubleClick(object sender, EventArgs e)
        {
            txtFrameData.Text = txtFrameDataC3.Text;
        }
        private void txtFrameDataC4_DoubleClick(object sender, EventArgs e)
        {
            txtFrameData.Text = txtFrameDataC4.Text;
        }
        private void txtFrameDataC5_DoubleClick(object sender, EventArgs e)
        {
            txtFrameData.Text = txtFrameDataC5.Text;
        }

        private void txtFrameData_DoubleClick(object sender, EventArgs e)
        {
            if (chkFrameDataC1.Checked)
            {
                txtFrameDataC1.Text = txtFrameData.Text;
            }
            if (chkFrameDataC2.Checked)
            {
                txtFrameDataC2.Text = txtFrameData.Text;
            }
            if (chkFrameDataC3.Checked)
            {
                txtFrameDataC3.Text = txtFrameData.Text;
            }
            if (chkFrameDataC4.Checked)
            {
                txtFrameDataC4.Text = txtFrameData.Text;
            }
            if (chkFrameDataC5.Checked)
            {
                txtFrameDataC5.Text = txtFrameData.Text;
            }
        }
    }
}