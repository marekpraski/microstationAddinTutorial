using Bentley.Interop.MicroStationDGN;
using System;

namespace csAddins
{
    public class SegmentDraw : IPrimitiveCommandEvents
    {
        private Application app = MyAddin.app;
        private int clickCounter = 0;
        private Point3d[] segmentPoints = new Point3d[2];
        private SegmentDrawForm inputForm = new SegmentDrawForm();

        #region metody spełniające interfejs IPrimitiveCommandEvents

        public void Keyin(string Keyin)
        {
            
        }

        public void DataPoint(ref Point3d Point, View View)
        {
            if(this.clickCounter == 0)
            {
                this.segmentPoints[0] = Point;
                Dynamics(ref Point, View, MsdDrawingMode.Normal);
                placeCell(Point, View, MsdDrawingMode.Normal);
            }
            if(this.clickCounter == 1)
            {
                Dynamics(ref Point, View, MsdDrawingMode.Normal);
                this.segmentPoints[1] = Point;
                finalizeOperation(Point, View);
                app.CommandState.StopDynamics();
            }
            this.clickCounter++;
        }

        public void Reset()
        {
            app.CommandState.SetDefaultCursor();
        }

        public void Cleanup()
        {
            
        }

        public void Dynamics(ref Point3d Point, View View, MsdDrawingMode DrawMode)
        {
            if(this.clickCounter == 0)
                drawCell(ref Point, View, DrawMode);
            if (this.clickCounter == 1)
                drawLine(ref Point, View, DrawMode);
        }

        public void Start()
        {
            //inputForm.AttachToToolSettings(MyAddin.s_addin);
            //inputForm.Show();
            app.CommandState.EnableAccuSnap();
            app.CommandState.StartDynamics();
        }

        #endregion

        private void drawCell(ref Point3d point, View view, MsdDrawingMode drawMode)
        {
            CellElement cell = makeCell(ref point, view);
            cell.Redraw(drawMode);
        }

        private void placeCell(Point3d point, View view, MsdDrawingMode drawMode)
        {
            CellElement cell = makeCell(ref point, view);
            this.app.ActiveModelReference.AddElement(cell);
        }

        private void drawLine(ref Point3d point, View view, MsdDrawingMode drawMode)
        {
            this.segmentPoints[1] = point;
            Element line = app.CreateLineElement1(null, ref this.segmentPoints);
            line.Redraw(drawMode);
        }

        private void finalizeOperation(Point3d point, View view)
        {
            Element[] elems = new Element[2];
            elems[0] = app.CreateLineElement1(null, ref this.segmentPoints);
            elems[1] = makeCell(ref point, view);
            app.ActiveModelReference.AddElements(ref elems);
        }

        #region tworzenie celki przy kursorze
        private CellElement makeCell(ref Point3d point, View view)
        {
            string cellName = "cross";
            Element[] cellElems = getCellSegments(point);
            //Element[] cellElems = getTextCellSegments(point);
            return app.CreateCellElement1(cellName, ref cellElems, point, false);
        }

        private Element[] getTextCellSegments(Point3d point)
        {
            throw new NotImplementedException();
        }

        private Element[] getCellSegments(Point3d point)
        {
            
            Point3d p1 = app.Point3dFromXY(point.X, point.Y + 3);
            Point3d p2 = app.Point3dFromXY(point.X, point.Y - 3);
            Point3d p3 = app.Point3dFromXY(point.X + 3, point.Y);
            Point3d p4 = app.Point3dFromXY(point.X - 3, point.Y);
            Point3d[] line1Points = new Point3d[] { p1, p2 };
            Point3d[] line2Points = new Point3d[] { p3, p4 };
            Element line1 = app.CreateLineElement1(null, ref line1Points);
            Element line2 = app.CreateLineElement1(null, ref line2Points);
            return new Element[] { line1, line2 };
        }

        #endregion
    }
}