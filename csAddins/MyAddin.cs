using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace csAddins
{
    internal sealed class MyAddin : Bentley.MicroStation.AddIn
    {
        private MyAddin(System.IntPtr mdlDesc)
            : base(mdlDesc)
        {
        }
        protected override int Run(string[] commandLine)
        {
            //MessageBox.Show("Hello World");
            //string sWinFrameworkPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            //MessageBox.Show("Framework Path =" + sWinFrameworkPath);
            CreateElement.LineAndLineString();
            CreateElement.ShapeAndComplexShape();
            CreateElement.TextAndTextNode();
            CreateElement.CellAndSharedCell();
            CreateElement.LinearAndAngularDimension();
            CreateElement.CurveAndBsplineCurve();
            CreateElement.ConeAndBsplineSurface();
            return 0;
        }
    }
}
