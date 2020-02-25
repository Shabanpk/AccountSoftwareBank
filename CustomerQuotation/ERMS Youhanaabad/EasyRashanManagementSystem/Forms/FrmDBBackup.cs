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
using System.Net.Mail;
using System.Net;


namespace EasyRashanManagementSystem
{
    public partial class FrmDBBackup : Form
    {
        public FrmDBBackup()
        {
            InitializeComponent();
        }

        #region Modifiers

        private string FilePath = "D:\\ERMSDB\\ERMS.bak";
        private string FilePath2 = "D:\\ERMSDB\\ERMS.rar";

        #endregion

        #region Functions

        private void ClearFields()
        {
            this.txtAttachment.Text = "";
        }


        #endregion

        #region Events

        private void btnClose_Click(object sender, EventArgs e)
        {
            System.IO.File.Delete(this.FilePath);
            System.IO.File.Delete(this.FilePath2);
            this.Close();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            //DAL_DbBackUp NewDALObj = new DAL_DbBackUp();
            //NewDALObj.DbBackUp();
            try
            {
                DAL_DbBackUp dalDbBackUp = new DAL_DbBackUp();
                if (dalDbBackUp.DatabaseBackup())
                {
                    txtAttachment.Enabled = true;
                    dalDbBackUp.CreateBackUpLog();
                    this.btnAttachement.Enabled = true;
                    this.btnSendEmail.Enabled = false;
                    this.btnBackUp.Enabled = false;
                }
                else
                {
                    dalDbBackUp.CreateBackUpLog();
                    this.txtAttachment.Enabled = false;
                    this.btnAttachement.Enabled = false;
                    this.btnSendEmail.Enabled = false;
                    this.btnBackUp.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "BackUp files (*.bak)|*.rar|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = "D:\\ERMSDB";
                openFileDialog.Title = "Please Select BackUp File...";
                this.txtAttachment.Text = "";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.txtAttachment.Text = openFileDialog.FileName;
                    this.btnSendEmail.Enabled = true;
                }
                else
                    this.btnSendEmail.Enabled = false;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void EmailCode()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            try
            {
                message.From = new MailAddress("phahad.baig@gmail.com");
                message.To.Add("phahad.baig@gmail.com");
                message.To.Add("phahad.gemini@gmail.com");
                //message.To.Add("khalid.usman05@yahoo.com");
                message.To.Add("saghars_farman@yahoo.com");
                MailMessage mailMessage1 = message;
                string str1 = "BackUp Of ERMS (YouhanaAbad) of Dated:-";
                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.Date;
                string str2 = dateTime.ToString("yyyy/MM/dd");
                string str3 = str1 + str2;
                mailMessage1.Subject = str3;
                MailMessage mailMessage2 = message;
                string str4 = "This is Backup file Created by Mr. Saghar Farman\r\n Branch Manager of Rashan Ghar Organization Youhanaabad Branch on Date:= '";
                dateTime = DateTime.Now;
                // ISSUE: variable of a boxed type
                DateTime local = dateTime.Date;
                string str5 = "'";
                string str6 = str4 + (object)local + str5;
                mailMessage2.Body = str6;
                Attachment attachment = new Attachment(this.txtAttachment.Text.Trim());
                smtpClient.Port = 587;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential("phahad.baig@gmail.com", "Iloveyoumuhammad786");
                //smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 300000;
                message.Attachments.Add(attachment);
                smtpClient.Send(message);
                message.Dispose();
                attachment.Dispose();
                System.IO.File.Delete(this.FilePath);
                System.IO.File.Delete(this.FilePath2);
                this.Close();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                message.Dispose();
                System.IO.File.Delete(this.FilePath);
                System.IO.File.Delete(this.FilePath2);
            }
        }

        #endregion
    }
}
