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
    public partial class frmDbBackup : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        string strOper = "";
        public frmDbBackup()
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
            string file = "C:\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(connString.constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        MessageBox.Show("DB Backup Completed!");
                        conn.Close();
                    }
                }
            }

            //string _filename = "posfm" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Title = "Backup Database";
            //sfd.Filter = "SQL files (*.sql)|*.sql";
            //sfd.FileName = _filename;
            //if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    using (MySqlConnection conn = new MySqlConnection(connString.constring))
            //    {
            //        using (MySqlCommand cmd = new MySqlCommand())
            //        {
            //            using (MySqlBackup mb = new MySqlBackup(cmd))
            //            {
            //                try
            //                {
            //                    cmd.Connection = conn;
            //                    conn.Open();
            //                    mb.ExportToFile(sfd.FileName);
            //                    conn.Close();
            //                    strOper = "Backup Database";
            //                    Logs();
            //                    MessageBox.Show(this, "Database Backup Done!", "Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    this.Close();
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show(ex.Message, "Message");
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
