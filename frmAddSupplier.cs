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
using System.IO;
using System.Drawing.Imaging;
using System.Globalization;

using System.Text.RegularExpressions;
namespace POSFM
{
    public partial class frmAddSupplier : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        bool isExist = false;
        public frmAddSupplier()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reset()
        {
            txtSName.Clear();
            txtSAdd.Clear();
            txtSProd.Clear();
        }

        //SAVE
        private void btnSave_Click(object sender, EventArgs e)
        {
            {
                if (txtSName.Text == "")
                {
                    MessageBox.Show("Please Input Supplier Name!", "Message");
                    return;
                }

                if (txtSAdd.Text == "")
                {
                    MessageBox.Show("Please Input Supplier Adddress!", "Message");
                    return;
                }

                if (txtSProd.Text == "")
                {
                    MessageBox.Show("Please Input Supplier Product!", "Message");
                    return;
                }

                if (account.IsSupplierAdd)
                {
                    chkExist();
                    if (isExist)
                    {
                        isExist = false;
                        MessageBox.Show("Supplier already exist!", "Message");
                        return;
                    }

                    addAccount();
                    Logs();
                }
                else
                {
                    updAccount();
                }

            }
        }

        string strOper = "";
        private void Logs()
        {
            string query = "Insert into tblLogs(userId, LogsOper, LogsDate) values (@d1, @d2, @d3)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@d1", account.UserAccountId);
            cmd.Parameters.AddWithValue("@d2", strOper);
            cmd.Parameters.AddWithValue("@d3", DateTime.Now.ToString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //IF PRODUCT AND SUPPLIER NAME EXIST
        private void chkExist()
        {
            try
            {
                con.Open();
                string query = "select * from tblsupplier WHERE supplierName = '" + txtSName.Text + "' AND supplierProduct = '" + txtSProd.Text + "'";
                adpt = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
                con.Close();
            }
        }

        //ADD
        private void addAccount()
        {

            try
            {
                string query = "Insert into tblsupplier(supplierName, supplierAddress, supplierProduct) values (@d1, @d2, @d3)";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtSName.Text);
                cmd.Parameters.AddWithValue("@d2", txtSAdd.Text);
                cmd.Parameters.AddWithValue("@d3", txtSProd.Text);
                cmd.Parameters.AddWithValue("@d4", DateTime.Now.ToString());

                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("New Product added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                strOper = "Add New Supplier: ";
                Logs();
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        //UPDATE
        private void updAccount()
        {

            try
            {
                string query = "update tblsupplier set supplierName = @d1, supplierAddress = @d2, supplierProduct = @d3 where supplierId = " + account.SelectSupplierId;
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@d1", txtSName.Text);
                cmd.Parameters.AddWithValue("@d2", txtSAdd.Text);
                cmd.Parameters.AddWithValue("@d3", txtSProd.Text);

                con.Open();
                adpt = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                con.Close();
                MessageBox.Show("Supplier Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                strOper = "Update Supplier: ";
                Logs();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            //private void frmAddSupplier_FormClosing(object sender, FormClosingEventArgs e)
            //{
            //    account.IsSupplierAdd = false;
            //    account.IsSupplierEdit = false;
            //    this.Hide();
            //    e.Cancel = true;
          
 
            //}
        }
    }
}
