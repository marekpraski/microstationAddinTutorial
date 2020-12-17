using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace csAddins
{
    class DemoForm
    {
        public static void Toolbar(string unparsed)
        {
            ToolbarForm toolbarForm = new ToolbarForm();
            toolbarForm.AttachAsGuiDockable(MyAddin.s_addin, "toolbar");
            toolbarForm.Show();
        }
        public static void Modal(string unparsed)
        {
            ModalForm modalForm = new ModalForm();
            if (DialogResult.OK == modalForm.ShowDialog())
                MessageBox.Show(modalForm.tbValue.Text.ToString());
        }
        public static void TopLevel(string unparsed)
        {
            MultiScaleCopyForm multiCopyForm = new MultiScaleCopyForm();
            multiCopyForm.AttachAsTopLevelForm(MyAddin.s_addin, false);
            multiCopyForm.Show();
        }
        public static void ToolSettings(string unparsed)
        {
            NoteCoordForm coordForm = new NoteCoordForm();
            //coordForm.AttachToToolSettings(MyAddin.s_addin);
            coordForm.Show();
        }
    }
}

