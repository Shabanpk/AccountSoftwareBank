using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmGetWidowText : Form
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();
        [DllImport("user32.dll")]

        static extern int GetWindowText(int hWnd, StringBuilder text, int count);
        //Use SetWindowPos to make app alwaysontop

        private void GetActiveWindow()
        {
            const int nChars = 256;
            int handle = 0;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();
            GetWindowText(handle, Buff, nChars);
            //{

            textBox1.Text = Buff.ToString();
            textBox2.Text = handle.ToString();
            //    //this.captionWindowLabel.Text = Buff.ToString();
            //    //this.IDWindowLabel.Text = handle.ToString();
            //}
        }

        public FrmGetWidowText()
        {
            InitializeComponent();
        }

        private void FrmGetWidowText_Load(object sender, EventArgs e)
        {
            GetActiveWindow();
        }


    }
}
