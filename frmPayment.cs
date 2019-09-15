using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace POSFM
{
    public partial class frmPayment : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmPayment()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void frmPayment_Load(object sender, EventArgs e)
        {
        }

        private void txtPay_TextChanged(object sender, EventArgs e)
        {
            if (txtPay.Text.Length > 0)
            {
                decimal price = decimal.Parse(txtTotal.Text);
                string strPaid = txtPay.Text;
                decimal paid = decimal.Parse(strPaid.Replace(",", ""));
                decimal change = paid - price;
                txtChange.Text = change.ToString("#,##0.00");
            }
            else
            {
                txtChange.Clear();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtPay.Text == "")
            {
                MessageBox.Show("Please Input Payment!", "Message");
                return;
            }

            if (Convert.ToDecimal(txtChange.Text) < 0)
            {
                MessageBox.Show("Insufficient Payment!", "Message");
                return;
            }

            updData();
            updDisplay();
            account.IsTrans = true;
            rpt();
            this.Close();
        }

        private void updData()
        {
            decimal decPay = Convert.ToDecimal(txtPay.Text);
            string strPay  = decPay.ToString("#,##0.00");

            string query = "update tblorder set Order_Payment = @d1, Order_Change = @d2 where Order_TransNo = '"+ account.TransNo + "'";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@d1", strPay);
            cmd.Parameters.AddWithValue("@d2", txtChange.Text);
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            con.Close();
        }

     
        private void rpt()
        {
            con.Open();
            DataSet1 ds = new DataSet1();
            adpt = new MySqlDataAdapter("SELECT * FROM tblorder WHERE Order_TransNo = '" + account.TransNo + "'", con);
            adpt.Fill(ds.tblorder);

            reports_receipt receipt = new reports_receipt();

            TextObject text1 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["cashier"];
            text1.Text = account.UserFullName;

            TextObject text2 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["OderType"];
            text2.Text = account.OrderType;

            TextObject text3 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["tableNo"];
            if(account.StrTableNo == "")
            {
                text3.Text = "-";
            }
            else
            {
                text3.Text = account.StrTableNo;
            }

            receipt.SetDataSource(ds);

            frmReceipt rpt = new frmReceipt();
            rpt.crpReceipt.ReportSource = receipt;
            rpt.crpReceipt.RefreshReport();
            rpt.crpReceipt.RefreshReport();

            rpt.ShowDialog();
            con.Close();
        }

        private void txtPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 46 || (char.IsControl(e.KeyChar)))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void updDisplay()
        {
            string query = "Update tblproduction set resetProd = 'True', Status = 'Pending' where Order_TransNo = '" + account.TransNo + "'";
            cmd = new MySqlCommand(query, con);
            con.Open();
            adpt = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            con.Close();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
