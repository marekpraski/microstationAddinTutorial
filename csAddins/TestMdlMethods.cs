using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using System;
using System.Runtime.InteropServices;

namespace csAddins
{
    class TestMdlMethods
    {
        [DllImport("stdmdlbltin.dll")]
        public static extern int mdlElement_getType(Element e);
        /*
         * deklaracja mdl
         * 
            int mdlElement_getType
            (
            MSElement const* el  
            );
        */
        // działa poprawnie
        public static void getTypeElement()
        {
            Element oLine = getLine();
            Element sh = getShape();
            Element cell = getCell();

            int typeLine = mdlElement_getType(oLine);
            int typeShape = mdlElement_getType(sh);
            int typeCell = mdlElement_getType(cell);
        }


        [DllImport("stdmdlbltin.dll")]
        public static extern int mdlElement_size(Element e);
        /*
         * deklaracja mdl
         * 
            int mdlElement_size 
            (
            MSElement const* el  
            );
        */
        // działa poprawnie
        public static void getElementSize()
        {
            Element oLine = getLine();
            Element sh = getShape();
            Element cell = getCell();

            Range3d r = sh.Range;
            
            int sizeLine = mdlElement_size(oLine);
            int sizeShape = mdlElement_size(sh);
            int sizeCell = mdlElement_size(cell);
        }


        [DllImport("stdmdlbltin.dll")]
        public static extern int mdlElement_extractRange(out Range3d range, Element e);
        /*
         * deklaracja mdl
         * 
            int       mdlElement_extractRange  
                ( 
                DVector3d*       rangeP , 
                MSElementCP       pElement  
                ); 

        */
        // 
        public static void getElementRange()
        {
            Element sh = getShape();

            Range3d r;
            int success = mdlElement_extractRange(out r, sh);
        }

        [DllImport("stdmdlbltin.dll")]
        public static extern int mdlLine_create
                    (
                    Element pElementOut,
                    Element pElementIn,
                    Point3d[] points
                    );

        public static void mdlLine()
        {
            Application app = Utilities.ComApp;

            Point3d[] pntArray = new Point3d[2];
            pntArray[0] = app.Point3dZero();
            pntArray[1] = app.Point3dFromXY(1, 2);

            Element line = null;
            mdlLine_create(line, null, pntArray);
        }

        public static Element getLine()
        {
            Application app = Utilities.ComApp;
            Point3d[] pntArray = new Point3d[2];
            pntArray[0] = app.Point3dZero();
            pntArray[1] = app.Point3dFromXY(10, 20);

            LineElement oLine = app.CreateLineElement1(null, ref pntArray);
            return oLine;
        }
        public static Element getShape()
        {
            Application app = Utilities.ComApp;
            Point3d[] pntArray = new Point3d[6];
            pntArray[0] = app.Point3dFromXY(0, -6);
            pntArray[1] = app.Point3dFromXY(0, -2);
            pntArray[2] = app.Point3dFromXY(2, -2);
            pntArray[3] = app.Point3dFromXY(2, -4);
            pntArray[4] = app.Point3dFromXY(4, -4);
            pntArray[5] = app.Point3dFromXY(4, -6);

            ShapeElement oShape = app.CreateShapeElement1(null, ref pntArray, MsdFillMode.NotFilled);
            return oShape;
        }

        public static Element getCell()
        {
            Application app = Utilities.ComApp;
            app.AttachCellLibrary("sample2.cel", MsdConversionMode.Always);
            Point3d origin = app.Point3dFromXY(1, -13);
            double xScale = 0.1 * app.ActiveModelReference.UORsPerMasterUnit / 1000.0;
            Point3d scale = app.Point3dFromXYZ(xScale, xScale, xScale);
            Matrix3d rMatrix = app.Matrix3dIdentity();
            CellElement oCell = app.CreateCellElement2("DECID", ref origin, ref scale, true, ref rMatrix);
            return oCell;
        }

    }
}
