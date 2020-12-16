using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace csAddins
{
    [Bentley.MicroStation.AddInAttribute
                 (KeyinTree = "csAddins.commands.xml", MdlTaskID = "CSADDINS")]
    internal sealed class MyAddin : Bentley.MicroStation.AddIn
    {
        private MyAddin(System.IntPtr mdlDesc)
            : base(mdlDesc)
        {
        }
        protected override int Run(string[] commandLine)
        {
            return 0;
        }
    }
}
