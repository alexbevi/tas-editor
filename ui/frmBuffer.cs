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
            
            bufferedInput = buffer;
            bufferedType  = type;

            sbarMovieType.Text = Enum.GetName(typeof(MovieType), type);

            if (bufferedInput == null) return;
            
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

            bufferedInput = null;

            lvInputBuffer.VirtualMode = false;
            lvInputBuffer.Clear();
            lvInputBuffer.Columns.Add("Frame");
                                    
            sbarMovieType.Text = "None";
            bufferedType       = MovieType.None;            
        }

        public void Dispose(ref ArrayList buffer)
        {
            buffer = bufferedInput;
            this.Dispose();            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
            {
                Methods.MovieBufferIO.Save(dlg.FileName, sbarMovieType.Text, bufferedInput);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
            {
                ArrayList buffer = new ArrayList();
                string bufferType = null ;
                Methods.MovieBufferIO.Load(dlg.FileName, ref bufferType, ref buffer);
                sbarMovieType.Text = bufferType;
            }
        }
    }
}