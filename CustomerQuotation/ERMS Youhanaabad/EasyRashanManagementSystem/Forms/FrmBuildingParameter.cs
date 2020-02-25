using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KansaiProject.MainForm;
using DAL;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmBuildingParameter : Form
    {
        string ConnString = DataAccess.ConnString;
        private long CustomerID = 0L;
        int CustID = 0;
        public static int AssignVersionID = 0;

        #region Modifiers

        long MUnitID = 0;
        bool GetRecord = false;
        bool UpdateRecord = false;

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

        public FrmBuildingParameter()
        {
            InitializeComponent();
        }

        #region Functions

        private void GetMaxCustomerID()
        {
            //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(CustomerID),0)+ 1  As [CustomerID] From Def_Customer ", sqlconn);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MUnitID = long.Parse(dt.Rows[0]["CustomerID"].ToString());
            }
            //ClsGlobal NewMaxIDFunction = new ClsGlobal { ColName = "MUnitID", TableName = "MUnit", whereClause = "" };
            //GlobalFunctions NewMaxID = new GlobalFunctions();
            ////MUnitID = NewMaxID.MaxID(NewMaxIDFunction);
           // txtCustomerID.Text = MUnitID.ToString();
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
            dt.Dispose();
        }

        private void InsertBuildingDetails()
        {
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                string Qry = @"Insert into Trans_BuildingDetails(CustomerID,VersionID,BuildingArea,FrameType,Width,WidthModule,EndWall,WallGrits,Length,BaySpacing,EveHeight,HeightBrick,FFL,RoofSlope,BracingType,StromWater,BuildingShape,TypeEve,TypeGlabe,ExteriorGrid,FilledWelt,Price,Created_At,Created_Id,Modify_At,IsActive)
                                Values
                                (@CustomerID,@VersionID,@BuildingArea,@FrameType,@Width,@WidthModule,@EndWall,@WallGrits,@Length,@BaySpacing,@EveHeight,@HeightBrick,@FFL,@RoofSlope,@BracingType,@StromWater,@BuildingShape,@TypeEve,@TypeGlabe,@ExteriorGrid,@FilledWelt,@Price,@Created_At,@Created_Id,@Modify_At,@IsActive)";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand(Qry, sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.Text;
                command.CommandText = Qry;
                //this.CustomerID = int.Parse(DataAccess.GetMaxNO("CustomerID", "Def_Customer").ToString());
                // NewBEObj.ExpenseID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@CustomerID", int.Parse(cboReferenceNo.Value.ToString()));
                command.Parameters.AddWithValue("@VersionID",int.Parse(cboVersionName.Value.ToString()));
                command.Parameters.AddWithValue("@BuildingArea",txtBuldingArea.Text);
                command.Parameters.AddWithValue("@FrameType",txtFrametype.Text);
                command.Parameters.AddWithValue("@Width",txtWidth.Text);
                command.Parameters.AddWithValue("@WidthModule",txtWidthModule.Text);
                command.Parameters.AddWithValue("@EndWall",txtEndWall.Text);
                command.Parameters.AddWithValue("@WallGrits",txtWallGrits.Text);
                command.Parameters.AddWithValue("@Length",txtLength.Text);
                command.Parameters.AddWithValue("@BaySpacing",txtBySpacing.Text);
                command.Parameters.AddWithValue("@EveHeight",txtEveHeight.Text);
                command.Parameters.AddWithValue("@HeightBrick",txtHeightOfBrick.Text);
                command.Parameters.AddWithValue("@FFL",txtFFL.Text);
                command.Parameters.AddWithValue("@RoofSlope",txtRoofSlope.Text);
                command.Parameters.AddWithValue("@BracingType",txtBracingType.Text);
                command.Parameters.AddWithValue("@StromWater",txtStromWater.Text);
                command.Parameters.AddWithValue("@BuildingShape",txtBuildingShape.Text);
                command.Parameters.AddWithValue("@TypeEve",txtTypeOfEve.Text);
                command.Parameters.AddWithValue("@TypeGlabe",txtTypeGlabe.Text);
                command.Parameters.AddWithValue("@ExteriorGrid",txtExteriorRigid.Text);
                command.Parameters.AddWithValue("@FilledWelt",txtFilletWeld.Text);
                command.Parameters.AddWithValue("@Price",txtPrice.Text);
                command.Parameters.AddWithValue("@Created_At", DateTime.Now);
                command.Parameters.AddWithValue("@Created_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@Modify_At", (object)"1900-01-01");
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                string QryItem = @"Insert into BuildingDetailsHistory(CustomerID,VersionID,BuildingArea,FrameType,Width,WidthModule,EndWall,WallGrits,Length,BaySpacing,EveHeight,HeightBrick,FFL,RoofSlope,BracingType,StromWater,BuildingShape,TypeEve,TypeGlabe,ExteriorGrid,FilledWelt,Price,Created_At,Created_Id,Modify_At,IsActive)
                                Values
                                (@CustomerID,@VersionID,@BuildingArea,@FrameType,@Width,@WidthModule,@EndWall,@WallGrits,@Length,@BaySpacing,@EveHeight,@HeightBrick,@FFL,@RoofSlope,@BracingType,@StromWater,@BuildingShape,@TypeEve,@TypeGlabe,@ExteriorGrid,@FilledWelt,@Price,@Created_At,@Created_Id,@Modify_At,@IsActive)";
                command.CommandType = CommandType.Text;
                command.CommandText = QryItem;
                //this.CustomerID = int.Parse(DataAccess.GetMaxNO("CustomerID", "Def_Customer").ToString());
                // NewBEObj.ExpenseID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@CustomerID", int.Parse(cboReferenceNo.Value.ToString()));
                command.Parameters.AddWithValue("@VersionID", int.Parse(cboVersionName.Value.ToString()));
                command.Parameters.AddWithValue("@BuildingArea", txtBuldingArea.Text);
                command.Parameters.AddWithValue("@FrameType", txtFrametype.Text);
                command.Parameters.AddWithValue("@Width", txtWidth.Text);
                command.Parameters.AddWithValue("@WidthModule", txtWidthModule.Text);
                command.Parameters.AddWithValue("@EndWall", txtEndWall.Text);
                command.Parameters.AddWithValue("@WallGrits", txtWallGrits.Text);
                command.Parameters.AddWithValue("@Length", txtLength.Text);
                command.Parameters.AddWithValue("@BaySpacing", txtBySpacing.Text);
                command.Parameters.AddWithValue("@EveHeight", txtEveHeight.Text);
                command.Parameters.AddWithValue("@HeightBrick", txtHeightOfBrick.Text);
                command.Parameters.AddWithValue("@FFL", txtFFL.Text);
                command.Parameters.AddWithValue("@RoofSlope", txtRoofSlope.Text);
                command.Parameters.AddWithValue("@BracingType", txtBracingType.Text);
                command.Parameters.AddWithValue("@StromWater", txtStromWater.Text);
                command.Parameters.AddWithValue("@BuildingShape", txtBuildingShape.Text);
                command.Parameters.AddWithValue("@TypeEve", txtTypeOfEve.Text);
                command.Parameters.AddWithValue("@TypeGlabe", txtTypeGlabe.Text);
                command.Parameters.AddWithValue("@ExteriorGrid", txtExteriorRigid.Text);
                command.Parameters.AddWithValue("@FilledWelt", txtFilletWeld.Text);
                command.Parameters.AddWithValue("@Price", txtPrice.Text);
                command.Parameters.AddWithValue("@Created_At", DateTime.Now);
                command.Parameters.AddWithValue("@Created_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@Modify_At", (object)"1900-01-01");
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                string QryCustomerIsAssign = @"UPDATE def_Customer
                                            SET IsAssignBuildingDetails = @IsAssignBuildingDetails
                                            WHERE CustomerID=@CustomerID";
                command.CommandType = CommandType.Text;
                command.CommandText = QryCustomerIsAssign;
                //this.CustomerID = int.Parse(DataAccess.GetMaxNO("CustomerID", "Def_Customer").ToString());
                // NewBEObj.ExpenseID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@CustomerID", int.Parse(cboReferenceNo.Value.ToString()));
                command.Parameters.AddWithValue("@IsAssignBuildingDetails",true);
                command.ExecuteNonQuery();

                sqlTransaction.Commit();
                sqlConnection.Close();
                command.Dispose();
                sqlTransaction.Dispose();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sqlTransaction.Rollback();
            }
            //try
            //{
            //    SqlConnection sqlconn = new SqlConnection(ConnString);
            //    sqlconn.Open();
            //    SqlCommand sqlcomm = new SqlCommand("InsertMUnit", sqlconn);
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@MUnitID", txtMUnitID.Text);
            //    sqlcomm.Parameters.AddWithValue("@MUnitName", txtMUnitName.Text);
            //    sqlcomm.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
            //    sqlcomm.ExecuteNonQuery();
            //    sqlconn.Close();
            //    sqlcomm.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
            ////BE_MUnit NewObjMUnit = new BE_MUnit { MUnitID = int.Parse(txtMUnitID.Text.ToString()), MUnitName = txtMUnitName.Text, IsActive = bool.Parse(chkIsActive.CheckedValue.ToString()) };
            ////BP_Munit NewBpObj = new BP_Munit();
            ////NewBpObj.InsertMUnit(NewObjMUnit);
        }

        private bool ValidateEntry()
        {
            if (cboReferenceNo.Value == null)
            {
                MessageBox.Show("Please Select the reference Number", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cboReferenceNo.Focus();
                return false;
            }
            else if (cboReferenceNo.Value == null)
            {
                MessageBox.Show("Please Select the Customer Name", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cboCustomerName.Focus();
                return false;
            }
            else if (txtBuldingArea.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please Enter Building Area", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtWidthModule.Focus();
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            UpdateRecord = false;
            txtBuldingArea.Text = "";
            txtFrametype.Text = "";
            txtWidth.Text = "";
            txtWidthModule.Text = "";
            txtEndWall.Text = "";
            txtWallGrits.Text = "";
            txtLength.Text = "";
            txtBySpacing.Text = "";
            txtEveHeight.Text = "";
            txtHeightOfBrick.Text = "";
            txtFFL.Text = "";
            txtRoofSlope.Text = "";
            txtBracingType.Text = "";
            txtStromWater.Text = "";
            txtBuildingShape.Text = "";
            txtTypeOfEve.Text = "";
            txtTypeGlabe.Text = "";
            txtExteriorRigid.Text = "";
            txtFilletWeld.Text = "";
            txtPrice.Text = "";
            //txtCustomerID.Text = "";
            btnOK.Enabled = true;
            btnEdit.Enabled = false;
            txtWidthModule.Focus();
            LoadReferenceNumber(cboReferenceNo);
            LoadVersionName(cboVersionName);
            cboReferenceNo.ReadOnly = false;
            cboCustomerName.ReadOnly = false;
            cboVersionName.ReadOnly = false;
        }

        //private bool GetDuplicate()
        //{
        //    SqlConnection sqlconn = new SqlConnection(ConnString);
        //    sqlconn.Open();
        //    SqlCommand sqlcomm = new SqlCommand("Select MUnitID From Munit Where MUnitName='" + txtMUnitName.Text + "'", sqlconn);
        //    DataTable dt = new DataTable();
        //    dt = GetDataTable(sqlcomm, sqlconn);
        //    if (dt.Rows.Count > 0)
        //    {
        //        GetRecord = true;
        //    }
        //    else
        //    {
        //        GetRecord = false;
        //    }
        //    //BE_MUnit NewBEObj = new BE_MUnit { MUnitName=txtMUnitName.Text.Trim()};
        //    //BP_Munit NEwBPObj = new BP_Munit();
        //    //GetRecord = NEwBPObj.DuplicateMUnit(NewBEObj);
        //    sqlconn.Close();
        //    sqlcomm.Dispose();
        //    dt.Dispose();
        //    return GetRecord;
        //}

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

        public void UpdateBuildingDetails()
        {
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                string Qry = @"Update Trans_BuildingDetails
                                set
                                VersionID=@VersionID,
                                BuildingArea=@BuildingArea,
                                FrameType=@FrameType,
                                Width=@Width,
                                WidthModule=@WidthModule,
                                EndWall=@EndWall,
                                WallGrits=@WallGrits,
                                Length=@Length,
                                BaySpacing=@BaySpacing,
                                EveHeight=@EveHeight,
                                HeightBrick=@HeightBrick,
                                FFL=@FFL,
                                RoofSlope=@RoofSlope,
                                BracingType=@BracingType,
                                StromWater=@StromWater,
                                BuildingShape=@BuildingShape,
                                TypeEve=@TypeEve,
                                TypeGlabe=@TypeGlabe,
                                ExteriorGrid=@ExteriorGrid,
                                FilledWelt=@FilledWelt,
                                Price=@Price,
                                Modify_At=@Modify_At,
                                Modify_Id=@Modify_Id,
                                IsActive=@IsActive
                                where CustomerID=@CustomerID";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand(Qry, sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.Text;
                command.CommandText = Qry;
                command.Parameters.AddWithValue("@CustomerID", int.Parse(cboReferenceNo.Value.ToString()));
                command.Parameters.AddWithValue("@VersionID", int.Parse(cboVersionName.Value.ToString()));
                command.Parameters.AddWithValue("@BuildingArea", txtBuldingArea.Text);
                command.Parameters.AddWithValue("@FrameType", txtFrametype.Text);
                command.Parameters.AddWithValue("@Width", txtWidth.Text);
                command.Parameters.AddWithValue("@WidthModule", txtWidthModule.Text);
                command.Parameters.AddWithValue("@EndWall", txtEndWall.Text);
                command.Parameters.AddWithValue("@WallGrits", txtWallGrits.Text);
                command.Parameters.AddWithValue("@Length", txtLength.Text);
                command.Parameters.AddWithValue("@BaySpacing", txtBySpacing.Text);
                command.Parameters.AddWithValue("@EveHeight", txtEveHeight.Text);
                command.Parameters.AddWithValue("@HeightBrick", txtHeightOfBrick.Text);
                command.Parameters.AddWithValue("@FFL", txtFFL.Text);
                command.Parameters.AddWithValue("@RoofSlope", txtRoofSlope.Text);
                command.Parameters.AddWithValue("@BracingType", txtBracingType.Text);
                command.Parameters.AddWithValue("@StromWater", txtStromWater.Text);
                command.Parameters.AddWithValue("@BuildingShape", txtBuildingShape.Text);
                command.Parameters.AddWithValue("@TypeEve", txtTypeOfEve.Text);
                command.Parameters.AddWithValue("@TypeGlabe", txtTypeGlabe.Text);
                command.Parameters.AddWithValue("@ExteriorGrid", txtExteriorRigid.Text);
                command.Parameters.AddWithValue("@FilledWelt", txtFilletWeld.Text);
                command.Parameters.AddWithValue("@Price", txtPrice.Text);
                command.Parameters.AddWithValue("@Modify_At", DateTime.Now);
                command.Parameters.AddWithValue("@Modify_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
                command.ExecuteNonQuery();
                command.Parameters.Clear();


                string QryItem = @"Insert into BuildingDetailsHistory(CustomerID,VersionID,BuildingArea,FrameType,Width,WidthModule,EndWall,WallGrits,Length,BaySpacing,EveHeight,HeightBrick,FFL,RoofSlope,BracingType,StromWater,BuildingShape,TypeEve,TypeGlabe,ExteriorGrid,FilledWelt,Price,Modify_At,IsActive)
                                Values
                                (@CustomerID,@VersionID,@BuildingArea,@FrameType,@Width,@WidthModule,@EndWall,@WallGrits,@Length,@BaySpacing,@EveHeight,@HeightBrick,@FFL,@RoofSlope,@BracingType,@StromWater,@BuildingShape,@TypeEve,@TypeGlabe,@ExteriorGrid,@FilledWelt,@Price,@Modify_At,@IsActive)";
                command.CommandType = CommandType.Text;
                command.CommandText = QryItem;
                command.Parameters.AddWithValue("@CustomerID", int.Parse(cboReferenceNo.Value.ToString()));
                command.Parameters.AddWithValue("@VersionID", int.Parse(cboVersionName.Value.ToString()));
                command.Parameters.AddWithValue("@BuildingArea", txtBuldingArea.Text);
                command.Parameters.AddWithValue("@FrameType", txtFrametype.Text);
                command.Parameters.AddWithValue("@Width", txtWidth.Text);
                command.Parameters.AddWithValue("@WidthModule", txtWidthModule.Text);
                command.Parameters.AddWithValue("@EndWall", txtEndWall.Text);
                command.Parameters.AddWithValue("@WallGrits", txtWallGrits.Text);
                command.Parameters.AddWithValue("@Length", txtLength.Text);
                command.Parameters.AddWithValue("@BaySpacing", txtBySpacing.Text);
                command.Parameters.AddWithValue("@EveHeight", txtEveHeight.Text);
                command.Parameters.AddWithValue("@HeightBrick", txtHeightOfBrick.Text);
                command.Parameters.AddWithValue("@FFL", txtFFL.Text);
                command.Parameters.AddWithValue("@RoofSlope", txtRoofSlope.Text);
                command.Parameters.AddWithValue("@BracingType", txtBracingType.Text);
                command.Parameters.AddWithValue("@StromWater", txtStromWater.Text);
                command.Parameters.AddWithValue("@BuildingShape", txtBuildingShape.Text);
                command.Parameters.AddWithValue("@TypeEve", txtTypeOfEve.Text);
                command.Parameters.AddWithValue("@TypeGlabe", txtTypeGlabe.Text);
                command.Parameters.AddWithValue("@ExteriorGrid", txtExteriorRigid.Text);
                command.Parameters.AddWithValue("@FilledWelt", txtFilletWeld.Text);
                command.Parameters.AddWithValue("@Price", txtPrice.Text);
                command.Parameters.AddWithValue("@Modify_At", DateTime.Now);
                //command.Parameters.AddWithValue("@Modify_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
                command.ExecuteNonQuery();


                sqlTransaction.Commit();
                sqlConnection.Close();
                command.Dispose();
                sqlTransaction.Dispose();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sqlTransaction.Rollback();
            }
        }

        public void FormDesign()
        {
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.ForeColor = GlobalVaribles.lblheaderforeColor;
            btnClose.BackColor = GlobalVaribles.btnCloseBackColor;
            btnClose.FlatAppearance.BorderSize = GlobalVaribles.btnCloseBorder;
            this.BackColor = GlobalVaribles.FormColor;
            btnCustomerID.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnCustomerID.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnCustomerID.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnCustomerID.Font = GlobalVaribles.btnFontStyle;
            btnOK.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnOK.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnOK.Font = GlobalVaribles.btnFontStyle;
            btnClear.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnClear.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnClear.Font = GlobalVaribles.btnFontStyle;
            btnEdit.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnEdit.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnEdit.Font = GlobalVaribles.btnFontStyle;
            PanelFooter.BackColor = GlobalVaribles.PanelFooter;
        }

        public void LoadReferenceNumber(UltraCombo cbo)
        {
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand("Select CustomerId,ReferenceNo From Def_Customer where IsActive=1 and IsAssignBuildingDetails=0 order by customerID desc", connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "CustomerID";
            cbo.DisplayMember = "ReferenceNo";
            cbo.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = 0;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public void LoadReferenceNumberEdit(UltraCombo cbo, long CustomerID)
        {
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand("Select CustomerId,ReferenceNo From Def_Customer where IsActive=1 and IsAssignBuildingDetails=1 and CustomerID='"+CustomerID+"'", connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            CustID = int.Parse(CustomerID.ToString());
            cbo.ValueMember = "CustomerID";
            cbo.DisplayMember = "ReferenceNo";
            cbo.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = CustomerID;
            cbo.ReadOnly = true;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public void LoadCustomerName(UltraCombo cbo, long CustomerID)
        {
            //            string Qry=@"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
            //inner join Trans_LoanAssign B ON A.PersonID=B.PersonID
            //where LoanPostingID='" + PersonID + "' and A.IsActive=1";
            string Qry = @"Select CustomerID,CustomerName from Def_Customer A where CustomerID='" + CustomerID+ "' ";
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand(Qry, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "CustomerID";
            cbo.DisplayMember = "CustomerName";
            cbo.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = CustomerID;
            //cbo.Value = 1;
            cbo.ReadOnly = true;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public void LoadCustomerNameEdit(UltraCombo cbo, long CustomerID)
        {
            //            string Qry=@"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
            //inner join Trans_LoanAssign B ON A.PersonID=B.PersonID
            //where LoanPostingID='" + PersonID + "' and A.IsActive=1";
            string Qry = @"Select CustomerID,CustomerName from Def_Customer A where CustomerID='" + CustomerID + "' ";
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand(Qry, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "CustomerID";
            cbo.DisplayMember = "CustomerName";
            cbo.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = 1;
            //cbo.ReadOnly = true;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public void LoadVersionName(UltraCombo cbo)
        {
            //            string Qry=@"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
            //inner join Trans_LoanAssign B ON A.PersonID=B.PersonID
            //where LoanPostingID='" + PersonID + "' and A.IsActive=1";
            string Qry = @"Select VersionID,VersionName from Def_Version where IsActive=1";
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand(Qry, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "VersionId";
            cbo.DisplayMember = "VersionName";
            cbo.DisplayLayout.Bands[0].Columns["VersionID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = 1;

            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        void VersionNameAssign(long CustomerID)
        {
            string Qry = @"Select VersionID from Trans_BuildingDetails A
                        inner join Def_Customer B on A.CustomerID=B.CustomerID
                        Where A.CustomerID='"+CustomerID+"'";
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand(Qry, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            AssignVersionID = Convert.ToInt32(dataTable.Rows[0]["VersionID"].ToString());
        }

        public void LoadVersionNameEdit(UltraCombo cbo,long CustomerID)
        {
            //            string Qry=@"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
            //inner join Trans_LoanAssign B ON A.PersonID=B.PersonID
            //where LoanPostingID='" + PersonID + "' and A.IsActive=1";
            string Qry = @"Select VersionID,VersionName from Def_Version where IsActive=1";
//            string Qry = @"Select A.VersionID,VersionName from Trans_BuildingDetails A
//                            inner join Def_Version B on A.VersionID=B.VersionID
//                            where CustomerID='"+CustomerID+"' and B.IsActive=1";

            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand(Qry, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "VersionId";
            cbo.DisplayMember = "VersionName";
            cbo.DisplayLayout.Bands[0].Columns["VersionID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            VersionNameAssign(CustomerID);
            cbo.Value = AssignVersionID;
            //cbo.ReadOnly = true;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        #endregion

        #region Events

        private void FrmBuildingParameter_Load(object sender, EventArgs e)
        {
            FormDesign();
            LoadReferenceNumber(cboReferenceNo);
          //  GetMaxCustomerID();
            LoadVersionName(cboVersionName);
            btnEdit.Enabled = false;
        }

        private void FrmBuildingParameter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void FrmBuildingParameter_Paint(object sender, PaintEventArgs e)
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

        private void cboPersonName_ValueChanged(object sender, EventArgs e)
        {
            CustID = Convert.ToInt32(cboReferenceNo.Value);
            LoadCustomerName(cboCustomerName, CustID);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!UpdateRecord)
            {
                if (ValidateEntry())
                {
                    InsertBuildingDetails();
                    MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    //if (GetDuplicate())
                    //{
                    //    MessageBox.Show("Duplicate Record", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    txtMUnitName.Focus();
                    //}
                    //else
                    //{
                    //    InsertMUnit();
                    //    MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    ClearForm();
                    //    GetMaxMUnitID();
                    //}
                }
            }
        }

        private void btnCustomerID_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                string GetQry = @"Select A.CustomerID,B.ReferenceNo,B.CustomerName,BuildingArea,FrameType,EndWall from Trans_BuildingDetails A
                                inner join Def_Customer B on A.CustomerID= B.CustomerID ";
                rValue = FrmGSearch.Show(GetQry, false, "All Building Parameters");
                if (rValue == null)
                    return;
                MUnitID = long.Parse(rValue.ToString());
               // CustID =Convert.ToInt32(MUnitID.ToString());
                DataTable dt = new DataTable();
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
                string Qry = @"Select A.CustomerID,ReferenceNo,CustomerName,VersionID,BuildingArea,FrameType,Width,WidthModule,EndWall,
                                WallGrits,Length,BaySpacing,EveHeight,HeightBrick,FFL,RoofSlope,BracingType,StromWater,BuildingShape,
                                TypeEve,TypeGlabe,ExteriorGrid,FilledWelt,Price,A.IsActive
                                from Trans_BuildingDetails A
                                inner join Def_Customer B on A.CustomerID =B.CustomerID
                                where A.CustomerID=@CustomerId";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                //SqlCommand sqlcomm = new SqlCommand("GetMUnitName", sqlconn);
                //sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@CustomerID", MUnitID);
                dt = GetDataTable(sqlcomm, sqlconn);

                if (dt.Rows.Count > 0)
                {
                    UpdateRecord = true;
                    GetRecord = true;
                    LoadReferenceNumberEdit(cboReferenceNo, MUnitID);
                  //  LoadCustomerNameEdit(cboCustomerName, MUnitID);
                    LoadVersionNameEdit(cboVersionName, MUnitID);

                    //cboReferenceNo.Text = dt.Rows[0]["ReferenceNo"].ToString();
                   //cboReferenceNo.Text = dt.Rows[0]["ReferenceNo"].ToString();
                   //cboCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    //cboVersionName.Value = int.Parse(dt.Rows[0]["VersionID"].ToString());
                    txtBuldingArea.Text = dt.Rows[0]["BuildingArea"].ToString();
                    txtFrametype.Text= dt.Rows[0]["FrameType"].ToString();
                    txtWidth.Text= dt.Rows[0]["Width"].ToString();
                    txtWidthModule.Text= dt.Rows[0]["WidthModule"].ToString();
                    txtEndWall.Text= dt.Rows[0]["EndWall"].ToString();
                    txtWallGrits.Text= dt.Rows[0]["WallGrits"].ToString();
                    txtLength.Text= dt.Rows[0]["Length"].ToString();
                    txtBySpacing.Text= dt.Rows[0]["BaySpacing"].ToString();
                    txtEveHeight.Text= dt.Rows[0]["EveHeight"].ToString();
                    txtHeightOfBrick.Text= dt.Rows[0]["HeightBrick"].ToString();
                    txtFFL.Text= dt.Rows[0]["FFL"].ToString();
                    txtRoofSlope.Text= dt.Rows[0]["RoofSlope"].ToString();
                    txtBracingType.Text= dt.Rows[0]["BracingType"].ToString();
                    txtStromWater.Text= dt.Rows[0]["StromWater"].ToString();
                    txtBuildingShape.Text= dt.Rows[0]["BuildingShape"].ToString();
                    txtTypeOfEve.Text= dt.Rows[0]["TypeEve"].ToString();
                    txtTypeGlabe.Text= dt.Rows[0]["TypeGlabe"].ToString();
                    txtExteriorRigid.Text= dt.Rows[0]["ExteriorGrid"].ToString();
                    txtFilletWeld.Text= dt.Rows[0]["FilledWelt"].ToString();
                    txtPrice.Text= dt.Rows[0]["Price"].ToString();
                    chkIsActive.CheckedValue = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                    btnEdit.Enabled = true;
                    btnOK.Enabled = false;

                }
                dt.Dispose();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                UpdateBuildingDetails();
                MessageBox.Show("Record Updated Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                btnEdit.Enabled = false;
                btnOK.Enabled = true;
            }
        }

        #endregion

    }
}
