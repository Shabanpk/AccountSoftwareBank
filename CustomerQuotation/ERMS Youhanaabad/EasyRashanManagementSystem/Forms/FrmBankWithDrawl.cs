using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessEntities;
using BusinessProcessObjects;
using DAL;
using System;

namespace EasyRashanManagementSystem
{
    public partial class FrmBankWithDrawl : Form
    {
        public FrmBankWithDrawl()
        {
            InitializeComponent();
        }

        #region Functions

        private bool SaveRecord()
        {
            try
            {
                BE_BankWithDrawl NewBEObj = new BE_BankWithDrawl();
                NewBEObj.WD_Date = DateTime.Now;
                NewBEObj.ChequeNo = txtChequeNo.Text;
                NewBEObj.BankID = int.Parse(cboBanks.Value.ToString());
                NewBEObj.WD_Amount=Convert.ToDouble(txtWithDrawlAmount.Value);
                BP_BankWithDrawl NewInsBP = new BP_BankWithDrawl();
                NewInsBP.Insert_Record(NewBEObj);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return false;
            }
        }

        private bool ValidateFormEntry()
        {
            try
            {
                if (txtChequeNo.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Please Enter Cheque No...", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (Convert.ToDouble(txtWithDrawlAmount.Value) == 0)
                {
                    MessageBox.Show("Please Enter Some Amount to WithDraw...", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (Convert.ToDouble(txtWithDrawlAmount.Value) < 0)
                {
                    MessageBox.Show("Please Enter Positive Value...", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (Convert.ToDouble(txtWithDrawlAmount.Value) > Convert.ToDouble(txtBankBalance.Value))
                {
                    MessageBox.Show("Withdrawl Amount is Greater than Bank Balance...", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return false;
            }
        }

        private void ClearForm()
        {
            txtChequeNo.Text = "";
            txtWithDrawlAmount.Text = "";

        }

        #endregion

        #region Events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateFormEntry())
                {
                    if (SaveRecord())
                    {
                        MessageBox.Show("Record Saved Successfully", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtWithDrawlAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '\b')
            {
                e.Handled = false; ;
            }
            else if (e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {

            }
        }

        private void cboBanks_ValueChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            BP_BankWithDrawl NewBP = new BP_BankWithDrawl();
            dt = NewBP.GetBankBalance(int.Parse(cboBanks.Value.ToString()));
            if (dt.Rows.Count > 0)
            {
                txtBankBalance.Text = dt.Rows[0]["BankBalance"].ToString();
            }
        }

        private void FrmBankWithDrawl_Load(object sender, EventArgs e)
        {
            BP_BankWithDrawl NewBP = new BP_BankWithDrawl();
            NewBP.LoadComboBanks(cboBanks);
            DataTable dtCashBalance = new DataTable();
            DataTable dtBankBalance = new DataTable();
            dtCashBalance = NewBP.GetCashBalance();
            if (dtCashBalance.Rows.Count > 0)
            {
                txtCashBalance.Text = dtCashBalance.Rows[0]["CashBalance"].ToString();
            }
            dtBankBalance = NewBP.GetBankBalance(int.Parse(cboBanks.Value.ToString()));
            if (dtBankBalance.Rows.Count > 0)
            {
                txtBankBalance.Text = dtBankBalance.Rows[0]["BankBalance"].ToString();
            }
        }

        #endregion

    }
}
