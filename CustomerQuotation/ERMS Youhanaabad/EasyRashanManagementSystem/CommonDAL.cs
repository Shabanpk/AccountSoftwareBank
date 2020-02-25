using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessEntities;
using BusinessProcessObjects;
using DAL;
using System.Data.SqlClient;
using VersatileEducationSystem;
using CrystalDecisions.CrystalReports.Engine;


namespace EasyRashanManagementSystem
{
    public class CommonDAL
    {
        public static void ShowReport(object rpt, string ReportCaption)
        {
            FrmReportCont frmRpt = new FrmReportCont();
            frmRpt.crvReports.ReportSource = rpt;
            frmRpt.crvReports.Refresh();
            frmRpt.Text = ReportCaption;
            frmRpt.ShowDialog();
        }

        #region Form Related Functions

        public static void CenterForm(Form frm)
        {
            Rectangle R = new Rectangle();
            R = Screen.PrimaryScreen.Bounds;
            frm.Left = (R.Width / 2) - (frm.Width / 2);
            frm.Top = ((R.Height / 2) - (frm.Height / 2)) - 50;

        }

        public static void CenterForm(Form frmChild, Form frmMDI)
        {
            frmChild.Left = (frmMDI.Width / 2) - (frmChild.Width / 2);
            frmChild.Top = ((frmMDI.Height / 2) - (frmChild.Height / 2)) - 50;

        }

        public static void ShowForm(Form frmChild, Form frmMDI, bool isOnlyOne, bool isInCenter)
        {
            if (isOnlyOne)
            {
                foreach (Form frm in Application.OpenForms)
                    if (frm.Name.Equals(frmChild.Name))
                    {
                        frm.BringToFront();
                        frm.Focus();
                        return;
                    }
            }
            if (isInCenter)
                CenterForm(frmChild, frmMDI);

            frmChild.MdiParent = frmMDI;
            frmChild.Show();
        }

        public static void ShowForm(Form f, bool isOnlyOne, bool isInCenter)
        {
            if (isOnlyOne)
            {
                foreach (Form frm in Application.OpenForms)
                    if (frm.Name.Equals(f.Name))
                    {
                        frm.BringToFront();
                        frm.Focus();
                        return;
                    }
            }

            f.Show();
            if (isInCenter)
                CenterForm(f);
            return;

        }

        public static void ShowmainForm(Form form)
        {
            Rectangle rect = new Rectangle();
            rect = Screen.PrimaryScreen.WorkingArea;
            form.Left = rect.Left;
            form.Top = rect.Top;
            form.Size = new System.Drawing.Size(rect.Size.Width, rect.Size.Height);
            form.Refresh();

        }

        #endregion

        #region Global Functions

        public static string GetDate(DateTime dt)
        {
            string strDate = string.Empty;
            DateTime dtDate = dt.AddMonths(-1);
            int strDay = dtDate.Day;
            int strMonth = dtDate.Month;
            int strYear = dtDate.Year;
            strDate = strYear + "-" + strMonth + "-" + strDay;
            return strDate;
        }

        #endregion

    }
}
