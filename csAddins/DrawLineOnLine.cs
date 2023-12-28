using Bentley.Interop.MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace csAddins
{
	internal class DrawLineOnLine : IPrimitiveCommandEvents
	{
		private Application app = MyAddin.app;
		private Point3d[] linestringPoints = new Point3d[2];
		private bool userClicked = false;
		private double accuracy = 0.0001;
		private LineElement selectedLine = null;

		private List<Segment3d> lineSegments = new List<Segment3d>();
		#region metody implementujące IPrimitiveCommandEvents
		public void Keyin(string Keyin)
		{

		}

		public void DataPoint(ref Point3d Point, View View)
		{
			if (!userClicked)
			{
				//zaczynając rysować, jeżeli zasnapuję się do elementu, mogę odczytać link
				Element el = MyAddin.app.CommandState.LocateElement(Point, View, true);
				if (!setSelectedLine(el))
					return;
				bool hasLinks = checkLinksExist(el);
				int index = getNearestSegmentIndex(selectedLine, Point);
				lineSegments.Add(selectedLine.Segment[index]);
				Point = drapePointOnLineSegment(selectedLine.Segment[index], Point);
				app.CommandState.StartDynamics();
				linestringPoints[0] = Point;
				userClicked = true;
			}
			else
				constructLine();
		}

		private void constructLine()
		{
			Element elem = app.CreateLineElement1(null, ref linestringPoints);
			string s = "hello world!";
			DataBlock dtb = new DataBlockClass();
			dtb.CopyString(ref s, true);
			elem.AddUserAttributeData(123, dtb);    //przekazany tekst zapisywany jest w elemencie w Linkages

			app.ActiveModelReference.AddElement(elem);

			//nie dodaję tego elementu do dgn tylko wyświetlam go tymczasowo; taki element jest niezaznaczalny i znika po ponownym uruchomieniu funkcji
			//app.CreateTransientElementContainer1(elem, MsdTransientFlags.Overlay, MsdViewMask.AllViews, MsdDrawingMode.Temporary);

			Reset();
		}

		public void Reset()
		{
			userClicked = false;
			this.linestringPoints = new Point3d[2];
			this.lineSegments.Clear();
			this.selectedLine = null;
			app.CommandState.StartPrimitive(this, false);
		}

		public void Cleanup()
		{

		}

		public void Dynamics(ref Point3d Point, View View, MsdDrawingMode DrawMode)
		{
			if (!userClicked)
				return;

			int index = getNearestSegmentIndex(selectedLine, Point);
			if (index == 0)		//indeksy w obiektach Bentley zaczynają się od 1
				return;

			if (!segmentsAreEqual(selectedLine.Segment[index], lineSegments.Last()))      //przeskoczył do innego segmentu linii;
				modifyLinestingPointsArray(index);

			Point = drapePointOnLineSegment(selectedLine.Segment[index], Point);
			linestringPoints[linestringPoints.Length - 1] = Point;

			Element elem = app.CreateLineElement1(null, ref linestringPoints);
			elem.Redraw(DrawMode);
		}

		private void modifyLinestingPointsArray(int index)
		{
			Point3d p = getSharedPoint(selectedLine.Segment[index], lineSegments.Last());
			if (linestringPoints.Length == 2)      //przeskoczył do kolejnego segmentu linii po raz pierwszy
			{
				expandPointsArray();
				linestringPoints[linestringPoints.Length - 2] = p;
				lineSegments.Add(selectedLine.Segment[index]);
			}
			else if (linestringPoints.Length > 2 && !pointsAreEqual(p, linestringPoints[linestringPoints.Length - 2]))      //przeskoczył do kolejnego segmentu linii; porównuję z przedostatnim, bo to jest werteks linii
			{
				expandPointsArray();
				linestringPoints[linestringPoints.Length - 2] = p;
				lineSegments.Add(selectedLine.Segment[index]);
			}
			else if (linestringPoints.Length > 2 && pointsAreEqual(p, linestringPoints[linestringPoints.Length - 2]))     //wrócił do poprzedniego segmentu linii; porównuję z przedostatnim, bo to jest werteks linii
			{
				contractPointsArray();
				lineSegments.RemoveAt(lineSegments.Count - 1);
			}
		}

		private Point3d getSharedPoint(Segment3d segmentOne, Segment3d segmentTwo)
		{
			if (pointsAreEqual(segmentOne.EndPoint, segmentTwo.StartPoint))
				return segmentOne.EndPoint;
			
			return segmentTwo.EndPoint;
		}

		private bool segmentsAreEqual(Segment3d segmentOne, Segment3d segmentTwo)
		{
			return pointsAreEqual(segmentOne.StartPoint, segmentTwo.StartPoint) && pointsAreEqual(segmentOne.EndPoint, segmentTwo.EndPoint); ;
		}

		private bool pointsAreEqual(Point3d p1, Point3d p2)
		{
			return Math.Abs(p1.X - p2.X) < accuracy && Math.Abs(p1.Y - p2.Y) < accuracy;
		}

		public void Start()
		{
			app.CommandState.EnableAccuSnap();
		}

		#endregion

		private bool setSelectedLine(Element el)
		{
			if (el.IsLineElement())
			{
				selectedLine = el.AsLineElement();
				return true;
			}
			return false;
		}

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
			Point3d[] newLinestringPoints = new Point3d[linestringPoints.Length + 1];
			for (int i = 0; i < this.linestringPoints.Length; i++)
			{
				newLinestringPoints[i] = linestringPoints[i];
			}
			this.linestringPoints = newLinestringPoints;
		}

		private void contractPointsArray()
		{
			Point3d[] newLinestringPoints = new Point3d[linestringPoints.Length - 1];
			for (int i = 0; i < newLinestringPoints.Length; i++)
			{
				newLinestringPoints[i] = linestringPoints[i];
			}
			this.linestringPoints = newLinestringPoints;
		}

		private Point3d drapePointOnLineSegment(Segment3d lineSegment, Point3d p)
		{
			if (Math.Abs(lineSegment.EndPoint.X - lineSegment.StartPoint.X) < accuracy) //ta linia jest pionowa
				return app.Point3dFromXY(lineSegment.StartPoint.X, p.Y);
			else if (Math.Abs(lineSegment.EndPoint.Y - lineSegment.StartPoint.Y) < accuracy) //ta linia jest pozioma
				return app.Point3dFromXY(p.X, lineSegment.StartPoint.Y);

			//określam linię wzorem y = ax + b
			double a = (lineSegment.EndPoint.Y - lineSegment.StartPoint.Y) / (lineSegment.EndPoint.X - lineSegment.StartPoint.X);
			double b = lineSegment.StartPoint.Y - a * lineSegment.StartPoint.X;

			double ap = -1 / a;     //współczynnik kierunkowy linii prostopadłej
			double bp = p.Y - ap * p.X;     //współczynnik b linii prostopadłej

			double x = (bp - b) / (a - ap);
			double y = a * x + b;
			return app.Point3dFromXY(x, y);   //punkt przecięcia tej linii z linią prostopadłą przechodzącą przez przekazany punkt
		}

		private int getNearestSegmentIndex(LineElement lineEl, Point3d point)
		{
			for (int i = 0; i < lineEl.SegmentsCount; i++)
			{
				Segment3d s = lineEl.Segment[i + 1];
				Point3d p = drapePointOnLineSegment(s,point);
				if (isPointOnSegment(s, p))
					return i + 1;
			}
			return 0;
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

		private double getDistanceBetweenPoints(Point3d p1, Point3d p2)
		{
			double a = (p1.X - p2.X) * (p1.X - p2.X);
			double b = (p1.Y - p2.Y) * (p1.Y - p2.Y);
			double c = a + b;

			return Math.Sqrt(c);
		}


		public bool isPointOnSegment(Segment3d lineSegment, Point3d p)
		{
			//punkt X leży na odcinku AB jeżeli suma długości odcinków AX + XB jest równa długości odcinka AB
			double segmentLength = getDistanceBetweenPoints(lineSegment.StartPoint, lineSegment.EndPoint);
			double dist1 = getDistanceBetweenPoints(p, lineSegment.StartPoint);
			double dist2 = getDistanceBetweenPoints(p, lineSegment.EndPoint);
			return (Math.Abs(dist1 + dist2 - segmentLength) < accuracy);
		}

	}
}
