using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DAL;

namespace DAL
{
    public partial class FrmGSearch : Form
    {
        public FrmGSearch()
        {
            InitializeComponent();
        }

        #region Modifiers
        public DataTable tempDT = null;
        private object returnValue = null;
        #endregion

        #region Functions

        public static Object Show(string query, bool showFirstCol, string formHeading)
        {
            try
            {
                string Connstring = DataAccess.ConnString;
                //string Connstring = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
                //string Connstring = "Data Source=192.168.3.90;Initial Catalog=RashanGharLatest;User id=sa;password=123";
                //string Connstring = "Data Source=FAHAD;Initial Catalog=RashanGharLatest;User id=sa;password=123456";
                //string ConnString = DataAccess.GetConnection();
                SqlConnection sqlconn = new SqlConnection(Connstring);
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.Connection = sqlconn;
                //SqlDataAdapter da; // = new SqlDataAdapter();
                DataTable dt = new DataTable();

                if (!query.Trim().Equals(""))
                {
                    sqlcomm.CommandText = query;
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
                    sda.Fill(dt);

                    FrmGSearch fSearch = new FrmGSearch();

                    fSearch.grd.DataSource = dt;
                    fSearch.grd.DataBind();

                    fSearch.tempDT = new DataTable();
                    fSearch.tempDT = dt.Copy();

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    //--format Grid-----------------------------------------------------------------------------------------------------------------------------------------
                    //------------------------------------------------------------------------------------------------------------------------------------------------------

                    if (showFirstCol == false)
                        fSearch.grd.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                    else
                        fSearch.grd.DisplayLayout.Bands[0].Columns[0].Hidden = false;

                    for (int i = 0; i < fSearch.grd.DisplayLayout.Bands[0].Columns.Count; i++)
                    {
                        fSearch.grd.DisplayLayout.Bands[0].Columns[i].AutoEdit = false;
                        fSearch.grd.DisplayLayout.Bands[0].Columns[i].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                        fSearch.grd.DisplayLayout.Bands[0].Columns[i].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                    }

                    for (int i = 0; i < fSearch.tempDT.Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (showFirstCol)

                                fSearch.cboSearch.Items.Add(fSearch.tempDT.Columns[i].ColumnName);
                        }
                        else
                        {
                            fSearch.cboSearch.Items.Add(fSearch.tempDT.Columns[i].ColumnName);
                        }
                    }

                    if (fSearch.cboSearch.Items.Count > 0)
                        fSearch.cboSearch.SelectedIndex = 0;

                    if (fSearch.grd.Rows.Count > 0)
                    {
                        fSearch.grd.Rows[0].Selected = true;
                    }

                    fSearch.Text = formHeading;
                    fSearch.ShowDialog();

                    sqlcomm.Dispose();
                    sda.Dispose();
                    sqlconn.Close();

                    return fSearch.returnValue;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in FrmGSearch.Show :" + e.Message);
                return null;
            }
        }

        private void GetGridValue()
        {
            if (grd.Selected.Rows != null)
                if (grd.Selected.Rows.Count > 0)
                {
                    returnValue = grd.Selected.Rows[0].Cells[0].Value;
                    return;
                }

            returnValue = null;
        }

        #endregion

        #region Events

        private void FrmGSearch_Load(object sender, EventArgs e)
        {
            cboSearch.Focus();
        }

        private void grd_DoubleClick(object sender, EventArgs e)
        {
            GetGridValue();
            this.Close();
        }

        private void grd_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            GetGridValue();
            this.Close();
        }

        private void grd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                GetGridValue();
                if (returnValue != null)
                    this.Close();
            }
        }

        private void FrmGSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                GetGridValue();
                this.Close();
            }
            if (e.KeyValue == 27)
            {
                this.Close();
            }




        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GetGridValue();
            this.Close();
        }

        private void txtSearchText_TextChanged_1(object sender, EventArgs e)
        {

            if (txtSearchText.Text.Trim().Equals(""))
            {
                grd.DataSource = tempDT;
                grd.DataBind();
            }
            else
            {
                DataRow[] rows = tempDT.Select(cboSearch.Text + " like '%" + txtSearchText.Text + "%'");
                //DataRow row;
                DataTable dt = tempDT.Copy();
                dt.Rows.Clear();

                for (int i = 0; i < rows.Length; i++)
                {
                    dt.Rows.Add(rows[i].ItemArray);
                }

                grd.DataSource = dt;
                grd.DataBind();
                grd.Refresh();

                if (rows.Length > 0)
                    grd.Rows[0].Selected = true;
            }

        }

        private void txtSearchText_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                this.Close();
            }
            //--If Up Key Press
            if (e.KeyValue == 38)
            {
                //if (cboSearch.SelectedIndex > 0)
                //    cboSearch.SelectedIndex -= 1;
                if (grd.Rows.Count > 0)
                    if (this.grd.Selected.Rows == null)
                    {
                        grd.Rows[0].Selected = true;
                    }
                    else
                    {
                        if (grd.Selected.Rows[0].Index > 0)
                            grd.Rows[grd.Selected.Rows[0].Index - 1].Selected = true;
                    }
            }
            //--If Down Key Press
            if (e.KeyValue == 40)
            {
                //if (cboSearch.SelectedIndex < cboSearch.Items.Count-1)
                //    cboSearch.SelectedIndex += 1;
                if (grd.Rows.Count > 0)
                    if (this.grd.Selected.Rows == null)
                    {
                        grd.Rows[0].Selected = true;
                    }
                    else
                    {
                        if (grd.Selected.Rows[0].Index < grd.Rows.Count - 1)

                            grd.Rows[grd.Selected.Rows[0].Index + 1].Selected = true;
                    }
            }

            if (e.KeyValue == 13)
            {
                GetGridValue();
                this.Close();
            }
            this.Text = e.KeyValue.ToString();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
