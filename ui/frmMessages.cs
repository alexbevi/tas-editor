using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MovieSplicer.UI
{
    public partial class frmMessages : Form
    {
        public frmMessages()
        {
            InitializeComponent();
        }

        public void AddMsg(string message)
        {
            lstMessages.Items.Add(DateTime.Now.ToString() + ": " + message);
        }
    }
}