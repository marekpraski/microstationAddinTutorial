using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Bentley.MicroStation.WinForms;
using System.IO;

namespace csAddins
{
    // In order to make a form on top of all other windows in Mstn, 
    //you should derive this form from a Bentley defined base class ----Bentley.MicroStation.WinForms.Adapter.
    public partial class MultiScaleCopyForm : Adapter //by³o Form
    {
        public MultiScaleCopyForm()
        {
            InitializeComponent();
        }

        private void MultiScaleCopyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(tbCopies.Text);
            sb.Append(" ");
            sb.Append(tbScale.Text);
            sb.Append(" ");
            sb.Append(tbXOffset.Text); 
            sb.Append(" ");
            sb.Append(tbYOffset.Text);
            sb.Append(" ");
            sb.Append(tbZOffset.Text);

            try
            {
                string fileName = "csAddins.ini";
                File.WriteAllText(fileName, sb.ToString());
            }
            catch
            {
                MessageBox.Show("b³¹d zapisu do pliku  ");
            }
        }

        private void MultiScaleCopyForm_Load(object sender, EventArgs e)
        {
            try
            {
                string iniData = File.ReadAllText("csAddins.ini");
                string[] data = iniData.Split(' ');
                tbCopies.Text = data[0];
                tbScale.Text = data[1];
                tbXOffset.Text = data[2];
                tbYOffset.Text = data[3];
                tbZOffset.Text = data[4];
            }
            catch
            {
                MessageBox.Show("b³¹d odczytu z pliku  ");
            }
        }

        private void tbScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
                e.Handled = true;
        }

        private void tbXOffset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '-')
                e.Handled = true;
        }

        private void tbCopies_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            tbScale.Text = tbScale.Tag.ToString();
            tbXOffset.Text = tbXOffset.Tag.ToString();
            tbYOffset.Text = tbYOffset.Tag.ToString();
            tbZOffset.Text = tbZOffset.Tag.ToString();
            tbCopies.Text = tbCopies.Tag.ToString();
        }
    }
}