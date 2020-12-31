using System;
using System.Collections.Generic;
using System.Text;
using MPdll;
using System.Runtime.InteropServices;

namespace csAddins
{
    class Externals
    {
        //public static void displayMessage(string unparsed) 
        //{
        //    Class1.displayMessage();
        //}
        [DllImport("ModelerSql.dll")]
        public static extern void displayMe();

        public static void displayMessage(string unparsed)
        {
            displayMe();
        }
    }
}
