using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/***************************************************************************************************
 * Show the Copy Buffer form
 * 
 * Contains options for Loading/Saving a buffers contents as well as clearing the currently
 * buffered information.
 **************************************************************************************************/
namespace MovieSplicer.UI
{
    public partial class frmBuffer : Form
    {
        ArrayList bufferedInput;
        MovieType bufferedType;
        
        public frmBuffer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create the frmBuffer form object and populate the listview based on the 
        /// arrayList of string[] content
        /// </summary>
        public frmBuffer(ref ArrayList buffer, MovieType type, int columns)
        {
            InitializeComponent();

            if (buffer != null)
            {
                bufferedInput = buffer;
                bufferedType = type;

                bindControllerDataToListview(columns);
            }
        }

        private void bindControllerDataToListview(int columns)
        {
            sbarMovieType.Text = Enum.GetName(typeof(MovieType), bufferedType);

            // set controller columns            
            for (int j = 0; j < columns; j++)
                lvInputBuffer.Columns.Add("Controller " + (j + 1), 75);

            // HACK::set the first column's width once all columns are set. 
            // This prevents a weird sizing error that occurs periodically
            lvInputBuffer.Columns[0].Width = 75;

            try
            {
                lvInputBuffer.VirtualListSize = bufferedInput.Count;
                lvInputBuffer.VirtualListSource = bufferedInput;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Clear the Buffer (listview and related objects)
        /// </summary>        
        private void btnClear_Click(object sender, EventArgs e)
        {
            // if this object hasn't been created yet, return
            if (bufferedInput == null) return;
            
            clearBuffer();            
        }

        private void clearBuffer()
        {
            bufferedInput = null;

            lvInputBuffer.VirtualListSize = 0;
            lvInputBuffer.Clear(); lvInputBuffer.Columns.Add("Frame");

            sbarMovieType.Text = "None";
            bufferedType = MovieType.None;            
        }

        /// <summary>
        /// Destroy the form object, but pass the buffer and movie type back
        /// </summary>        
        public void Dispose(ref ArrayList buffer, ref MovieType type)
        {
            buffer = bufferedInput;
            type = bufferedType;
            this.Dispose();            
        }

        /// <summary>
        /// Save the contents of the copy buffer to an external file
        /// </summary>        
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "TAS Movie Editor Copy Buffer (*.tmb)|*.tmb)";
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
            {
                Methods.MovieBufferIO.Save(dlg.FileName, sbarMovieType.Text, bufferedInput, lvInputBuffer.Columns.Count - 1);
            }
        }

        /// <summary>
        /// Load the contents of an external file to the copy buffer
        /// </summary>        
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "TAS Movie Editor Copy Buffer (*.tmb)|*.tmb)";
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
            {
                ArrayList buffer = new ArrayList();
                string bufferType = null;
                Methods.MovieBufferIO.Load(dlg.FileName, ref bufferType, ref buffer);
                sbarMovieType.Text = bufferType;               

                if (buffer.Count > 0)
                {
                    lvInputBuffer.Clear(); lvInputBuffer.Columns.Add("Frame");

                    bufferedInput = buffer;
                    bufferedType = (MovieType)Enum.Parse(typeof(MovieType), bufferType);
                    int columns = ((string[])(bufferedInput[0])).Length;
                    bindControllerDataToListview(columns);
                }              
                else 
                    clearBuffer(); 
            }
        }

        /// <summary>
        /// Close the View Buffer form
        /// </summary>        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}