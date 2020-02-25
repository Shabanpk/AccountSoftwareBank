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
using KansaiProject.MainForm;
using System.Runtime.InteropServices;

namespace EasyRashanManagementSystem
{
    public partial class FrmMeasuringUnit : Form
    {
        string ConnString = DataAccess.ConnString;

        public FrmMeasuringUnit()
        {
            InitializeComponent();
        }

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

        #region Functions

        private void GetMaxMUnitID()
        {
            //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(MUnitID),0)+ 1  As [MUnitID] From MUnit ", sqlconn);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MUnitID  = long.Parse(dt.Rows[0]["MUnitID"].ToString());
            }
            //ClsGlobal NewMaxIDFunction = new ClsGlobal { ColName = "MUnitID", TableName = "MUnit", whereClause = "" };
            //GlobalFunctions NewMaxID = new GlobalFunctions();
            ////MUnitID = NewMaxID.MaxID(NewMaxIDFunction);
            txtMUnitID.Text = MUnitID.ToString();
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
            dt.Dispose();
        }

        private void InsertMUnit()
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand("InsertMUnit", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@MUnitID", txtMUnitID.Text);
                sqlcomm.Parameters.AddWithValue("@MUnitName", txtMUnitName.Text);
                sqlcomm.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
                sqlcomm.ExecuteNonQuery();
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            //BE_MUnit NewObjMUnit = new BE_MUnit { MUnitID = int.Parse(txtMUnitID.Text.ToString()), MUnitName = txtMUnitName.Text, IsActive = bool.Parse(chkIsActive.CheckedValue.ToString()) };
            //BP_Munit NewBpObj = new BP_Munit();
            //NewBpObj.InsertMUnit(NewObjMUnit);
        }

        private bool ValidateEntry() 
        {
            if (txtMUnitName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please Enter Some MUnitName", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtMUnitName.Focus();
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            UpdateRecord = false;
            txtMUnitName.Text = "";
            txtMUnitID.Text = "";
            btnOK.Enabled = true;
            btnEdit.Enabled = false;
            txtMUnitName.Focus();
        }

        private bool GetDuplicate()
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select MUnitID From Munit Where MUnitName='" + txtMUnitName.Text + "'", sqlconn);
            DataTable dt = new DataTable();
            dt = GetDataTable(sqlcomm, sqlconn);
            if (dt.Rows.Count > 0)
            {
                GetRecord = true;
            }
            else
            {
                GetRecord = false;
            }
            //BE_MUnit NewBEObj = new BE_MUnit { MUnitName=txtMUnitName.Text.Trim()};
            //BP_Munit NEwBPObj = new BP_Munit();
            //GetRecord = NEwBPObj.DuplicateMUnit(NewBEObj);
            sqlconn.Close();
            sqlcomm.Dispose();
            dt.Dispose();
            return GetRecord;
        }

        public void UpdateMUnit()
        {
            string a = string.Empty;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("UpdateMUnitName", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@MUnitID", txtMUnitID.Text );
            sqlcomm.Parameters.AddWithValue("@MUnitName", txtMUnitName.Text );
            sqlcomm.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue );

            try
            {
                sqlconn.Open();
                sqlcomm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            //BE_MUnit NewBEObj = new BE_MUnit { MUnitID=int.Parse(MUnitID.ToString()),MUnitName=txtMUnitName.Text.Trim(),IsActive=bool.Parse(chkIsActive.CheckedValue.ToString()) };
            //BP_Munit NewBPObj = new BP_Munit();
            //NewBPObj.UPdateMUnit(NewBEObj);
        }

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

        private static DataTable GetDataTable(string qry, SqlConnection sqlconn)
        {
            //string a = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            //string a = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            string ConnString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";
            sqlconn.ConnectionString = ConnString ;
            SqlDataAdapter da = new SqlDataAdapter(qry, sqlconn);
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
            btnUntiID.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnUntiID.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnUntiID.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnUntiID.Font = GlobalVaribles.btnFontStyle;
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

        #endregion

        #region Events

        private void FrmMeasuringUnit_Load(object sender, EventArgs e)
        {
            FormDesign();
            GetMaxMUnitID();
            btnEdit.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!UpdateRecord)
            {
                if (ValidateEntry())
                {
                    if (GetDuplicate())
                    {
                        MessageBox.Show("Duplicate Record", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtMUnitName.Focus();
                    }
                    else
                    {
                        InsertMUnit();
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        GetMaxMUnitID();
                    }
                }
            }
        }

        private void btnUntiID_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                
                rValue = FrmGSearch.Show("Select MUnitID,MUnitName From MUnit", false, "All Measuring Units");
                if (rValue == null)
                    return;
                MUnitID = long.Parse(rValue.ToString());
                DataTable dt = new DataTable();
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand("GetMUnitName", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@MUnitID", MUnitID  );
                dt = GetDataTable(sqlcomm,sqlconn );
                if (dt.Rows.Count > 0)
                {
                    UpdateRecord = true;
                    GetRecord = true;
                    txtMUnitID.Text = dt.Rows[0]["MUnitID"].ToString();
                    txtMUnitName.Text = dt.Rows[0]["MUnitName"].ToString();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            GetMaxMUnitID();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            {
                if (ValidateEntry())
                {
                    UpdateMUnit();
                    MessageBox.Show("Record Updated Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    GetMaxMUnitID();
                    btnEdit.Enabled = false;
                    btnOK.Enabled = true;
                }
            }
        }

        private void FrmMeasuringUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMUnitName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only enter string and space and numeric
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FrmMeasuringUnit_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(GlobalVaribles.BorderColor);
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        #endregion
    }
}
