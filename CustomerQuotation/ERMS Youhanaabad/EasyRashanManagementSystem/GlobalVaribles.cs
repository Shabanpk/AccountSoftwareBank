using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net.NetworkInformation;
using System.Drawing;


namespace EasyRashanManagementSystem
{
    public static class GlobalVaribles
    {
        public static int UserID = 0;
        public static string UserName = "";
        public static string UserStatus = "";
        public static string FormName = "";
        public static string ProjectName = "Karwan Management System";
        public static string DeveloperName = "Fahad Mirza";
        public static string OrganizationName = "FS-It Solutions";
        public static string BranchName = "Youhanaabad Branch";
        public static string abc = "Rashan Ghar Youhanaabad Branch";
        public static string BranchHeadName = "Saghar Farman";
        public static string CompanyName = "RASHAN GHAR";
        public static string Address = " 19 J BLOCK MAIN YAHONAABAD LAHORE " +
                                       " Phone # 0301-4049945, 0300-4929348";
        public static string OnlyAddress = " YouhanaAbad Lahore.";
        public static string OrgPhoneNo = "Phone # 0300-4929348";
        public static string ConfirmationMsg = "Are you sure you want to close this form?";
        public static string StyleID;
        public static string Style;
        public static DataTable dtMainMenu = new DataTable();
        static DateTime dtNowFormat = DateTime.Now;
        public static DataTable dtSetUser = new DataTable();
        public static string IpAddress = "";
        public static string SoftwareVersion = "v.1.1";
        public static DataTable dtGlobal = new DataTable();

        public static string DateFormat()
        {
            string str = dtNowFormat.ToString("dd-MM-yyyy");
            return str;
        }

        public static bool PingHost(string NameHost)
        {
            bool IsPing = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(NameHost);
                IsPing = reply.Status == IPStatus.Success;
            }
            catch (PingException ex)
            {
                IsPing = false;
            }
            return IsPing;
        }

        #region MainScreen

        public static Color MainPnlBackHoverColor = ColorTranslator.FromHtml("#E2DDC7");
        public static Color MainPnlBackLeaveColor = Color.White;

        #endregion

        #region Panel Header

        public static Color PanelHeader = ColorTranslator.FromHtml("#002D40");
        public static Color lblheaderforeColor = Color.White;

        #endregion

        #region  ButtonClose

        public static Color btnCloseBackColor = ColorTranslator.FromHtml("#002D40");
        public static int btnCloseBorder = 0;
        
        #endregion

        #region Form

        public static Color FormColor = ColorTranslator.FromHtml("#ECEEF7");
        #endregion

        #region ButtonGeneral

        public static Color btnBackColor = ColorTranslator.FromHtml("#002D40");
        public static Color btnForeColor = Color.White;
        public static Font btnFontStyle = new System.Drawing.Font("Open Sans", 9, FontStyle.Bold); 
           
        #endregion

        #region PanelFooter

        public static Color PanelFooter = ColorTranslator.FromHtml("#DEE1EE");

        #endregion

        #region FormBorderColor

        public static Color BorderColor = ColorTranslator.FromHtml("#002D40");

        #endregion

    }
}
