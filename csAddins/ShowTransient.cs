using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bentley.Interop.MicroStationDGN;

namespace csAddins
{
	internal class ShowTransient
	{
		private Application app = MyAddin.app;
		private List<Element> elems;
		private TransientElementContainer trc;

		private readonly string[] levelsToScan = new string[] { "75_Osie_wyrobisk", "75_Osie_wyrobisk_przerwa" };
        public ShowTransient()
        {
			this.elems = getCentreLinesFromDgn();
        }

		public void showLinkages()
		{
			for (int i = 0; i < this.elems.Count; i++)
			{
				showOneLinkageData(this.elems[i]);
			}
		}

		public void hideLinkages()
		{
			trc.Reset();
		}

		private void showOneLinkageData(Element el)
		{
			LineElement line = el.AsLineElement();
			DataBlock[] db = el.GetUserAttributeData(123);
			string s = "";
			if (db != null && db.Length > 0)
				db[0].CopyString(ref s, false); //odczytuję tekst, jest ""hello world!", tak jak przypisałem podczas tworzenia elementu

			Point3d origin = line.EndPoint;
			Matrix3d rotation = app.Matrix3dFromAxisAndRotationAngle(2, 0);    //działa tylko w TopView
			TextElement te = app.CreateTextElement1(null, s, ref origin, ref rotation);
			trc = app.CreateTransientElementContainer1(te, MsdTransientFlags.Overlay, MsdViewMask.AllViews, MsdDrawingMode.Temporary);
		}

		private List<Element> getCentreLinesFromDgn()
		{
			Element[] allElems = scanDgn(levelsToScan);

			List<Element> els = new List<Element>();
			for (int i = 0; i < allElems.Length; i++)
			{				
				if (allElems[i].IsLineElement())
					els.Add(allElems[i]);
			}
			return els;
		}

		private Element[] scanDgn(string[] levels)
		{
			ElementScanCriteria scanCriteria = new ElementScanCriteriaClass();
			scanCriteria.ExcludeAllLevels();
			for (int i = 0; i < levels.Length; i++)
			{
				Level lvl = app.ActiveDesignFile.Levels.Find(levels[i]);
				scanCriteria.IncludeLevel(lvl);
			}
			return app.ActiveModelReference.Scan(scanCriteria).BuildArrayFromContents();
		}
	}
}
