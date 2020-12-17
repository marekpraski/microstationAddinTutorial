using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Bentley.MicroStation.InteropServices;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace csAddins
{
    [Bentley.MicroStation.AddInAttribute
                 (KeyinTree = "csAddins.commands.xml", MdlTaskID = "CSADDINS")]
    internal sealed class MyAddin : Bentley.MicroStation.AddIn
    {
        internal static MyAddin s_addin;
        private MyAddin(System.IntPtr mdlDesc)
            : base(mdlDesc)
        {
            s_addin = this;
        }
        protected override int Run(string[] commandLine)
        {
            BCOM.Application app = Utilities.ComApp;
            app.CadInputQueue.SendKeyin("csAddins DemoForm Toolbar");
            return 0;
        }
    }
}
