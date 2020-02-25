namespace EasyRashanManagementSystem.Forms
{
    partial class FrmTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGrdUserManagement = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrdUserManagement)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrdUserManagement
            // 
            this.dataGrdUserManagement.AllowUserToAddRows = false;
            this.dataGrdUserManagement.AllowUserToDeleteRows = false;
            this.dataGrdUserManagement.AllowUserToOrderColumns = true;
            this.dataGrdUserManagement.AllowUserToResizeColumns = false;
            this.dataGrdUserManagement.AllowUserToResizeRows = false;
            this.dataGrdUserManagement.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGrdUserManagement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrdUserManagement.Location = new System.Drawing.Point(63, 48);
            this.dataGrdUserManagement.Name = "dataGrdUserManagement";
            this.dataGrdUserManagement.RowHeadersVisible = false;
            this.dataGrdUserManagement.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGrdUserManagement.RowTemplate.Height = 30;
            this.dataGrdUserManagement.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGrdUserManagement.Size = new System.Drawing.Size(253, 240);
            this.dataGrdUserManagement.TabIndex = 5;
            this.dataGrdUserManagement.TabStop = false;
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 443);
            this.Controls.Add(this.dataGrdUserManagement);
            this.Name = "FrmTest";
            this.Text = "FrmTest";
            this.Load += new System.EventHandler(this.FrmTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrdUserManagement)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrdUserManagement;
    }
}