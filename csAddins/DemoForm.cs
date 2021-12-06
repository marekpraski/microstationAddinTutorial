using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using BCOM = Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation;

namespace csAddins
{
    class DemoForm
    {
        class NoteCoordClass : IPrimitiveCommandEvents
        {
            private BCOM.Application app = Utilities.ComApp;
            private NoteCoordForm myForm = new NoteCoordForm();
            private Point3d[] m_atPoints = new Point3d[3];
            private int m_nPoints = 0;

            #region metody spe³niaj¹ce interface IPrimitiveCommandEvents
            /// actions after selecting another tool in Microstation
            public void Cleanup()
            {
                myForm.DetachFromMicroStation();
            }
            ///  User-supplied datapoint.
            /// point: Where the user clicked.
            /// view: User clicked in this view.
            public void DataPoint(ref Point3d Point, BCOM.View View)
            {
                if (m_nPoints == 0)
                {
                    app.CommandState.StartDynamics();
                    m_atPoints[0] = Point;
                    m_nPoints = 1;
                    app.ShowPrompt("Identify note position");
                }
                else
                {
                    Dynamics(ref Point, View, MsdDrawingMode.Normal);
                    Reset();
                }
            }
            /// Called by the state machine for each mouse move event.
            /// point: Current cursor location.
            /// view: Cursor is in this view.
            /// drawMode: Draw mode supplied by the drawing engine.
            public void Dynamics(ref Point3d Point, BCOM.View View, MsdDrawingMode DrawMode)
            {
                if (m_nPoints != 1)
                    return;
                string[] txtStr = new string[2];
                Point3d[] txtPts = new Point3d[2];
                Element[] elems = new Element[3]; m_atPoints[1] = Point;
                txtStr[0] = (myForm.rbEN.Checked ? "E=" : "X=") + m_atPoints[0].X.ToString("F2");
                txtStr[1] = (myForm.rbEN.Checked ? "N=" : "Y=") + m_atPoints[0].Y.ToString("F2");
                double txtLen = app.ActiveSettings.TextStyle.Width * Math.Max(txtStr[0].Length, txtStr[1].Length);
                double txtLineSpacing = app.ActiveSettings.TextStyle.Height;
                if (myForm.rbHorizontal.Checked)
                {
                    m_atPoints[2].X = m_atPoints[1].X + (m_atPoints[0].X > m_atPoints[1].X ? -txtLen : txtLen) * 1.2;
                    m_atPoints[2].Y = m_atPoints[1].Y;
                    txtPts[0].X = (m_atPoints[1].X + m_atPoints[2].X) / 2;
                    txtPts[0].Y = m_atPoints[1].Y + txtLineSpacing;
                    txtPts[1].X = txtPts[0].X;
                    txtPts[1].Y = m_atPoints[1].Y - txtLineSpacing;
                }
                else
                {
                    m_atPoints[2].X = m_atPoints[1].X;
                    m_atPoints[2].Y = m_atPoints[1].Y + (m_atPoints[0].Y > m_atPoints[1].Y ? -txtLen : txtLen) * 1.2;
                    txtPts[0].X = m_atPoints[1].X - txtLineSpacing;
                    txtPts[0].Y = (m_atPoints[1].Y + m_atPoints[2].Y) / 2;
                    txtPts[1].X = m_atPoints[1].X + txtLineSpacing;
                    txtPts[1].Y = txtPts[0].Y;
                }
                elems[0] = app.CreateLineElement1(null, ref m_atPoints);
                elems[0].LineStyle = app.ActiveDesignFile.LineStyles.Find("0", null);
                Matrix3d rMatrix = app.Matrix3dIdentity();
                for (int i = 1; i < 3; i++)
                {
                    elems[i] = app.CreateTextElement1(null, txtStr[i - 1], ref txtPts[i - 1], ref rMatrix);
                    elems[i].AsTextElement().TextStyle.Font = app.ActiveDesignFile.Fonts.Find(MsdFontType.MicroStation, "ENGINEERING", null);
                    elems[i].AsTextElement().TextStyle.Justification = MsdTextJustification.CenterCenter;
                    if (myForm.rbVertical.Checked)
                        elems[i].RotateAboutZ(ref txtPts[i - 1], Math.PI / 2);
                }
                CellElement elemCell = app.CreateCellElement1("NoteCoordCell", ref elems, ref m_atPoints[0], false);
                elemCell.Redraw(DrawMode);
                if (MsdDrawingMode.Normal == DrawMode)
                    app.ActiveModelReference.AddElement(elemCell);
            }
            /// Keyin: User-supplied key-in while command is active.
            public void Keyin(string Keyin)
            {
            }
            /// Restart this tool.
            public void Reset()
            {
                m_nPoints = 0;
                app.CommandState.StartPrimitive(this, false);
            }
            /// Start this command.
            public void Start()
            {
                //myForm.AttachToToolSettings(MyAddin.s_addin);
                //myForm.Show();
                app.ShowCommand("Note Coordinate");
                app.ShowPrompt("Please identify a point");
                app.CommandState.EnableAccuSnap();
            }
            #endregion

        }


        class MultiScaleCopyClass : ILocateCommandEvents
        {
            #region metody implementuj¹ce interface ILocateCommandEvents
            private BCOM.Application app = Utilities.ComApp;
            private MultiScaleCopyForm myForm = new MultiScaleCopyForm();
            public void Accept(Element Elem, ref Point3d Point, BCOM.View View)
            {
                Element newEl;
                Point3d orgPnt;
                double dScale = double.Parse(myForm.tbScale.Text);
                Point3d offsetPnt = app.Point3dFromXYZ(double.Parse(myForm.tbXOffset.Text),
                                                       double.Parse(myForm.tbYOffset.Text),
                                                       double.Parse(myForm.tbZOffset.Text));
                for (int i = 0; i < int.Parse(myForm.tbCopies.Text); i++)
                {
                    newEl = app.ActiveModelReference.CopyElement(Elem, null);
                    newEl.Move(ref offsetPnt);
                    orgPnt.X = (newEl.Range.Low.X + newEl.Range.High.X) * 0.5;
                    orgPnt.Y = (newEl.Range.Low.Y + newEl.Range.High.Y) * 0.5;
                    orgPnt.Z = (newEl.Range.Low.Z + newEl.Range.High.Z) * 0.5;
                    newEl.ScaleAll(ref orgPnt, dScale, dScale, dScale);
                    newEl.Redraw(MsdDrawingMode.Normal);
                    Elem = newEl.Clone();
                }
            }
            public void Cleanup()
            {
                myForm.DetachFromMicroStation();
            }
            public void Dynamics(ref Point3d Point, BCOM.View View, MsdDrawingMode DrawMode)
            {
            }
            public void LocateFailed()
            {
                app.CommandState.StartLocate(this);
            }
            public void LocateFilter(Element Element, ref Point3d Point, ref bool Accepted)
            {
            }
            public void LocateReset()
            {
            }
            public void Start()
            {
                myForm.AttachToToolSettings(MyAddin.s_addin);
                myForm.Show();
                app.ShowCommand("MultiScaleCopy");
                app.ShowPrompt("Please identify an element");
                app.CommandState.EnableAccuSnap();
            }

            #endregion
        }

        public static void displayMessage(string unparsed)
        {
            MessageBox.Show("hi!");
        }


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
            Utilities.ComApp.CommandState.StartLocate(new MultiScaleCopyClass());
        }
        public static void ToolSettings(string unparsed)
        {
            BCOM.Application app = Utilities.ComApp;
            if (app.ActiveModelReference.Is3D)
            {
                MessageBox.Show("This tool can only work in 2D model");
                return;
            }
            app.ActiveSettings.AnnotationScaleEnabled = false;
            app.CommandState.StartPrimitive(new NoteCoordClass(), false);
        }

// ----------------------------------------------------------------------------------------------------------

        public class LevelChangedClass : ILevelChangeEvents
        {
            public void LevelChanged(MsdLevelChangeType ChangeType, Level TheLevel, ModelReference TheModel)
            {
                if (MsdLevelChangeType.AfterCreate == ChangeType ||
                    MsdLevelChangeType.AfterDelete == ChangeType ||
                    MsdLevelChangeType.ChangeName == ChangeType)
                    PopulateLevelList();
            }
            public static void MyAddin_NewDesignFileEvent
                               (AddIn sender, AddIn.NewDesignFileEventArgs eventArgs)
            {
                if (AddIn.NewDesignFileEventArgs.When.AfterDesignFileOpen == eventArgs.WhenCode)
                    PopulateLevelList();
            }
            private static void PopulateLevelList()
            {
                LevelChangedForm myLevelChangedForm = null;
                foreach (Form myForm in System.Windows.Forms.Application.OpenForms)
                    if ("LevelChangedForm" == myForm.Name)
                    {
                        myLevelChangedForm = (LevelChangedForm)myForm;
                        break;
                    }
                if (null != myLevelChangedForm)
                {
                    myLevelChangedForm.listBox1.Items.Clear();
                    foreach (Level myLvl in Utilities.ComApp.ActiveDesignFile.Levels)
                        myLevelChangedForm.listBox1.Items.Add(myLvl.Name);
                }
            }
        }

        public static LevelChangedClass myLevelChanged = null;
        public static AddIn.NewDesignFileEventHandler myNewDGNHandler = null;
        private static LevelChangedForm myLevelForm = null;
        public static void LevelChanged(string unparsed)
        {
            if (null == myLevelForm || myLevelForm.IsDisposed)
            {
                myLevelForm = new LevelChangedForm();
                myLevelForm.AttachAsTopLevelForm(MyAddin.s_addin, false);
                myLevelForm.Show(); myLevelChanged = new LevelChangedClass();
                Utilities.ComApp.AddLevelChangeEventsHandler(myLevelChanged); myNewDGNHandler = new AddIn.NewDesignFileEventHandler
                         (LevelChangedClass.MyAddin_NewDesignFileEvent);
                MyAddin.s_addin.NewDesignFileEvent += myNewDGNHandler;
            }
            else
                myLevelForm.Activate();
        }
    }
}

