using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EasyRashanManagementSystem.Main_Form;
using EasyRashanManagementSystem.Forms;

namespace EasyRashanManagementSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmLogin());
            Form fc = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmMainScreen frmMain = new FrmMainScreen();
                frmMain.IsMdiContainer = true;
                frmMain.ShowDialog();
            }
        }
    }
}
