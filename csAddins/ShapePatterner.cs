using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;

namespace csAddins
{
    public class ShapePatterner
    {
        private readonly Application app;

        public ShapePatterner(Application app)
        {
            this.app = app;
        }
        public ShapeElement applyPattern(ShapeElement shapeElement, AreaPattern pattern)
        {
            View view = app.CommandState.LastView();
            Matrix3d viewRoration = view.get_Rotation();
            shapeElement.SetPattern(pattern, viewRoration);
            changePatternCell(shapeElement);
            return shapeElement;
        }


        [DllImport("stdmdlbltin.dll")]
        public static extern int mdlPattern_addAssociative(ref int edPP, int line1, int line2,
            ref PatternParams pParams, ref Point3d pPoint, ref Matrix3d pRot, string option_, int modelRef);


        [DllImport("stdmdlbltin.dll")]
        public static extern int mdlPattern_extractAssociative(ref PatternParams pParams, ref Point3d pOrigin, 
            int elemP, int modelRef, int index);


        [DllImport("stdmdlaccessor.dll")]
        public static extern int ElmdscrAccessor_getMSElement(int ElementDescr);

        [StructLayout(LayoutKind.Sequential)]
        public class DwgHatchDefLine 
        {
            public double angle;
            public Point2d through;
            public Point2d offset;
            public short nDashes;   //VBA Integer (2 bytes)
            public double dashes;   //VBA dashes(20) double ???
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DwgHatchDefType 
        {
            public short nDefLines;   //VBA Integer (2 bytes)
            DwgHatchDefLine deprecatedLines; 
            public double pixelSize;
            public short islandStyle;   //VBA Integer (2 bytes)
        }

        [StructLayout(LayoutKind.Sequential)]
        public class PatternParams
        {
            public Matrix3d rMatrix;                        // pattern coordinate system       */
            public Point3d offset;                          // offset from element origin      */
            public double space1;                               // primary (row) spacing           */
            public double angle1;                               // angle of first hatch or pattern */
            public double space2;                               // secondary (column) spacing      */
            public double angle2;                               // angle of second hatch           */
            public double pscale;                                   // pattern scale                   */
            public double tolerance;                                     // pattern tolerance               */
            public IntPtr cellName = Marshal.AllocHGlobal(1024); //VBA cellName(1024) as Byte  // name of cell - if id is zero    */
            public long cellId;                                      // ID of shared cell definition    */
            public int modifiers;                               // pattern modifiers bit field     */
            public int minLine;                             // min line of multi-line element  */
            public int maxLine;                     // max line of multi-line element  */
            public int color;                       // pattern / hatch color           */
            public int weight;                       // pattern / hatch weight          */
            public int style;                       // pattern / hatch line style      */
            public short holeStyle;   //VBA Integer (2 bytes)  // hole parity style               */
            public DwgHatchDefType dwgHatchDef;       // DWG style hatch definition      */
            public Point3d origin;                    // hatch origin */
        }

        private void changePatternCell(Element elem) 
        {
            PatternParams ppar = new PatternParams();
            Point3d origin = app.Point3dFromXYZ(20, -6, 0);
            int modelRef = elem.ModelReference.MdlModelRefP();
            int elmP = elem.MdlElementDescrP();
            int MSElemP = ElmdscrAccessor_getMSElement(elmP);

            mdlPattern_extractAssociative(ref ppar, ref origin, MSElemP, modelRef, 0);
            ppar.cellId = 2264448492371968;
            mdlPattern_addAssociative(ref elmP, -1, -1, ref ppar, ref origin, ref ppar.rMatrix, "PATTERN_AREA", modelRef);
            elem = app.MdlCreateElementFromElementDescrP(elmP);
        }
    }
}
