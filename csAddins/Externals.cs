using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClassLibrary1;

namespace csAddins
{
    class Externals
    {

        public static void displayMessage(string unparsed)
        {
            string message = Class1.getMs();
            MessageBox.Show(message);
        }

    }
}
