using System;
using System.Collections.Generic;
using System.Text;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;

namespace csAddins
{
    class CreateElement
    {
        public static void LineAndLineString()
        {
            Application app = Utilities.ComApp;
            Point3d startPnt = app.Point3dZero();
            Point3d endPnt = startPnt;
            startPnt.X = 10;
            LineElement oLine = app.CreateLineElement2(null, ref startPnt, ref endPnt);
            oLine.Color = 0; oLine.LineWeight = 2;
            app.ActiveModelReference.AddElement(oLine);

            Point3d[] pntArray = new Point3d[5];
            pntArray[0] = app.Point3dZero();
            pntArray[1] = app.Point3dFromXY(1, 2);
            pntArray[2] = app.Point3dFromXY(3, -2);
            pntArray[3] = app.Point3dFromXY(5, 2);
            pntArray[4] = app.Point3dFromXY(6, 0);
            oLine = app.CreateLineElement1(null, ref pntArray);
            oLine.Color = 1; oLine.LineWeight = 2;
            app.ActiveModelReference.AddElement(oLine);
        }
        public static void ShapeAndComplexShape()
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
            oShape.Color = 0; oShape.LineWeight = 2;
            app.ActiveModelReference.AddElement(oShape);

            ChainableElement[] elmArray = new ChainableElement[2];
            for (int i = 0; i < 6; i++)
                pntArray[i].X += 5;
            elmArray[0] = app.CreateLineElement1(null, ref pntArray);
            pntArray[2].Y = -8;
            elmArray[1] = app.CreateArcElement3(null, ref pntArray[5], ref pntArray[2], ref pntArray[0]);
            ComplexShapeElement oComplexShape = app.CreateComplexShapeElement1(ref elmArray, MsdFillMode.NotFilled);
            oComplexShape.Color = 1; oComplexShape.LineWeight = 2;
            app.ActiveModelReference.AddElement(oComplexShape);
        }
        public static void TextAndTextNode()
        {
            Application app = Utilities.ComApp;
            double savedTextHeight = app.ActiveSettings.TextStyle.Height;
            double savedTextWidth = app.ActiveSettings.TextStyle.Width;
            Font savedFont = app.ActiveSettings.TextStyle.Font;
            bool savedAnnotationScaleEnabled = app.ActiveSettings.AnnotationScaleEnabled;
            app.ActiveSettings.TextStyle.Height = 0.7;
            app.ActiveSettings.TextStyle.Width = 0.6;
            app.ActiveSettings.TextStyle.Font = app.ActiveDesignFile.Fonts.Find(MsdFontType.WindowsTrueType, "Arial", null);
            app.ActiveSettings.AnnotationScaleEnabled = false;

            Point3d origin = app.Point3dFromXY(0, -7.5);
            Matrix3d rMatrix = app.Matrix3dIdentity();
            TextElement oText = app.CreateTextElement1(null, "Text String", ref origin, ref rMatrix);
            oText.Color = 0;
            app.ActiveModelReference.AddElement(oText);

            origin = app.Point3dFromXY(2, -9);
            TextNodeElement oTN = app.CreateTextNodeElement1(null, ref origin, ref rMatrix);
            oTN.AddTextLine("Text Node Line 1");
            oTN.AddTextLine("Text Node Line 2");
            oTN.Color = 1;
            app.ActiveModelReference.AddElement(oTN);

            app.ActiveSettings.TextStyle.Height = savedTextHeight;
            app.ActiveSettings.TextStyle.Width = savedTextWidth;
            app.ActiveSettings.TextStyle.Font = savedFont;
            app.ActiveSettings.AnnotationScaleEnabled = savedAnnotationScaleEnabled;
        }
        public static void CellAndSharedCell()
        {
            Application app = Utilities.ComApp;
            app.AttachCellLibrary("sample2.cel", MsdConversionMode.Always);
            Point3d origin = app.Point3dFromXY(1, -13);
            double xScale = 0.1 * app.ActiveModelReference.UORsPerMasterUnit / 1000.0;
            Point3d scale = app.Point3dFromXYZ(xScale, xScale, xScale);
            Matrix3d rMatrix = app.Matrix3dIdentity();
            CellElement oCell = app.CreateCellElement2("DECID", ref origin, ref scale, true, ref rMatrix);
            oCell.Color = 0;
            app.ActiveModelReference.AddElement(oCell);

            SharedCellElement oSC;
            xScale = 0.02 * app.ActiveModelReference.UORsPerMasterUnit / 1000.0;
            scale = app.Point3dFromXYZ(xScale, xScale, xScale);
            for (int x = 4; x <= 8; x += 2)
            {
                origin = app.Point3dFromXY(x, -14);
                oSC = app.CreateSharedCellElement2("NORTH", ref origin, ref scale, true, ref rMatrix);
                oSC.OverridesComponentColor = true; oSC.Color = 1;
                app.ActiveModelReference.AddElement(oSC);
            }
        }
        public static void LinearAndAngularDimension()
        {
            Application app = Utilities.ComApp;
            DimensionStyle ds = app.ActiveSettings.DimensionStyle;
            ds.OverrideAnnotationScale = true; ds.AnnotationScale = 1;
            ds.OverrideTextHeight = true; ds.TextHeight = 0.5;
            ds.OverrideTextWidth = true; ds.TextWidth = 0.4;
            ds.MinLeader = 0.01;
            ds.TerminatorArrowhead = MsdDimTerminatorArrowhead.Filled;
            Matrix3d rMatrix = app.Matrix3dIdentity();
            Point3d[] pnts = new Point3d[3];
            pnts[0] = app.Point3dFromXY(0, -17);
            pnts[1] = app.Point3dFromXY(3, -17);
            DimensionElement oDim = app.CreateDimensionElement1(null, ref rMatrix, MsdDimType.SizeArrow, null);
            oDim.AddReferencePoint(app.ActiveModelReference, ref pnts[0]);
            oDim.AddReferencePoint(app.ActiveModelReference, ref pnts[1]);
            oDim.Color = 0; oDim.DimHeight = 1;
            app.ActiveModelReference.AddElement(oDim);

            ds.AnglePrecision = MsdDimValueAnglePrecision.DimValueAnglePrecision1Place;
            oDim = app.CreateDimensionElement1(null, ref rMatrix, MsdDimType.AngleSize, null);
            pnts[0] = app.Point3dFromXY(7, -13);
            pnts[1] = app.Point3dFromXY(5, -15);
            pnts[2] = app.Point3dFromXY(9, -15);
            oDim.AddReferencePoint(app.ActiveModelReference, ref pnts[0]);
            oDim.AddReferencePoint(app.ActiveModelReference, ref pnts[1]);
            oDim.AddReferencePoint(app.ActiveModelReference, ref pnts[2]);
            oDim.Color = 1; oDim.DimHeight = 1;
            app.ActiveModelReference.AddElement(oDim);
        }
        public static void CurveAndBsplineCurve()
        {
            Application app = Utilities.ComApp;
            Point3d[] pntArray = new Point3d[5];
            pntArray[0] = app.Point3dFromXY(0, -19);
            pntArray[1] = app.Point3dFromXY(1, -17);
            pntArray[2] = app.Point3dFromXY(2, -19);
            pntArray[3] = app.Point3dFromXY(3, -17);
            pntArray[4] = app.Point3dFromXY(4, -19);
            CurveElement oCurve = app.CreateCurveElement1(null, ref pntArray);
            oCurve.Color = 0; oCurve.LineWeight = 2;
            app.ActiveModelReference.AddElement(oCurve);

            for (int i = 0; i < 5; i++)
                pntArray[i].X += 5;
            InterpolationCurve oInterpolationCurve = new InterpolationCurveClass();
            oInterpolationCurve.SetFitPoints(ref pntArray);
            BsplineCurveElement oBsplineCurve = app.CreateBsplineCurveElement2(null, oInterpolationCurve);
            oBsplineCurve.Color = 1; oBsplineCurve.LineWeight = 2;
            app.ActiveModelReference.AddElement(oBsplineCurve);
        }
        public static void ConeAndBsplineSurface()
        {
            Application app = Utilities.ComApp;
            Point3d basePt = app.Point3dFromXYZ(2, -23, 0);
            Point3d topPt = app.Point3dFromXYZ(2, -20, 0);
            Matrix3d rMatrix = app.Matrix3dFromAxisAndRotationAngle(0, app.Pi() / 6);
            ConeElement oCone = app.CreateConeElement1(null, 2, ref basePt, 1, ref topPt, ref rMatrix);
            oCone.Color = 0;
            app.ActiveModelReference.AddElement(oCone);

            Point3d[] aFitPnts = new Point3d[4];
            InterpolationCurve oFitCurve = new InterpolationCurveClass();
            BsplineCurve[] aCurves = new BsplineCurve[3];

            aFitPnts[0] = app.Point3dFromXYZ(5.9, -21, -0.5);
            aFitPnts[1] = app.Point3dFromXYZ(6.9, -20, 1);
            aFitPnts[2] = app.Point3dFromXYZ(7.9, -20.3, 1.3);
            aFitPnts[3] = app.Point3dFromXYZ(8.9, -20.8, 0.3);
            oFitCurve.SetFitPoints(ref aFitPnts);
            oFitCurve.BesselTangents = true;
            aCurves[0] = new BsplineCurveClass();
            aCurves[0].FromInterpolationCurve(oFitCurve);

            aFitPnts[0] = app.Point3dFromXYZ(6.4, -22, 0);
            aFitPnts[1] = app.Point3dFromXYZ(7.1, -21.2, 0.7);
            aFitPnts[2] = app.Point3dFromXYZ(7.7, -21, 1);
            aFitPnts[3] = app.Point3dFromXYZ(8.4, -21.7, -0.2);
            oFitCurve.SetFitPoints(ref aFitPnts);
            oFitCurve.BesselTangents = true;
            aCurves[1] = new BsplineCurveClass();
            aCurves[1].FromInterpolationCurve(oFitCurve);

            aFitPnts[0] = app.Point3dFromXYZ(5.9, -23, 0);
            aFitPnts[1] = app.Point3dFromXYZ(7.2, -23.1, 1.2);
            aFitPnts[2] = app.Point3dFromXYZ(7.8, -23.3, 0.8);
            aFitPnts[3] = app.Point3dFromXYZ(8.7, -22.8, 0.2);
            oFitCurve.SetFitPoints(ref aFitPnts);
            oFitCurve.BesselTangents = true;
            aCurves[2] = new BsplineCurveClass();
            aCurves[2].FromInterpolationCurve(oFitCurve);

            BsplineSurface oBsplineSurface = new BsplineSurfaceClass();
            oBsplineSurface.FromCrossSections(ref aCurves, MsdBsplineSurfaceDirection.V, 4, true, true);
            BsplineSurfaceElement oSurfaceElm = app.CreateBsplineSurfaceElement1(null, oBsplineSurface);
            oSurfaceElm.Color = 1;
            app.ActiveModelReference.AddElement(oSurfaceElm);
        }
    }
}
