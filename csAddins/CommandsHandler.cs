using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace csAddins
{
    public class CommandsHandler
    {
        public static void DrawLine(string unparsed)
        {
            MyAddin.app.CommandState.StartPrimitive(new DynamicLineDraw());
        }
		public static void DrawOnLine(string unparsed)
		{
			MyAddin.app.CommandState.StartPrimitive(new DrawLineOnLine());
		}
	}
}
