using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessEntities;
using BusinessProcessObjects;
using DAL;
using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using KansaiProject.MainForm;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;

namespace EasyRashanManagementSystem
{
    public partial class FrmOpeningBalance : Form
    {
        public FrmOpeningBalance()
        {
            InitializeComponent();
        }

        #region Modifiers

        long MaxOpbalID = 0;
        int ItemID;
        long OpBalID = 0;

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

        private void BackDateAuthorization()
        {
            if (GlobalVaribles.UserStatus == "FMNG")
            {

                dtpOpeningDate.Enabled = true;
            }
            else
            {
                dtpOpeningDate.Enabled = false;
            }
        }

        private void GetMaxID()
        {
            BP_OpeningBalance NewMaxBP = new BP_OpeningBalance();
            MaxOpbalID = NewMaxBP.GetMaxID();
            txtOpeningID.Text = MaxOpbalID.ToString();
        }

        private bool InsertRecord()
        {
            try
            {
                BE_OpeningBalance BEObjMaster = new BE_OpeningBalance();
                BEObjMaster.OpeningBalanceID = int.Parse(txtOpeningID.Text.ToString());
                BEObjMaster.OpeningDate = dtpOpeningDate.DateTime;
                BEObjMaster.Remarks = txtRemarks.Text;
                BP_OpeningBalance MasterBP = new BP_OpeningBalance();
                MasterBP.InsertOpeningBalanceM(BEObjMaster);
                BE_OpeningBalance BEDetObj = new BE_OpeningBalance();
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    BEDetObj.OpeningBalanceID = BEObjMaster.OpeningBalanceID;
                    BEDetObj.ItemID = int.Parse(grd.Rows[i].Cells["ItemID"].Value.ToString());
                    BEDetObj.ItemQty = double.Parse(grd.Rows[i].Cells["ItemQty"].Value.ToString());
                    BP_OpeningBalance NewBEOBj = new BP_OpeningBalance();
                    NewBEOBj.InsertOpeningBalanceD(BEDetObj);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,GlobalVaribles.ProjectName,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return false;
            }
        }

        private void LoadCombo()
        {
            //BP_OpeningBalance LoadComboBP = new BP_OpeningBalance();
            //LoadComboBP.LoadGroups(cboGroup);
        }

        private void ClearForm()
        {
            dtpOpeningDate.Value = DateTime.Now;
            txtRemarks.Text = "";
            //cboGroup.Value = 1;
            dsItems.Rows.Clear();
            //DuplicateItem = 0;
            dtpOpeningDate.Focus();
        }

        private bool ValidateOpBal()
        {
            if (grd.Rows.Count == 0)
            {
                MessageBox.Show("Please Enter Some items to Save");
                return false;
            }
            return true;
        }

        #endregion

        #region Events

        private void FrmOpeningBalance_Load(object sender, EventArgs e)
        {
            dtpOpeningDate.Value = DateTime.Now;
            GetMaxID();
            LoadCombo();
            BackDateAuthorization();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateOpBal())
                {
                    if (InsertRecord())
                    {
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        GetMaxID();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            object rvalue;
            try
            {
                //on request of saghar sab on 17-12-2016
                if (GlobalVaribles.UserStatus == "FMNG")
                {
                    rvalue = FrmGSearch.Show("Select ItemID,ItemName, ItemPrice From Items IT ", false, "All Items ");
                }
                else
                {
                    rvalue = FrmGSearch.Show("Select ItemID,ItemName From Items IT ", false, "All Items ");
                }
                
                if (rvalue == null)
                    return;
                ItemID = int.Parse(rvalue.ToString());
                DataTable dt = new DataTable();
                BP_OpeningBalance NewOpObj = new BP_OpeningBalance();
                dt = NewOpObj.GetItems(ItemID);
                //txtItems.Text = dt.Rows[0]["ItemName"].ToString() + " " + dt.Rows[0]["MUnitName"].ToString() + " " + dt.Rows[0]["ItemPrice"].ToString();
                if (GlobalVaribles.UserStatus != "FMNG")
                {
                    txtItems.Text = dt.Rows[0]["ItemName"].ToString() + " " + dt.Rows[0]["MUnitName"].ToString();
                }
                else
                {
                    txtItems.Text = dt.Rows[0]["ItemName"].ToString() + " " + dt.Rows[0]["MUnitName"].ToString() + " Purchase Price : " + dt.Rows[0]["ItemPrice"].ToString();
                }
                txtQty.Focus();
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnOpBalID_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                btnSave.Enabled = false;
                dsItems.Rows.Clear();
                rValue = FrmGSearch.Show("Select OpeningBalanceID,OpeningbalanceID,OpeningDate From OpeningBalanceM ", true, "All Opening Balance");
                if (rValue == null)
                    return;
                OpBalID = long.Parse(rValue.ToString());
                DataTable dt = new DataTable();
                BP_OpeningBalance NewBPObj = new BP_OpeningBalance();
                dt = NewBPObj.GetOpBalID(int.Parse(OpBalID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtOpeningID.Text = dt.Rows[0]["OpeningBalanceID"].ToString();
                    dtpOpeningDate.Value = dt.Rows[0]["OpeningDate"].ToString();
                    //cboGroup.Value = dt.Rows[0]["ItemGroupID"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridRow row = grd.Rows.Band.AddNew();
                        row.Cells["ItemID"].Value = dt.Rows[i]["ItemID"].ToString();
                        row.Cells["ItemName"].Value = dt.Rows[i]["ItemName"].ToString();
                        row.Cells["MUnitName"].Value = dt.Rows[i]["MUnitName"].ToString();
                        row.Cells["ItemPrice"].Value = dt.Rows[i]["ItemPrice"].ToString();
                        row.Cells["ItemQty"].Value = dt.Rows[i]["Qty"].ToString();
                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            GetMaxID();
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // // only enter integer
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                DataTable dt = new DataTable();
                BP_OpeningBalance NewBP = new BP_OpeningBalance();
                dt = NewBP.GetItems(ItemID);
                if (dt != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = grd.Rows.Band.AddNew();
                    row.Cells["ItemID"].Value = dt.Rows[0]["ItemID"].ToString();
                    row.Cells["ItemName"].Value = dt.Rows[0]["ItemName"].ToString();
                    row.Cells["MUnitName"].Value = dt.Rows[0]["MUnitName"].ToString();
                    row.Cells["ItemPrice"].Value = dt.Rows[0]["ItemPrice"].ToString();
                    row.Cells["ItemQty"].Value = txtQty.Value;
                }
                txtQty.Text = "";
                txtItems.Text = "";
                btnItems.Focus();
                dt.Dispose();
            }
        }

        private void btnCross_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmOpeningBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void grd_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // create Image Column
            // e.Layout.Bands[0].Columns.Add("Image").DataType = typeof(Image);

            // header properties set 
            e.Layout.Bands[0].ColHeaderLines = 1;
            e.Layout.Bands[0].Columns[0].Header.Appearance.ForeColor = Color.Black;
            e.Layout.Bands[0].Columns[1].Header.Appearance.ForeColor = Color.Black;
            e.Layout.Bands[0].Columns[2].Header.Appearance.ForeColor = Color.Black;
            e.Layout.Bands[0].Columns[3].Header.Appearance.ForeColor = Color.Black;
            e.Layout.Bands[0].Columns[4].Header.Appearance.ForeColor = Color.Black;
            e.Layout.Bands[0].Columns[0].Header.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            e.Layout.Bands[0].Columns[1].Header.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            e.Layout.Bands[0].Columns[2].Header.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            e.Layout.Bands[0].Columns[3].Header.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            e.Layout.Bands[0].Columns[4].Header.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            e.Layout.Bands[0].Columns[0].Header.Appearance.FontData.SizeInPoints = 11;
            e.Layout.Bands[0].Columns[1].Header.Appearance.FontData.SizeInPoints = 11;
            e.Layout.Bands[0].Columns[2].Header.Appearance.FontData.SizeInPoints = 11;
            e.Layout.Bands[0].Columns[3].Header.Appearance.FontData.SizeInPoints = 11;
            e.Layout.Bands[0].Columns[4].Header.Appearance.FontData.SizeInPoints = 11;

            // column width 
            //grdItems.DisplayLayout.Bands[0].Columns[4].Width = 66; // image product And Package 

            // header appearance
            e.Layout.Bands[0].Override.HeaderAppearance.BackGradientStyle = GradientStyle.None;
            e.Layout.Bands[0].Override.HeaderAppearance.BackHatchStyle = BackHatchStyle.None;
            e.Layout.Bands[0].Override.HeaderStyle = HeaderStyle.WindowsXPCommand;

            // header text Alignment 
            e.Layout.Bands[0].Columns[0].Header.Appearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[1].Header.Appearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[2].Header.Appearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[3].Header.Appearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[4].Header.Appearance.TextHAlign = HAlign.Center;

            // text alignment of cell 
            // horizontal coulmn center
            e.Layout.Bands[0].Columns[0].CellAppearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[1].CellAppearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[2].CellAppearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[3].CellAppearance.TextHAlign = HAlign.Center;
            e.Layout.Bands[0].Columns[4].CellAppearance.TextHAlign = HAlign.Center;

            // cell of appearence allow edit
            e.Layout.Bands[0].Columns[0].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns[3].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns[1].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns[2].CellActivation = Activation.NoEdit;
            e.Layout.Bands[0].Columns[4].CellActivation = Activation.NoEdit;

            // vertical coulmn wise middle
            e.Layout.Bands[0].Columns[0].CellAppearance.TextVAlign = VAlign.Middle;
            e.Layout.Bands[0].Columns[1].CellAppearance.TextVAlign = VAlign.Middle;
            e.Layout.Bands[0].Columns[2].CellAppearance.TextVAlign = VAlign.Middle;
            e.Layout.Bands[0].Columns[3].CellAppearance.TextVAlign = VAlign.Middle;
            e.Layout.Bands[0].Columns[4].CellAppearance.TextVAlign = VAlign.Middle;

            // cell click action on grid
            grd.DisplayLayout.Bands[0].Override.ActiveCellAppearance.TextHAlign = HAlign.Center;

            // grid selected row false
            grd.DisplayLayout.Override.ActiveRowAppearance.Reset();

            // row height
            this.grd.DisplayLayout.Override.DefaultRowHeight = 25;

            // header caption text
            e.Layout.Bands[0].Columns[0].Header.Caption = "ItemID";
            e.Layout.Bands[0].Columns[1].Header.Caption = "Item Name";
            e.Layout.Bands[0].Columns[2].Header.Caption = "MUnit Name";
            e.Layout.Bands[0].Columns[3].Header.Caption = "Item Price";
            e.Layout.Bands[0].Columns[3].Header.Caption = "Item Qty";
            // column order
            //grdItems.DisplayLayout.Bands[0].Columns[4].Header.VisiblePosition = 0;  // image product And Package
            //grdItems.DisplayLayout.Bands[0].Columns[0].Header.VisiblePosition = 1;   // Stock Item
            //grdItems.DisplayLayout.Bands[0].Columns[1].Header.VisiblePosition = 2;   // Barcode
            //grdItems.DisplayLayout.Bands[0].Columns[2].Header.VisiblePosition = 3; // Batch Code
            //grdItems.DisplayLayout.Bands[0].Columns[3].Header.VisiblePosition = 4; // Qty

            // cell click row select ..
            //e.Layout.Bands[0].Columns[1].CellClickAction = CellClickAction.RowSelect;

            // cell padding
            //grdItems.DisplayLayout.Bands[0].Override.CellPadding = 20;

            // columns style as a url
            //e.Layout.Bands[0].Columns[0].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;

            //e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LightYellow;
        }

        #endregion
    }
}
