namespace POSFM
{
    partial class frmInventory
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
            this.bttnStockIn = new System.Windows.Forms.Button();
            this.bttnStockOut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bttnStockIn
            // 
            this.bttnStockIn.BackColor = System.Drawing.Color.Teal;
            this.bttnStockIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bttnStockIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttnStockIn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnStockIn.ForeColor = System.Drawing.Color.White;
            this.bttnStockIn.Image = global::POSFM.Properties.Resources.pos;
            this.bttnStockIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttnStockIn.Location = new System.Drawing.Point(70, 118);
            this.bttnStockIn.Name = "bttnStockIn";
            this.bttnStockIn.Size = new System.Drawing.Size(195, 70);
            this.bttnStockIn.TabIndex = 14;
            this.bttnStockIn.Text = "Stock In";
            this.bttnStockIn.UseVisualStyleBackColor = false;
            // 
            // bttnStockOut
            // 
            this.bttnStockOut.BackColor = System.Drawing.Color.Teal;
            this.bttnStockOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bttnStockOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttnStockOut.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttnStockOut.ForeColor = System.Drawing.Color.White;
            this.bttnStockOut.Image = global::POSFM.Properties.Resources.pos;
            this.bttnStockOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttnStockOut.Location = new System.Drawing.Point(282, 118);
            this.bttnStockOut.Name = "bttnStockOut";
            this.bttnStockOut.Size = new System.Drawing.Size(195, 70);
            this.bttnStockOut.TabIndex = 15;
            this.bttnStockOut.Text = "Stock Out";
            this.bttnStockOut.UseVisualStyleBackColor = false;
            // 
            // frmInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 295);
            this.Controls.Add(this.bttnStockOut);
            this.Controls.Add(this.bttnStockIn);
            this.Name = "frmInventory";
            this.Text = "frmInventory";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bttnStockIn;
        private System.Windows.Forms.Button bttnStockOut;
    }
}