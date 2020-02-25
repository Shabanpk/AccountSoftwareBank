using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;


namespace DAL
{
    public class DAL_DbBackUp
    {

        //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //string ConnString = "Data Source=FAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";
        //string ConnString = "Data Source=192.168.3.90;Initial Catalog=RashanGharLatest;User id=sa;password=123";
        string ConnString = DataAccess.ConnString;

        private DataTable GetdataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {
            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable GetDataTable(string qry, SqlConnection sqlconn)
        {
            sqlconn.ConnectionString = ConnString;
            SqlDataAdapter da = new SqlDataAdapter(qry, sqlconn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void DbBackUp()
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand();

                //if (!File.Exists(@"D:\bkup20130126.BAK"))
                //if (!File.Exists(@"D:\bkup20130126.BAK"))
                //{
                    string SqlQry = @"DECLARE @name VARCHAR(50) -- database name 
                                  DECLARE @path VARCHAR(256) -- path for backup files  
                                  DECLARE @fileName VARCHAR(256) -- filename for backup  
                                  DECLARE @fileDate VARCHAR(256) -- used for file name                                    
                                  SET @path = 'D:\abc\subdirectory\' 
                                  SELECT @fileDate = GETDATE() 
                                  DECLARE db_cursor CURSOR FOR  
                                  SELECT name 
                                  FROM master.dbo.sysdatabases 
                                  WHERE name NOT IN ('master','model','msdb','tempdb') and name ='RashanGhar'  
                                  OPEN db_cursor   
                                  FETCH NEXT FROM db_cursor INTO @name   
                                  WHILE @@FETCH_STATUS = 0   
                                  BEGIN   
                                       SET @fileName = @path + 'bkup' + @fileDate + '.BAK'   
                                       BACKUP DATABASE @name TO DISK = @fileName  

                                       FETCH NEXT FROM db_cursor INTO @name   
                                END   
                                CLOSE db_cursor   
                                DEALLOCATE db_cursor";
                    sqlcomm.CommandText = SqlQry;
                    sqlcomm.Connection = sqlconn;
                    sqlcomm.ExecuteNonQuery();

                    //MailMessage NewMail = new MailMessage();
                    //NewMail.To.Add("phahad.gemini@gmail.com");
                    //NewMail.Subject = "RashanGhar-Bkup";
                    //NewMail.Body = "This is backupfile";
                    //NewMail.From = new MailAddress("phahad.baig@gmail.com");

                    //System.Net.Mail.Attachment attach = new Attachment(@"D:\bkup20130113.BAK");
                    //if (attach != null)
                    //{
                    //    NewMail.Attachments.Add(attach);
                    //}

                    //SmtpClient smtpobj = new SmtpClient("smtp.gmail.com", 587);
                    //smtpobj.EnableSsl = true;
                    //smtpobj.Timeout = 10000000;
                    //smtpobj.UseDefaultCredentials = false;
                    //smtpobj.Credentials = new NetworkCredential("phahad.baig@gmail.com", "iloveyoumuhammad");
                    //smtpobj.Send(NewMail);
                    //attach.Dispose();
                    //NewMail.Dispose();
                    //File.Delete(@"D:\bkup20130113.BAK");
                    sqlconn.Close();
                    sqlcomm.Dispose();
                //}
                //else
                //{
                //    MessageBox.Show("File Already Exits.","Easy Rashan management System",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void TableCreation()
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string Qry = @"Select count(id)As[ID] from Sysobjects where name='Trans_BankWithDrawl' and Type='U'";
                bool IsExit = false;
                int Exist = 0;
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                DataTable dt = new DataTable();
                dt = GetdataTable(sqlcomm, sqlconn);
                if (dt.Rows.Count > 0)
                {
                    Exist = int.Parse(dt.Rows[0]["ID"].ToString());
                    if (Exist == 0)
                    {
                        IsExit = false;
                    }
                    else
                        IsExit = true;
                    sqlconn.Close();
                    sqlcomm.Dispose();
                }
                if (IsExit == false)
                {
                    string qry = @"CREATE TABLE [dbo].[Trans_BankWithDrawl](
	                               [WD_ID] [int] NULL,
	                               [WD_Date] [datetime] NULL,
	                               [ModifiedDate] [datetime] NULL,
	                               [BankID] [int] NULL,
	                               [WD_ChequeNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	                               [WD_Amount] [float] NULL
                                   ) ON [PRIMARY]";
                    sqlcomm.CommandText = qry;
                    sqlcomm.Connection = sqlconn;
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();
                    sqlcomm.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void CreateDirectory()
        {
            string path = @"F:\abc\subdirectory\";

            if (!Directory.Exists(path))
            {

                // Creating folder 

                DirectoryInfo di = Directory.CreateDirectory(path);

                // setting attributes

                //di.Attributes=FileAttributes.Directory;
                //di.Attributes = FileAttributes.Hidden;
            }
        }

        public DataTable GetDueDate(string DueDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select PRID,Convert(varchar(11),PRDate,106) As [PRDate],
                                CONVERT(varchar(11),CreditDueDate,106) As [DueDate],(PRAmount-knockOffAmount) As [Balance] From KnockOffPurchase 
                                where CreditDueDate='" + DueDate + "'";
                dt = GetDataTable(qry, sqlconn);
                sqlconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable ColumnCreation()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                string qry = @"Select SlipNo From Registration";
                SqlCommand sqlcomm = new SqlCommand(qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return dt = null;
            }
            return dt;
        }

        public void CreateColumn()
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            string Qry = @"Alter Table Registration add SlipNo varchar(25) null";
            SqlCommand sqlcomm = new SqlCommand(Qry,sqlconn);
            sqlcomm.ExecuteNonQuery();
            sqlcomm.Dispose();
            sqlconn.Close();
        }

        public bool DatabaseBackup()
        {
            try
            {
                string path1 = "D:/ERMSDB/ERMS.bak";
                string path2 = "D:/ERMSDB/ERMS.rar";
                if (File.Exists(path1) && File.Exists(path2))
                {
                    File.Delete(path1);
                    File.Delete(path2);
                    SqlConnection connection = new SqlConnection(this.ConnString);
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("backup database RashanGhar To Disk='" + path1 + "'", connection);
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    sqlCommand.Dispose();
                }
                else if (!File.Exists(path1) && File.Exists(path2))
                {
                    File.Delete(path2);
                    SqlConnection connection = new SqlConnection(this.ConnString);
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("backup database RashanGhar To Disk='" + path1 + "'", connection);
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    sqlCommand.Dispose();
                }
                else if (File.Exists(path1) && !File.Exists(path2))
                {
                    File.Delete(path1);
                    SqlConnection connection = new SqlConnection(this.ConnString);
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("backup database RashanGhar To Disk='" + path1 + "'", connection);
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    sqlCommand.Dispose();
                }
                else
                {
                    SqlConnection connection = new SqlConnection(this.ConnString);
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("backup database RashanGhar To Disk='" + path1 + "'", connection);
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    sqlCommand.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
        }

        public void CreateBackUpLog()
        {
            SqlConnection sqlConnection = new SqlConnection(this.ConnString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            string str = "INSERT INTO BackupTiming\r\n                           (BackUpDate, IsActive)\r\n                           VALUES     (@BackUpDate, @IsActive)";
            sqlCommand.CommandText = str;
            sqlCommand.Parameters.Add("@BackUpdate", (object)DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@IsActive", (object)1);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

    }
}
