namespace VersatileEducationSystem
{
    partial class FrmReportCont
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
            this.crvReports = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crvReports
            // 
            this.crvReports.ActiveViewIndex = -1;
            this.crvReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvReports.Cursor = System.Windows.Forms.Cursors.Default;
            this.crvReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvReports.Location = new System.Drawing.Point(0, 0);
            this.crvReports.Name = "crvReports";
            this.crvReports.SelectionFormula = "";
            this.crvReports.ShowCloseButton = false;
            this.crvReports.ShowGroupTreeButton = false;
            this.crvReports.Size = new System.Drawing.Size(647, 519);
            this.crvReports.TabIndex = 0;
            this.crvReports.ViewTimeSelectionFormula = "";
            // 
            // FrmReportCont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 519);
            this.Controls.Add(this.crvReports);
            this.MaximizeBox = false;
            this.Name = "FrmReportCont";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmReportCont_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer crvReports;

    }
}