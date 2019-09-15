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
using System.Diagnostics;

namespace POSFM
{
    public partial class frmDbRestore : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        string strOper = "";

        public frmDbRestore()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }
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

        private void btnBackSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog _fd = new OpenFileDialog();
            _fd.Filter = "Database Backup Files (*.sql)|*.sql";
            _fd.DefaultExt = "sql";

            if (_fd.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(_fd.FileName))
            {
                txtRestore.Text = _fd.FileName;
            }

            _fd.Dispose();
            _fd = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file = "C:\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(connString.constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        MessageBox.Show("DB Restore Completed!");
                        conn.Close();
                    }
                }
            }
        }

        //using (MySqlConnection conn = new MySqlConnection(connString.constring))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand())
        //    {
        //        using (MySqlBackup mb = new MySqlBackup(cmd))
        //        {
        //            cmd.Connection = conn;
        //            conn.Open();
        //            mb.ImportFromFile(txtRestore.Text);
        //            conn.Close();
        //            strOper = "Restore Database";
        //            Logs();
        //            MessageBox.Show(this, "Database Restore Done!", "Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            this.Close();
        //        }
        //    }
        //}
    }
}
