
using System.Windows.Forms;
using Bentley.MicroStation.WinForms;

namespace csAddins
{
    public partial class SegmentDrawForm : Adapter
    {
        public SegmentDrawForm()
        {
            InitializeComponent();
        }

        private void tbStart_TextChanged(object sender, System.EventArgs e)
        {
            tbKoniec.Text = tbStart.Text;
        }
    }
}
