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
    public partial class frmReport_Print : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmReport_Print()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void prodName()
        {
            string query = "SELECT ProdDesc from tblproduct";
            cmd = new MySqlCommand(query, con);
            con.Open();
            adpt = new MySqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adpt.Fill(dt);
            int row = 0;
            cboxCat.Items.Add("---All---");
            while (row < dt.Rows.Count)
            {
                cboxCat.Items.Add(dt.Rows[row]["ProdDesc"].ToString());
                ++row;
            }
            con.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            con.Open();
            DataSet1 ds = new DataSet1();
            adpt = new MySqlDataAdapter("SELECT * FROM tblorder WHERE STR_TO_DATE(Order_TransDate, '%m/%d/%Y') BETWEEN '" + dtFrom.Value.Date.ToString("yyyy-MM-dd") + "' AND '" + dtTo.Value.Date.ToString("yyyy-MM-dd") +"'", con);
            adpt.Fill(ds.tblorder);

            reports_sales receipt = new reports_sales();

            TextObject text2 = (TextObject)receipt.ReportDefinition.Sections["Section1"].ReportObjects["Text18"];
            text2.Text = dtFrom.Value.ToShortDateString();

            TextObject text3 = (TextObject)receipt.ReportDefinition.Sections["Section1"].ReportObjects["Text15"];
            text3.Text = dtTo.Value.ToShortDateString();

            TextObject text4 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["prepared"];
            text4.Text = account.UserFullName;

            receipt.SetDataSource(ds);

            crpReports.ReportSource = receipt;
            crpReports.RefreshReport();
            con.Close();
        }

        private void frmReport_Print_Load(object sender, EventArgs e)
        {
            prodName();
        }

        private void selectAll()
        {
            con.Open();
            DataSet1 ds = new DataSet1();
            adpt = new MySqlDataAdapter("SELECT * FROM tblorder", con);
            adpt.Fill(ds.tblorder);

            reports_sales receipt = new reports_sales();

            TextObject text4 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["prepared"];
            text4.Text = account.UserFullName;

            receipt.SetDataSource(ds);

            crpReports.ReportSource = receipt;
            crpReports.RefreshReport();
            con.Close();
        }

        private void filterByName()
        {
            con.Open();
            DataSet1 ds = new DataSet1();
            adpt = new MySqlDataAdapter("SELECT * FROM tblorder WHERE ProdDesc = '" + cboxCat.Text + "'", con);
            adpt.Fill(ds.tblorder);

            reports_sales receipt = new reports_sales();

            TextObject text4 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["prepared"];
            text4.Text = account.UserFullName;

            receipt.SetDataSource(ds);

            crpReports.ReportSource = receipt;
            crpReports.RefreshReport();
            con.Close();
        }

        private void cboxCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboxCat.SelectedIndex == 0)
            {
                selectAll();
                btnSearch.Enabled = true;
            }
            else
            {
                filterByName();
                btnSearch.Enabled = false;
            }            
        }
    }
}
