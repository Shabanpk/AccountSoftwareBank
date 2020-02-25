using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using EasyRashanManagementSystem;

namespace KansaiProject.MainForm
{
    public partial class FrmConfirmationMessage : Form
    {

        #region Form Move Variables & Methods

        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool ReleaseCapture();
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        Form NewFrm = new Form();
        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        // drop shadow 
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00039000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        #endregion

        public FrmConfirmationMessage(string Msg, int Index,Form FrmName)
        {
            InitializeComponent();
            DisplayImage(Index);
            lblMessage.Text = Msg;
            NewFrm = FrmName;
        }

        #region Functions

        void DisplayImage(int PixIndex)
        {
            switch (PixIndex)
            {
                case 0:
                    {
                        pixBox.Image = EasyRashanManagementSystem.Properties.Resources.Tick3;
                        break;
                    }
                case 1:
                    {
                        pixBox.Image = EasyRashanManagementSystem.Properties.Resources.Error2;
                        break;
                    }
                case 2:
                    {
                        pixBox.Image = EasyRashanManagementSystem.Properties.Resources.Information;
                        break;
                    }
            }

        }

        #endregion

        private void FrmConfirmationMessage_Load(object sender, EventArgs e)
        {
            btnok.BackColor = ColorTranslator.FromHtml("#53a2b3");
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#75B4C1");
            btnok.FlatAppearance.MouseOverBackColor = col;
            btnok.FlatAppearance.BorderSize = 0;

            btnCancel.BackColor = ColorTranslator.FromHtml("#53a2b3");
            System.Drawing.Color cola = System.Drawing.ColorTranslator.FromHtml("#75B4C1");
            btnCancel.FlatAppearance.MouseOverBackColor = cola;
            btnCancel.FlatAppearance.BorderSize = 0;
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblConfirmation.BackColor = GlobalVaribles.PanelHeader;
            lblConfirmation.ForeColor = GlobalVaribles.lblheaderforeColor;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            NewFrm.Close();
            FormCollection fcForms = Application.OpenForms;
            foreach (Form fc in fcForms)
            {
                if (fc.Name != "FrmMain")
                {
                    fc.Focus(); 
                }
            }
            this.Close();
        }

    }
}
