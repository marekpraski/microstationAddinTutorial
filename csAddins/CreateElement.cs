
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace csAddins
{
    class CreateElement
    {

        public static ShapeElement createShapeElement()
        {
            Application app = Bentley.MicroStation.InteropServices.Utilities.ComApp;
            Point3d[] pntArray = new Point3d[4];

            //flat shape
            //pntArray[0] = app.Point3dFromXYZ(20, -6, 0);
            //pntArray[1] = app.Point3dFromXYZ(20, 0, 0);
            //pntArray[2] = app.Point3dFromXYZ(30, 0, 0);
            //pntArray[3] = app.Point3dFromXYZ(30, -6, 0);

            //vertical shape
            pntArray[0] = app.Point3dFromXYZ(20, 0, -6);
            pntArray[3] = app.Point3dFromXYZ(20, 0, 0);
            pntArray[2] = app.Point3dFromXYZ(30, 0, 0);
            pntArray[1] = app.Point3dFromXYZ(30, 0, -6);
            return app.CreateShapeElement1(null, ref pntArray, MsdFillMode.Outlined);
        }

        public static void createPatternedArea(string unparsed)
        {
            Application app = Bentley.MicroStation.InteropServices.Utilities.ComApp;
            ShapeElement shapeElement = createShapeElement();

            app.ActiveModelReference.AddElement(shapeElement);
            int pos = shapeElement.FilePosition;
            string quote = "\"";

            string keyin = "mdl load mp " + quote + pos + " 1 0 0 14" + quote;
            app.CadInputQueue.SendCommand(keyin, true);
        }

    }
}
