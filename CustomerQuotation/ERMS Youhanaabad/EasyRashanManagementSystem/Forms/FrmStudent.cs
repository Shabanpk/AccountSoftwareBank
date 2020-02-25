using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using System.Data.SqlClient;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmStudent : Form
    {
        DataTable dt = new DataTable();
         long StuID=0;
        public FrmStudent()
        {
            InitializeComponent();
        }

        void LoadcboClass()
        {
            dt = DataAccess.GetData(@"select * from Def_Class");
            if (dt != null && dt.Rows.Count > 0)
            {
                cboClass.ValueMember = "ClassID";
                cboClass.DisplayMember = "ClassName";
                cboClass.DataSource = dt;
            }
        }

        void LoadcboTeacher()
        {
            dt = DataAccess.GetData(@"select * from Def_Teacher");
            if (dt != null && dt.Rows.Count > 0)
            {
                cboteacher.ValueMember = "TeacherID";
                cboteacher.DisplayMember = "TeacherName";
                cboteacher.DataSource = dt;
            }
        }

        bool SaveRecord()
        {
            bool IsSave = false;

            string Qry = @"insert into Trans_Student (StudentID,Name,Age,FatherName,Address,ClassID,TeacherID)
                          Values ('"+txtStudentID.Text+"','" + txtStudentName.Text + "','" + dtpAge.Value+ "','" + txtFatherName.Text+ "','" + txtAddress.Text+ "','" + cboClass.SelectedValue + "','" + cboteacher.SelectedValue + "')";
            IsSave = DataAccess.InserRecord(Qry);
            return IsSave;
        }
        void ClearForm()
        {
            txtStudentID.Text = "";
            txtStudentName.Text = "";
            txtFatherName.Text = "";
            txtAddress.Text = "";
        }

        private void GetMaxMUnitID()
        {
            //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            SqlConnection sqlconn = new SqlConnection(DataAccess.ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(StudentID),0)+ 1  As [StudentID] From Trans_Student ", sqlconn);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                StuID = long.Parse(dt.Rows[0]["StudentID"].ToString());
            }
            txtStudentID.Text = StuID.ToString();
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
            dt.Dispose();
        }


        private void FrmStudent_Load(object sender, EventArgs e)
        {
            GetMaxMUnitID();
            LoadcboClass();
            LoadcboTeacher();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecord())
            {
                MessageBox.Show("Record Saved");
                ClearForm();
                GetMaxMUnitID();
                LoadcboClass();
                LoadcboTeacher();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmSudentListing newobj = new FrmSudentListing();
            newobj.ShowDialog();
        }

    }
}
