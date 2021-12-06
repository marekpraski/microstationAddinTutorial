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
        public static BCOM.Application app;
        private MyAddin(System.IntPtr mdlDesc)
            : base(mdlDesc)
        {
            s_addin = this;
        }
        protected override int Run(string[] commandLine)
        {
            string unparsed = "";
            app = Utilities.ComApp;

            //TestMdlMethods.getTypeElement();
            //TestMdlMethods.getElementSize();
            //TestMdlMethods.getElementRange();
            //TestMdlMethods.mdlLine();

            //app.CadInputQueue.SendKeyin("DemoForm Toolbar");
            //CreateElement.LineAndLineString(unparsed);
            //CreateElement.ShapeAndComplexShape(unparsed);
            //CreateElement.ShapeHatched();
            //CreateElement.GroupedHoleHatched();
            //CreateElement.TextAndTextNode(unparsed);
            //CreateElement.CellAndSharedCell(unparsed);
            //CreateElement.LinearAndAngularDimension(unparsed);
            //CreateElement.CurveAndBsplineCurve(unparsed);
            //CreateElement.ConeAndBsplineSurface(unparsed);
            return 0;
        }
    }
}
