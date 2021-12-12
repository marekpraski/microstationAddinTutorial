using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace csAddins
{
    public class CommandsHandler
    {
        public static void DrawLineTest(string unparsed)
        {
            MyAddin.app.CommandState.StartPrimitive(new DynamicLineDraw());
        }

        public static void DrawLineSegmentTest(string unparsed)
        {
            MyAddin.app.CommandState.StartPrimitive(new SegmentDraw());
        }
    }
}
