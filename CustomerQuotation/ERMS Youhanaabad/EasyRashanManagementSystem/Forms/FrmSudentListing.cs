using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmSudentListing : Form
    {
        public DataTable dt = new DataTable();
        public FrmSudentListing()
        {
            InitializeComponent();
        }

        public void LoadDataInGridView()
        {
            string Qry = @"Select StudentID,Name,FatherName,ClassName,TeacherName from Trans_Student A
                            inner join Def_Class B on A.ClassID=B.ClassID
                            inner join Def_Teacher C on A.TeacherId = C.TeacherID";
            dt = DataAccess.GetData(Qry);
            // First Time Load Form 
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dataGrdStudent.DataSource == null)
                {
                    dataGrdStudent.DataSource = dt;
                    //DataGridViewImageColumn imgd = new DataGridViewImageColumn();
                    //Image images = EasyRashanManagementSystem.Properties.Resources.reports;
                    //imgd.Image = images;
                    //imgd.HeaderText = "Edit";
                    //imgd.Name = "Edit";
                    //imgd.Width = 5;
                    //imgd.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    //imgd.DefaultCellStyle.ForeColor = Color.DimGray;
                    //dataGrdStudent.Columns.Add(imgd);
                    dataGrdStudent.Columns[0].Visible = false;

                    if (dataGrdStudent.Rows.Count <= 5)
                    {
                        BasicGridProperty();
                    }
                    else
                    {
                        GridViewProperty();
                    }
                }
                // Save Record and this code run
                else if (dataGrdStudent.Rows.Count > 0)
                {
                    dataGrdStudent.DataSource = dt;
                    dataGrdStudent.Columns.Remove("Edit");
                    DataGridViewImageColumn imgd = new DataGridViewImageColumn();
                    Image images = EasyRashanManagementSystem.Properties.Resources.reports;
                    imgd.Image = images;
                    imgd.HeaderText = "Edit";
                    imgd.Name = "Edit";
                    imgd.Width = 5;
                    imgd.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    imgd.DefaultCellStyle.ForeColor = Color.DimGray;
                    dataGrdStudent.Columns.Add(imgd);
                    dataGrdStudent.Columns[0].Visible = false;

                    if (dataGrdStudent.Rows.Count <= 5)
                    {
                        BasicGridProperty();
                    }
                    else
                    {
                        GridViewProperty();
                    }
                }
                // text Searh run code
                //else if (dataGrdStudent.Rows.Count == 0)
                //{
                //    dataGrdStudent.DataSource = dt;
                //    dataGrdStudent.Columns.Remove("Edit");
                //    DataGridViewImageColumn imgd = new DataGridViewImageColumn();
                //    Image images = SchoolManagementSystem.Properties.Resources.edit;
                //    imgd.Image = images;
                //    imgd.HeaderText = "Edit";
                //    imgd.Name = "Edit";
                //    imgd.Width = 5;
                //    imgd.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                //    imgd.DefaultCellStyle.ForeColor = Color.DimGray;
                //    dataGrdStudent.Columns.Add(imgd);
                //    dataGrdStudent.Columns[0].Visible = false;
                //    if (dataGrdStudent.Rows.Count <= 5)
                //    {
                //        BasicGridProperty();
                //    }
                //    else
                //    {
                //        GridViewProperty();
                //    }
                //}

            }
        }

        private void BasicGridProperty()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
            columnHeaderStyle.ForeColor = Color.Black;
            dataGrdStudent.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dataGrdStudent.AllowUserToAddRows = false;
            dataGrdStudent.ReadOnly = true;
            dataGrdStudent.Columns[0].Width = 10;
            dataGrdStudent.Columns[1].Width = 215;
            dataGrdStudent.Columns[2].Width = 210;
            dataGrdStudent.Columns[3].Width = 190;
            dataGrdStudent.Columns[4].Width = 190;
            //dataGrdStudent.Columns[5].Width = 60;
            dataGrdStudent.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGrdStudent.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dataGrdStudent.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGrdStudent.EnableHeadersVisualStyles = false;
            this.dataGrdStudent.GridColor = Color.DarkGray;
            dataGrdStudent.ForeColor = Color.DimGray;
            dataGrdStudent.EnableHeadersVisualStyles = false;
            dataGrdStudent.ColumnHeadersHeight = 40;
            dataGrdStudent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGrdStudent.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
            dataGrdStudent.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[3].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[4].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            //dataGrdStudent.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dataGrdStudent.Columns[1].HeaderCell.Style.Padding = new Padding(27, 0, 0, 0);
            dataGrdStudent.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dataGrdStudent.Columns[2].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dataGrdStudent.Columns[3].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dataGrdStudent.Columns[4].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
        }

        private void GridViewProperty()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
            columnHeaderStyle.ForeColor = Color.Black;
            dataGrdStudent.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dataGrdStudent.AllowUserToAddRows = false;
            dataGrdStudent.ReadOnly = true;
            dataGrdStudent.Columns[0].Width = 10;
            dataGrdStudent.Columns[1].Width = 180;
            dataGrdStudent.Columns[2].Width = 181;
            dataGrdStudent.Columns[3].Width = 180;
            dataGrdStudent.Columns[4].Width = 187;
            dataGrdStudent.Columns[5].Width = 60;
            dataGrdStudent.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGrdStudent.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dataGrdStudent.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGrdStudent.EnableHeadersVisualStyles = false;
            this.dataGrdStudent.GridColor = Color.DarkGray;
            dataGrdStudent.ForeColor = Color.DimGray;
            dataGrdStudent.EnableHeadersVisualStyles = false;
            dataGrdStudent.ColumnHeadersHeight = 40;
            dataGrdStudent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGrdStudent.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
            dataGrdStudent.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[3].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[4].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
            dataGrdStudent.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dataGrdStudent.Columns[1].HeaderCell.Style.Padding = new Padding(27, 0, 0, 0);
            dataGrdStudent.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dataGrdStudent.Columns[2].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dataGrdStudent.Columns[3].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dataGrdStudent.Columns[4].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void FrmSudentListing_Load(object sender, EventArgs e)
        {
            LoadDataInGridView();
        }
    }
}
