using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bentley.Interop.MicroStationDGN;

namespace csAddins
{
    public class DynamicLineDraw : IPrimitiveCommandEvents
    {
        private Application app = MyAddin.app;
        private Point3d[] linestringPoints = new Point3d[2];
        private int userClicked = 0;
        #region metody implementujące IPrimitiveCommandEvents
        public void Keyin(string Keyin)
        {
            
        }

        public void DataPoint(ref Point3d Point, View View)
        {
            if (userClicked == 0)
            {
                //zaczynając rysować, jeżeli zasnapuję się do elementu, mogę odczytać link
                Element el = MyAddin.app.CommandState.LocateElement(Point, View, true);
                bool hasLinks = checkLinksExist(el); 
                app.CommandState.StartDynamics();
                linestringPoints[0] = Point;
            }
            else
            {
                this.linestringPoints[userClicked] = Point;
                expandPointsArray();
                Dynamics(ref Point, View, MsdDrawingMode.Normal);
            }
            userClicked++;
        }

        public void Reset()
        {
            removeLastPoint();
            Element elem = app.CreateLineElement1(null, ref linestringPoints);
            app.ActiveModelReference.AddElement(elem);

            userClicked = 0;
            this.linestringPoints = new Point3d[2];
            app.CommandState.StartPrimitive(this, false);
        }

        public void Cleanup()
        {
            
        }

        public void Dynamics(ref Point3d Point, View View, MsdDrawingMode DrawMode)
        {
            if (userClicked == 0)
                return;
            linestringPoints[userClicked] = Point;

            Element elem = app.CreateLineElement1(null, ref linestringPoints);
            elem.Redraw(DrawMode);
        }

        public void Start()
        {
            app.CommandState.EnableAccuSnap();
        }

        #endregion

        private bool checkLinksExist(Element el)
        {
            if (el == null)
                return false;
            DatabaseLink[] links = el.GetDatabaseLinks(MsdDatabaseLinkage.Odbc);
            return links.Length > 0;
        }

        private void expandPointsArray()
        {
            Point3d[] newLinestringPoints = new Point3d[2 + userClicked];
            for (int i = 0; i < this.linestringPoints.Length; i++)
            {
                newLinestringPoints[i] = linestringPoints[i];
            }
            this.linestringPoints = newLinestringPoints;
        }

        private void removeLastPoint()
        {
            Point3d[] newLinestringPoints = new Point3d[this.linestringPoints.Length - 1];
            for (int i = 0; i < newLinestringPoints.Length; i++)
            {
                newLinestringPoints[i] = this.linestringPoints[i];
            }
            this.linestringPoints = newLinestringPoints;
        }

    }
}
