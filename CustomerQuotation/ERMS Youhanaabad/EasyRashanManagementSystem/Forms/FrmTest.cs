using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmTest : Form
    {
        BusinessEntities.BE_Users NewBEObj = new BusinessEntities.BE_Users();
        DAL.DAL_UserManagement NewDALLObj = new DAL.DAL_UserManagement();
        int i = 0;


        public FrmTest()
        {
            InitializeComponent();
        }

        void LoadDataGridView()
        {
            DataTable dt = new DataTable();
            DAL.DAL_UserManagement NewDLLObj = new DAL.DAL_UserManagement();
            dt = NewDLLObj.GetFormName();
            if (dt != null && dt.Rows.Count > 0)
            {
                    dataGrdUserManagement.DataSource = dt;

                    DataGridViewCheckBoxColumn CheckBox = new DataGridViewCheckBoxColumn();
                    CheckBox.HeaderText = "Rights";
                    CheckBox.Name = "CanView";
                    CheckBox.Width = 5;
                    CheckBox.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBox.DefaultCellStyle.ForeColor = Color.DimGray;
                    dataGrdUserManagement.Columns.Add(CheckBox);

                    dataGrdUserManagement.Columns[0].Visible = false;
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
                    columnHeaderStyle.ForeColor = Color.Black;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                    dataGrdUserManagement.Columns[1].ReadOnly = true;

                    dataGrdUserManagement.Columns[1].Width = 165;
                    dataGrdUserManagement.Columns[2].Width = 70;

                    dataGrdUserManagement.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGrdUserManagement.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGrdUserManagement.EnableHeadersVisualStyles = false;
                    this.dataGrdUserManagement.GridColor = Color.DarkGray;
                    dataGrdUserManagement.ForeColor = Color.DimGray;
                    dataGrdUserManagement.EnableHeadersVisualStyles = false;
                    dataGrdUserManagement.ColumnHeadersHeight = 30;
                    dataGrdUserManagement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    //dataGrdUserManagement.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    //dataGrdUserManagement.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    //dataGrdUserManagement.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    ////dataGrdUserManagement.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //dataGrdUserManagement.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
                    //dataGrdUserManagement.Columns[2].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
               
            }
        }

        private void FrmTest_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
        }
    }
}
