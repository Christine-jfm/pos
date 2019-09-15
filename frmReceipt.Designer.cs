namespace POSFM
{
    partial class frmReceipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReceipt));
            this.crpReceipt = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.dataSet11 = new POSFM.DataSet1();
            this.reports_receipt4 = new POSFM.reports_receipt();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            this.SuspendLayout();
            // 
            // crpReceipt
            // 
            this.crpReceipt.ActiveViewIndex = 0;
            this.crpReceipt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crpReceipt.Cursor = System.Windows.Forms.Cursors.Default;
            this.crpReceipt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crpReceipt.Location = new System.Drawing.Point(0, 0);
            this.crpReceipt.Name = "crpReceipt";
            this.crpReceipt.ReportSource = this.reports_receipt4;
            this.crpReceipt.Size = new System.Drawing.Size(466, 577);
            this.crpReceipt.TabIndex = 3;
            this.crpReceipt.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crpReceipt.Load += new System.EventHandler(this.crpReceipt_Load);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(512, 293);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(38, 10);
            this.hScrollBar1.TabIndex = 4;
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(466, 577);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.crpReceipt);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receipt";
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer crpReceipt;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private DataSet1 dataSet11;
        private reports_receipt reports_receipt4;
    }
}