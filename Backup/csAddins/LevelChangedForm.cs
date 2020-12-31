using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Bentley.MicroStation.WinForms;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace csAddins
{
    public partial class LevelChangedForm : Adapter
    {
        private BCOM.Application app = Utilities.ComApp;

        public LevelChangedForm()
        {
            InitializeComponent();
        }

        private void LevelChangedForm_Load(object sender, EventArgs e)
        {
            foreach (Level myLvl in app.ActiveDesignFile.Levels)
                listBox1.Items.Add(myLvl.Name);
        }

        private void LevelChangedForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            app.RemoveLevelChangeEventsHandler(DemoForm.myLevelChanged);
            MyAddin.s_addin.NewDesignFileEvent -= DemoForm.myNewDGNHandler;
        }
    }
}