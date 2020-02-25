using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using System.Runtime.InteropServices;
using KansaiProject.MainForm;
using System.Data.SqlClient;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmCustomerStatus : Form
    {
        string ConnString = DataAccess.ConnString;
        DataTable dt = new DataTable();
        int HandleGrid;
       

        #region Modifiers

        long MUnitID = 0;
        bool GetRecord = false;
        bool UpdateRecor = false;

        #endregion

        #region Form Move Variables & Methods

        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool ReleaseCapture();
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        #endregion

        public FrmCustomerStatus()
        {
            InitializeComponent();
        }

        #region Functions

        private static DataTable GetDataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {
            //string a = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            //string a = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            //string a = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456"; 
            //sqlconn.ConnectionString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";
            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void FormDesign()
        {
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.ForeColor = GlobalVaribles.lblheaderforeColor;
            btnClose.BackColor = GlobalVaribles.btnCloseBackColor;
            btnClose.FlatAppearance.BorderSize = GlobalVaribles.btnCloseBorder;
            this.BackColor = GlobalVaribles.FormColor;
            //btnEdit.Appearance.BackColor = GlobalVaribles.btnBackColor;
            //btnEdit.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            //btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            //btnEdit.Font = GlobalVaribles.btnFontStyle;

            btnOK.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnOK.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnOK.Font = GlobalVaribles.btnFontStyle;

            btnClear.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnClear.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnClear.Font = GlobalVaribles.btnFontStyle;
            //btnEdit.Appearance.BackColor = GlobalVaribles.btnBackColor;
            //btnEdit.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            //btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            //btnEdit.Font = GlobalVaribles.btnFontStyle;
           
            PanelFooter.BackColor = GlobalVaribles.PanelFooter;
        }

        void LoadDataGridCustomer(int ID)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            string Qry = @"Select A.CustomerID,ReferenceNo from Trans_BuildingDetails A
                                inner join Def_Customer B on A.CustomerID = B.CustomerID
                                where A.CustomerID=@CustomerID";
            string GetCount = @"Select count(*) as Value from trans_QuotationStatus where CustomerID=@CustomerID";

            string QryGetRecord = @"Select A.CustomerID,Proposal,Planning,Development,Approved from Trans_BuildingDetails A
                            inner join Def_Customer B on A.CustomerID = B.CustomerID
                            inner join Trans_QuotationStatus C on A.CustomerID= C.CustomerID
                            where A.CustomerID=@CustomerID";

            SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
            SqlCommand sqldetails = new SqlCommand(QryGetRecord, sqlconn);

            SqlCommand getcounttable = new SqlCommand(GetCount, sqlconn);
            sqlcomm.CommandType = CommandType.Text;
            sqldetails.CommandType = CommandType.Text;
            getcounttable.CommandType = CommandType.Text;
            sqlcomm.Parameters.AddWithValue("@CustomerId", MUnitID);
            sqldetails.Parameters.AddWithValue("@CustomerId", MUnitID);
            getcounttable.Parameters.AddWithValue("@CustomerId", MUnitID);
            dt = GetDataTable(sqlcomm, sqlconn);
            DataTable dtdetailss = new DataTable();
            dtdetailss = GetDataTable(sqldetails,sqlconn);
            DataTable dtcount = new DataTable();
            dtcount = GetDataTable(getcounttable, sqlconn);
            try
            {
                if (dt.Rows.Count > 0 && dt !=null)
            {
                if (HandleGrid == 0)
                {
                    //UpdateRecord = true;
                    GetRecord = true;
                    //  dataGrdStudent.Columns.Remove("CanView");
                    dataGrdQuotationStatus.DataSource = dt;
                    DataGridViewCheckBoxColumn CheckBoxProposal = new DataGridViewCheckBoxColumn();
                    CheckBoxProposal.HeaderText = "Proposal";
                    CheckBoxProposal.Name = "Proposal";
                    CheckBoxProposal.Width = 5;
                    CheckBoxProposal.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxProposal.DefaultCellStyle.ForeColor = Color.DimGray;
                    dataGrdQuotationStatus.Columns.Add(CheckBoxProposal);
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {
                        if (Convert.ToString(dtdetailss.Rows[0]["Proposal"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = true;
                            //dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = dtdetailss.Rows[0]["Proposal"].ToString();
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = false;
                    }
                    
                    //foreach (DataGridViewRow row in dataGrdQuotationStatus.Rows)
                    //{
                    //    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Proposal"];
                    //    if (Convert.ToString(dt.Rows[0]["Proposal"].ToString()) == "0")
                    //    {
                    //        chk.Value = chk.FalseValue;
                    //    }
                    //    else
                    //    {
                    //        chk.Value = chk.TrueValue;
                    //    }
                    //}
                        
                    //    //chk.Value = chk.FalseValue;
                    //   // dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = dt.Rows[0]["Proposal"].ToString();
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Proposal"].Value = dr["Proposal"];
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Planning"].Value = dr["Planning"];
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Development"].Value = dr["Development"];
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Approved"].Value = dr["Approved"];
                    //    //if (chk.Value == chk.TrueValue)
                    //    //{
                    //    //    chk.Value = chk.FalseValue;
                    //    //}
                    //    //else
                    //    //{
                    //    //    chk.Value = chk.TrueValue;
                    //    //}
                    //}


                    DataGridViewCheckBoxColumn CheckBoxPlanning = new DataGridViewCheckBoxColumn();
                    CheckBoxPlanning.HeaderText = "Planning";
                    CheckBoxPlanning.Name = "Planning";
                    CheckBoxPlanning.Width = 5;
                    CheckBoxPlanning.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxPlanning.DefaultCellStyle.ForeColor = Color.DimGray;
                    
                    dataGrdQuotationStatus.Columns.Add(CheckBoxPlanning);
                    //dataGrdQuotationStatus.Rows[0].Cells["Planning"].Value = dtdetailss.Rows[0]["Planning"].ToString();
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {
                        if (Convert.ToString(dtdetailss.Rows[0]["Planning"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Planning"].Value = true;
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Planning"].Value = false;
                    }
                    DataGridViewCheckBoxColumn CheckBoxDevelopment = new DataGridViewCheckBoxColumn();
                    CheckBoxDevelopment.HeaderText = "Development";
                    CheckBoxDevelopment.Name = "Development";
                    CheckBoxDevelopment.Width = 7;
                    CheckBoxDevelopment.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxDevelopment.DefaultCellStyle.ForeColor = Color.DimGray;
                    
                    dataGrdQuotationStatus.Columns.Add(CheckBoxDevelopment);
                    //dataGrdQuotationStatus.Rows[0].Cells["Development"].Value = dtdetailss.Rows[0]["Development"].ToString();
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {
                        if (Convert.ToString(dtdetailss.Rows[0]["Development"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Development"].Value = true;
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Development"].Value = false;
                    }
                    DataGridViewCheckBoxColumn CheckBoxApproved = new DataGridViewCheckBoxColumn();
                    CheckBoxApproved.HeaderText = "Approved";
                    CheckBoxApproved.Name = "Approved";
                    CheckBoxApproved.Width = 5;
                    CheckBoxApproved.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxApproved.DefaultCellStyle.ForeColor = Color.DimGray;
                    
                    dataGrdQuotationStatus.Columns.Add(CheckBoxApproved);
                 //   dataGrdQuotationStatus.Rows[0].Cells["Approved"].Value = dtdetailss.Rows[0]["Approved"].ToString();
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {

                        if (Convert.ToString(dtdetailss.Rows[0]["Approved"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Approved"].Value = true;
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Approved"].Value = false;
                    }
                    dataGrdQuotationStatus.Columns[0].Visible = false;
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
                    columnHeaderStyle.ForeColor = Color.Black;
                    dataGrdQuotationStatus.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                    //dataGrdStudent.AllowUserToAddRows = false;
                    //dataGrdStudent.ReadOnly = true;
                    dataGrdQuotationStatus.Columns[1].ReadOnly = true;

                    dataGrdQuotationStatus.Columns[1].Width = 202;
                    dataGrdQuotationStatus.Columns[2].Width = 90;
                    dataGrdQuotationStatus.Columns[3].Width = 90;
                    dataGrdQuotationStatus.Columns[4].Width = 110;
                    dataGrdQuotationStatus.Columns[5].Width = 90;

                    dataGrdQuotationStatus.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGrdQuotationStatus.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGrdQuotationStatus.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGrdQuotationStatus.EnableHeadersVisualStyles = false;
                    this.dataGrdQuotationStatus.GridColor = Color.DarkGray;
                    dataGrdQuotationStatus.ForeColor = Color.DimGray;
                    dataGrdQuotationStatus.EnableHeadersVisualStyles = false;
                    dataGrdQuotationStatus.ColumnHeadersHeight = 30;
                    dataGrdQuotationStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    dataGrdQuotationStatus.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[3].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[4].HeaderCell.Style.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[5].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    //dataGrdStudent.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGrdQuotationStatus.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[2].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[3].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[4].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[5].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    HandleGrid++;

                    //foreach (DataGridViewRow row in dataGrdQuotationStatus.Rows)
                    //{
                    //    if (Convert.ToBoolean(dt.Rows[0]["Planning"].ToString()) == false)
                    //    {
                    //        row.Cells["Proposal"].Value = false;
                    //    }
                    //    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
                    //    DataGridViewCheckBoxCell chkplanning = (DataGridViewCheckBoxCell)row.Cells["Planning"];
                    //    DataGridViewCheckBoxCell chkDevelopment = (DataGridViewCheckBoxCell)row.Cells["Development"];
                    //    DataGridViewCheckBoxCell chkApproved = (DataGridViewCheckBoxCell)row.Cells["Approved"];
                    //    if (Convert.ToString(dt.Rows[0]["Proposal"].ToString()) == "1")
                    //    {
                    //        chk.Value = chk.TrueValue;
                    //    }
                    //    else if (Convert.ToString(dt.Rows[0]["Planning"].ToString()) == "1")
                    //    {
                    //        chkplanning.Value = chk.TrueValue;
                    //    }
                    //    else if (Convert.ToString(dt.Rows[0]["Development"].ToString()) == "1")
                    //    {
                    //        chkDevelopment.Value = chk.TrueValue;
                    //    }
                    //    else if (Convert.ToString(dt.Rows[0]["Approved"].ToString()) == "1")
                    //    {
                    //        chkApproved.Value = chk.TrueValue;
                    //    }
                    //    //chk.Value = chk.FalseValue;
                    //    // dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = dt.Rows[0]["Proposal"].ToString();
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Proposal"].Value = dr["Proposal"];
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Planning"].Value = dr["Planning"];
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Development"].Value = dr["Development"];
                    //    //dataGrdQuotationStatus.Rows[DataGridRow].Cells["Approved"].Value = dr["Approved"];
                    //    //if (chk.Value == chk.TrueValue)
                    //    //{
                    //    //    chk.Value = chk.FalseValue;
                    //    //}
                    //    //else
                    //    //{
                    //    //    chk.Value = chk.TrueValue;
                    //    //}
                    //}

                }
                else
                {
                    //UpdateRecord = true;
                    GetRecord = true;
                    
                    dataGrdQuotationStatus.DataSource = dt;
                    dataGrdQuotationStatus.Columns.Remove("Proposal");
                    DataGridViewCheckBoxColumn CheckBoxProposal = new DataGridViewCheckBoxColumn();
                    CheckBoxProposal.HeaderText = "Proposal";
                    CheckBoxProposal.Name = "Proposal";
                    CheckBoxProposal.Width = 5;
                    CheckBoxProposal.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxProposal.DefaultCellStyle.ForeColor = Color.DimGray;
                    dataGrdQuotationStatus.Columns.Add(CheckBoxProposal);
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {
                        if (Convert.ToString(dtdetailss.Rows[0]["Proposal"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = true;
                            //dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = dtdetailss.Rows[0]["Proposal"].ToString();
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Proposal"].Value = false;
                    }

                    dataGrdQuotationStatus.Columns.Remove("Planning");
                    DataGridViewCheckBoxColumn CheckBoxPlanning = new DataGridViewCheckBoxColumn();
                    CheckBoxPlanning.HeaderText = "Planning";
                    CheckBoxPlanning.Name = "Planning";
                    CheckBoxPlanning.Width = 5;
                    CheckBoxPlanning.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxPlanning.DefaultCellStyle.ForeColor = Color.DimGray;

                    dataGrdQuotationStatus.Columns.Add(CheckBoxPlanning);
                    //dataGrdQuotationStatus.Rows[0].Cells["Planning"].Value = dtdetailss.Rows[0]["Planning"].ToString();
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {
                        if (Convert.ToString(dtdetailss.Rows[0]["Planning"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Planning"].Value = true;
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Planning"].Value = false;
                    }
                    dataGrdQuotationStatus.Columns.Remove("Development");
                    DataGridViewCheckBoxColumn CheckBoxDevelopment = new DataGridViewCheckBoxColumn();
                    CheckBoxDevelopment.HeaderText = "Development";
                    CheckBoxDevelopment.Name = "Development";
                    CheckBoxDevelopment.Width = 7;
                    CheckBoxDevelopment.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxDevelopment.DefaultCellStyle.ForeColor = Color.DimGray;

                    dataGrdQuotationStatus.Columns.Add(CheckBoxDevelopment);
                    //dataGrdQuotationStatus.Rows[0].Cells["Development"].Value = dtdetailss.Rows[0]["Development"].ToString();
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {
                        if (Convert.ToString(dtdetailss.Rows[0]["Development"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Development"].Value = true;
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Development"].Value = false;
                    }
                    dataGrdQuotationStatus.Columns.Remove("Approved");
                    DataGridViewCheckBoxColumn CheckBoxApproved = new DataGridViewCheckBoxColumn();
                    CheckBoxApproved.HeaderText = "Approved";
                    CheckBoxApproved.Name = "Approved";
                    CheckBoxApproved.Width = 5;
                    CheckBoxApproved.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBoxApproved.DefaultCellStyle.ForeColor = Color.DimGray;

                    dataGrdQuotationStatus.Columns.Add(CheckBoxApproved);
                  //  dataGrdQuotationStatus.Rows[0].Cells["Approved"].Value = dtdetailss.Rows[0]["Approved"].ToString();
                    if (Convert.ToInt32(dtcount.Rows[0]["Value"].ToString()) > 0)
                    {

                        if (Convert.ToString(dtdetailss.Rows[0]["Approved"].ToString()) == "1")
                        {
                            dataGrdQuotationStatus.Rows[0].Cells["Approved"].Value = true;
                        }
                    }
                    else
                    {
                        dataGrdQuotationStatus.Rows[0].Cells["Approved"].Value = false;
                    }
                    dataGrdQuotationStatus.Columns[0].Visible = false;
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
                    columnHeaderStyle.ForeColor = Color.Black;
                    dataGrdQuotationStatus.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                    //dataGrdStudent.AllowUserToAddRows = false;
                    //dataGrdStudent.ReadOnly = true;
                    dataGrdQuotationStatus.Columns[1].ReadOnly = true;

                    dataGrdQuotationStatus.Columns[1].Width = 202;
                    dataGrdQuotationStatus.Columns[2].Width = 90;
                    dataGrdQuotationStatus.Columns[3].Width = 90;
                    dataGrdQuotationStatus.Columns[4].Width = 110;
                    dataGrdQuotationStatus.Columns[5].Width = 90;

                    dataGrdQuotationStatus.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGrdQuotationStatus.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGrdQuotationStatus.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGrdQuotationStatus.EnableHeadersVisualStyles = false;
                    this.dataGrdQuotationStatus.GridColor = Color.DarkGray;
                    dataGrdQuotationStatus.ForeColor = Color.DimGray;
                    dataGrdQuotationStatus.EnableHeadersVisualStyles = false;
                    dataGrdQuotationStatus.ColumnHeadersHeight = 30;
                    dataGrdQuotationStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    dataGrdQuotationStatus.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[3].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[4].HeaderCell.Style.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[5].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    //dataGrdStudent.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGrdQuotationStatus.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[2].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[3].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[4].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdQuotationStatus.Columns[5].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    HandleGrid++;

                   
                   
                }
                btnOK.Enabled = true;
                     dt.Dispose();
            sqlcomm.Dispose();

            }
            }
            catch (Exception ex)
            {
            }
               
        }

        bool ValidateDataGrid()
        {
            bool IsValid = false;

            foreach (DataGridViewRow row in dataGrdQuotationStatus.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Proposal"].Value) == true)
                {
                    IsValid = true;

                }
                else if (Convert.ToBoolean(row.Cells["Planning"].Value) == true)
                {
                    IsValid = true;
                }
                else if (Convert.ToBoolean(row.Cells["Development"].Value) == true)
                {
                    IsValid = true;
                }
                else if (Convert.ToBoolean(row.Cells["Approved"].Value) == true)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }

        void ClearForm()
        {
            btnOK.Enabled = false;
            //HandleGrid++;
            if (HandleGrid > 0)
            {
                dataGrdQuotationStatus.DataSource = null;

                dataGrdQuotationStatus.Columns["Proposal"].Visible = false;
                dataGrdQuotationStatus.Columns["Planning"].Visible = false;
                dataGrdQuotationStatus.Columns["Development"].Visible = false;
                dataGrdQuotationStatus.Columns["Approved"].Visible = false;
                //dataGrdStudent.Columns.Remove("Proposal");
                //dataGrdStudent.Columns.Remove("Planning");
                //dataGrdStudent.Columns.Remove("Development");
                //dataGrdStudent.Columns.Remove("Approved");
            }
        }

        bool SaveRecord()
        {
            bool IsSave = false;
            try
            {

                DataTable dtQuotationStatus = new DataTable("dtStatus");
                dtQuotationStatus.Columns.Add("CustomerID", typeof(int));
                dtQuotationStatus.Columns.Add("ProposalID", typeof(int));
                dtQuotationStatus.Columns.Add("PlanningID", typeof(int));
                dtQuotationStatus.Columns.Add("DevelopmentID", typeof(int));
                dtQuotationStatus.Columns.Add("ApprovedID", typeof(int));
                int checkRowCheckbox = 0;

                DataRow drRow;
                foreach (DataGridViewRow dr in dataGrdQuotationStatus.Rows)
                {
                    drRow = dtQuotationStatus.NewRow();
                    drRow["CustomerID"] = MUnitID;
                    if (dr.Cells["Proposal"].Value == null || Convert.ToBoolean(dr.Cells["Proposal"].Value.ToString())==false)
                    {
                        drRow["ProposalID"] = 0;
                    }
                    else
                    {
                        drRow["ProposalID"] = dr.Cells["Proposal"].Value;
                        checkRowCheckbox++;
                    }

                    if (dr.Cells["Planning"].Value == null || Convert.ToBoolean(dr.Cells["Planning"].Value.ToString()) == false)
                    {
                        drRow["PlanningID"] = 0;
                    }
                    else
                    {
                        drRow["PlanningID"] = dr.Cells["Planning"].Value;
                        checkRowCheckbox++;
                    }
                    if (dr.Cells["Development"].Value == null || Convert.ToBoolean(dr.Cells["Development"].Value.ToString()) == false)
                    {
                        drRow["DevelopmentID"] = 0;
                    }
                    else
                    { 
                        drRow["DevelopmentID"] = dr.Cells["Development"].Value;
                        checkRowCheckbox++;
                    }
                    if (dr.Cells["Approved"].Value == null || Convert.ToBoolean(dr.Cells["Approved"].Value.ToString()) == false)
                    {
                        drRow["ApprovedID"] = 0;
                    }
                    else
                    { 
                         drRow["ApprovedID"] = dr.Cells["Approved"].Value;
                         checkRowCheckbox++;
                    }
                    

                    ////drRow["ProposalID"] = dr.Cells["Proposal"].Value ?? DBNull.Value;
                    //drRow["PlanningID"] = dr.Cells["Planning"].Value ?? DBNull.Value;
                    //drRow["DevelopmentID"] = dr.Cells["Development"].Value ?? DBNull.Value;
                    //drRow["ApprovedID"] = dr.Cells["Approved"].Value ?? DBNull.Value;

                    dtQuotationStatus.Rows.Add(drRow);
                }
                if (checkRowCheckbox == 1)
                {
                   
                        
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    sqlconn.Open();
                    string QryQuotation = @"insert into Trans_QuotationStatus(CustomerID,Proposal,Planning,Development,Approved,Created_At,Created_Id)
                                        Values
                                        (@CustomerID,@Proposal,@Planning,@Development,@Approved,@Created_At,@Created_Id)";
                    SqlCommand sqlcomm = new SqlCommand(QryQuotation, sqlconn);
                    sqlcomm.CommandType = CommandType.Text;
                    //sqlcomm.Parameters.AddWithValue("@MUnitID", MUnitID);
                    if (dtQuotationStatus.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtQuotationStatus.Rows)
                        {
                            sqlcomm.Parameters.AddWithValue("@CustomerID", dr["CustomerID"]);
                            sqlcomm.Parameters.AddWithValue("@Proposal", dr["ProposalID"]);
                            sqlcomm.Parameters.AddWithValue("@Planning", dr["PlanningID"]);
                            sqlcomm.Parameters.AddWithValue("@Development", dr["DevelopmentID"]);
                            sqlcomm.Parameters.AddWithValue("@Approved", dr["ApprovedID"]);
                            sqlcomm.Parameters.AddWithValue("@Created_At", DateTime.Now);
                            sqlcomm.Parameters.AddWithValue("@Created_Id", GlobalVaribles.UserID);
                            sqlcomm.ExecuteNonQuery();
                            sqlcomm.Parameters.Clear();
                            IsSave = true;
                            //dtQuotationStatus = new DataTable("status");
                            dtQuotationStatus = new DataTable("status");

                        }
                    }
                    // sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();
                    sqlcomm.Dispose();
                    
                }
                else
                {
                    MessageBox.Show("Please Select One Department", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                    IsSave = false;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            return IsSave;
        }

        bool UpdateRecord()
        {
           
            bool isUpdate = false;
            try 
	            {
                    DataTable dtQuotationStatus = new DataTable("dtStatus");
                    dtQuotationStatus.Columns.Add("CustomerID", typeof(int));
                    dtQuotationStatus.Columns.Add("ProposalID", typeof(int));
                    dtQuotationStatus.Columns.Add("PlanningID", typeof(int));
                    dtQuotationStatus.Columns.Add("DevelopmentID", typeof(int));
                    dtQuotationStatus.Columns.Add("ApprovedID", typeof(int));
                    int checkRowCheckbox = 0;

                    DataRow drRow;
                    foreach (DataGridViewRow dr in dataGrdQuotationStatus.Rows)
                    {
                        drRow = dtQuotationStatus.NewRow();
                        drRow["CustomerID"] = MUnitID;
                        if (dr.Cells["Proposal"].Value == null || Convert.ToBoolean(dr.Cells["Proposal"].Value.ToString()) == false)
                        {
                            drRow["ProposalID"] = 0;
                        }
                        else
                        {
                            drRow["ProposalID"] = dr.Cells["Proposal"].Value;
                            checkRowCheckbox++;
                        }

                        if (dr.Cells["Planning"].Value == null || Convert.ToBoolean(dr.Cells["Planning"].Value.ToString()) == false)
                        {
                            drRow["PlanningID"] = 0;
                        }
                        else
                        {
                            drRow["PlanningID"] = dr.Cells["Planning"].Value;
                            checkRowCheckbox++;
                        }
                        if (dr.Cells["Development"].Value == null || Convert.ToBoolean(dr.Cells["Development"].Value.ToString()) == false)
                        {
                            drRow["DevelopmentID"] = 0;
                        }
                        else
                        {
                            drRow["DevelopmentID"] = dr.Cells["Development"].Value;
                            checkRowCheckbox++;
                        }
                        if (dr.Cells["Approved"].Value == null || Convert.ToBoolean(dr.Cells["Approved"].Value.ToString()) == false)
                        {
                            drRow["ApprovedID"] = 0;
                        }
                        else
                        {
                            drRow["ApprovedID"] = dr.Cells["Approved"].Value;
                            checkRowCheckbox++;
                        }

                        dtQuotationStatus.Rows.Add(drRow);
                    }

                    if (checkRowCheckbox == 1)
                    {
                        SqlConnection sqlconn = new SqlConnection(ConnString);
                        sqlconn.Open();
                        string QryQuotation = @"update Trans_QuotationStatus
                                    set
                                    CustomerID=@customerID,
                                    Proposal=@Proposal,
                                    Planning=@Planning,	
                                    Development=@Development,
                                    Approved=@Approved,
                                    Created_At=@Created_At,
                                    Created_Id=@Created_Id
                                    where CustomerID=@CustomerID";
                        SqlCommand sqlcomm = new SqlCommand(QryQuotation, sqlconn);
                        sqlcomm.CommandType = CommandType.Text;
                        //sqlcomm.Parameters.AddWithValue("@MUnitID", MUnitID);
                        if (dtQuotationStatus.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtQuotationStatus.Rows)
                            {
                                sqlcomm.Parameters.AddWithValue("@CustomerID", dr["CustomerID"]);
                                sqlcomm.Parameters.AddWithValue("@Proposal", dr["ProposalID"]);
                                sqlcomm.Parameters.AddWithValue("@Planning", dr["PlanningID"]);
                                sqlcomm.Parameters.AddWithValue("@Development", dr["DevelopmentID"]);
                                sqlcomm.Parameters.AddWithValue("@Approved", dr["ApprovedID"]);
                                sqlcomm.Parameters.AddWithValue("@Created_At", DateTime.Now);
                                sqlcomm.Parameters.AddWithValue("@Created_Id", GlobalVaribles.UserID);
                                sqlcomm.ExecuteNonQuery();
                                sqlcomm.Parameters.Clear();
                                isUpdate = true;
                            }
                        }
                        // sqlcomm.ExecuteNonQuery();
                        sqlconn.Close();
                        sqlcomm.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Please Select One Department", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //return false;
                       isUpdate = false;
                    }
                  
                    
	            }
	            catch (Exception ex)
	            {
	            }
            return isUpdate;
        }

        bool CheckCustomerIDExistx(int CustomerID)
        { 
            bool IsCheck = false;
            string Qry = @"select count(*) as Value from Trans_QuotationStatus where CustomerID=@CustomerID";
            DataTable dt = new DataTable();
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.Parameters.AddWithValue("@CustomerID", CustomerID);
            dt = GetDataTable(sqlcomm, sqlconn);
            if (dt.Rows.Count > 0 && dt != null)
            {
                if (Convert.ToInt32(dt.Rows[0]["Value"].ToString()) >0 )
                {
                    IsCheck = true;
                }
            }
            return IsCheck;
        }

        #endregion

        #region Events

        private void FrmCustomerStatus_Load(object sender, EventArgs e)
        {
            FormDesign();
            HandleGrid = 0;
            btnOK.Enabled = false;
            //dataGrdStudent.Rows.Clear();
            //dataGrdStudent.Refresh();
        }

        private void FrmCustomerStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void FrmCustomerStatus_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(GlobalVaribles.BorderColor);
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {

                rValue = FrmGSearch.Show(@"Select A.CustomerID,ReferenceNo,CustomerName,PhoneNo,FrameType from Trans_BuildingDetails A
                                        inner join Def_Customer B on A.CustomerID=B.CustomerID
                                        where IsAssignBuildingDetails=1", false, "All Customers");
                if (rValue == null)
                    return;
                MUnitID = long.Parse(rValue.ToString());
                LoadDataGridCustomer(int.Parse(MUnitID.ToString()));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateDataGrid())
            {
                if (CheckCustomerIDExistx(Convert.ToInt32(MUnitID)))
                {
                    //update Record
                    if (UpdateRecord())
                    {
                        MessageBox.Show("Record updated Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        HandleGrid++;
                    }
                }
                else
                {
                    if (SaveRecord())
                    {
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        HandleGrid++;
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Please Select one Department", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //HandleGrid++;
            if (HandleGrid > 0)
            {
                dataGrdQuotationStatus.DataSource = null;

                dataGrdQuotationStatus.Columns["Proposal"].Visible = false;
                dataGrdQuotationStatus.Columns["Planning"].Visible = false;
                dataGrdQuotationStatus.Columns["Development"].Visible = false;
                dataGrdQuotationStatus.Columns["Approved"].Visible = false;
                //dataGrdStudent.Columns.Remove("Proposal");
                //dataGrdStudent.Columns.Remove("Planning");
                //dataGrdStudent.Columns.Remove("Development");
                //dataGrdStudent.Columns.Remove("Approved");
            }

          //  FrmCustomerStatus_Load(this, new EventArgs());
        }

        #endregion

    }
}
