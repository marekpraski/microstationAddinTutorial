using Bentley.Interop.MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csAddins
{
	internal class DrawLineOnLine : IPrimitiveCommandEvents
	{
		private Application app = MyAddin.app;
		private Point3d[] linestringPoints = new Point3d[2];
		private int userClicked = 0;
		private double accuracy = 0.0001;
		private Element selectedLine = null;
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
				selectedLine = el;
				Point = drapePointOnLine(selectedLine, Point);
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
			string s = "hello world!";
			DataBlock dtb = new DataBlockClass();
			dtb.CopyString(ref s, true);
			elem.AddUserAttributeData(123, dtb);    //przekazany tekst zapisywany jest w elemencie w Linkages

			//app.ActiveModelReference.AddElement(elem);

			//nie dodaję tego elementu do dgn tylko wyświetlam go tymczasowo; taki element jest niezaznaczalny i znika po ponownym uruchomieniu funkcji
			app.CreateTransientElementContainer1(elem, MsdTransientFlags.Overlay, MsdViewMask.AllViews, MsdDrawingMode.Temporary);

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

			Point = drapePointOnLine(selectedLine, Point);
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

			DataBlock[] db = el.GetUserAttributeData(123);
			string s = "";
			if (db != null && db.Length > 0)
				db[0].CopyString(ref s, false); //odczytuję tekst, jest ""hello world!", tak jak przypisałem podczas tworzenia elementu
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

		private Point3d drapePointOnLine(Element el, Point3d point)
		{
			LineElement lineEl = el.AsLineElement();
			LineElement segment = getNearestSegment(lineEl);
			return getPerpendicular(point, segment);
		}

		/// <summary>
		/// chwilowo do testów zwraca pierwszy
		/// </summary>
		/// <param name="lineEl"></param>
		/// <returns></returns>
		private LineElement getNearestSegment(LineElement lineEl)
		{
			Point3d[] verts = new Point3d[2];
			verts[0] = lineEl.Segment[1].StartPoint;
			verts[1] = lineEl.Segment[1].EndPoint;
			return app.CreateLineElement1(null, ref verts);
		}

		/// <summary>
		/// zwraca linię prostopadłą przechodzącą przez zadany punkt, który nie leży na tej linii; pierwszy punkt zwracanej linii prostopadłej jest punktem przekazanym,
		/// drugi punkt leży na tej linii, na której wykonywana jest metoda
		/// </summary>
		public Point3d getPerpendicular(Point3d p, LineElement line)
		{

			if (Math.Abs(line.EndPoint.X - line.StartPoint.X) < accuracy) //ta linia jest pionowa
				return app.Point3dFromXY(line.StartPoint.X, p.Y);
			else if (Math.Abs(line.EndPoint.Y - line.StartPoint.Y) < accuracy) //ta linia jest pozioma
				return app.Point3dFromXY(p.X, line.StartPoint.Y);

			//określam linię wzorem y = ax + b
			double a = (line.EndPoint.Y - line.StartPoint.Y) / (line.EndPoint.X - line.StartPoint.X);
			double b = line.StartPoint.Y - a * line.StartPoint.X;

			double ap = -1 / a;     //współczynnik kierunkowy linii prostopadłej
			double bp = p.Y - ap * p.X;     //współczynnik b linii prostopadłej

			double x = (bp - b) / (a - ap);
			double y = a * x + b;
			return app.Point3dFromXY(x, y);   //punkt przecięcia tej linii z linią prostopadłą przechodzącą przez przekazany punkt
		}

	}
}
