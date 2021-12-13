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
                string cellText = String.IsNullOrEmpty(inputForm.tbStart.Text) ? "start" : inputForm.tbStart.Text;
                placeCell(Point, View, cellText);
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
            inputForm.DetachFromMicroStation();
        }

        public void Dynamics(ref Point3d Point, View View, MsdDrawingMode DrawMode)
        {
            if (this.clickCounter == 0)
            {
                string cellText = String.IsNullOrEmpty(inputForm.tbStart.Text) ? "start" : inputForm.tbStart.Text;
                drawCell(ref Point, View, DrawMode, cellText);
            }
            if (this.clickCounter == 1)
                drawLine(ref Point, View, DrawMode);
        }

        public void Start()
        {
            inputForm.AttachToToolSettings(MyAddin.s_addin);
            inputForm.Show();
            app.CommandState.EnableAccuSnap();
            app.CommandState.StartDynamics();
        }

        #endregion

        private void drawCell(ref Point3d point, View view, MsdDrawingMode drawMode, string text)
        {
            TextElement textElem = makeTextElement(ref point, view, text);
            textElem.Redraw(drawMode);
        }

        private void placeCell(Point3d point, View view, string text)
        {
            TextElement textElem = makeTextElement(ref point, view, text);
            this.app.ActiveModelReference.AddElement(textElem);
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
            elems[1] = makeTextElement(ref point, view, inputForm.tbKoniec.Text);
            app.ActiveModelReference.AddElements(ref elems);
        }

        #region tworzenie celki
        private TextElement makeTextElement(ref Point3d point, View view, string text)
        {
            Matrix3d rMatrix = app.Matrix3dIdentity();
            TextElement te = app.CreateTextElement1(null, text, ref point, ref rMatrix);
            te.TextStyle.Font = app.ActiveDesignFile.Fonts.Find(MsdFontType.MicroStation, "ENGINEERING", null);
            te.TextStyle.Justification = MsdTextJustification.RightBottom;
            te.TextStyle.Height = 4;
            te.TextStyle.Width = 3;

            return te;
        }

        #endregion
    }
}