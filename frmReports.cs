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
    public partial class frmReports : Form
    {
        MySqlConnection con = new MySqlConnection();
        MySqlCommand cmd;
        MySqlDataAdapter adpt;
        public frmReports()
        {
            InitializeComponent();
            con.ConnectionString = connString.constring;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            DataSet1 ds = new DataSet1();
            adpt = new MySqlDataAdapter("SELECT * FROM tblorder ORDER BY Order_Id ASC", con);
            adpt.Fill(ds.tblorder);

            //adpt = new MySqlDataAdapter("SELECT * FROM tblaccount where userId = " + account.UserAccountId + "", con);
            //adpt.Fill(ds.tblaccount);

            reports_sales receipt = new reports_sales();
            frmReport_Print rpt = new frmReport_Print();

            int count = ds.Tables[0].Rows.Count;
            DataRow dr = ds.Tables[0].Rows[0];
            DataRow dr2 = ds.Tables[0].Rows[count-1];
            TextObject text2 = (TextObject)receipt.ReportDefinition.Sections["Section1"].ReportObjects["Text18"];
            TextObject text3 = (TextObject)receipt.ReportDefinition.Sections["Section1"].ReportObjects["Text15"];
            if (count > 0)
            {
                DateTime dateAndTime1 = DateTime.Parse(dr["Order_TransDate"].ToString());
                DateTime dateAndTime2 = DateTime.Parse(dr2["Order_TransDate"].ToString());
                text2.Text = dateAndTime1.ToShortDateString();
                text3.Text = dateAndTime2.ToShortDateString();
            }
            else
            {
                text2.Text = "--/--/----";
                text3.Text = "--/--/----";
            }

            TextObject text4 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["prepared"];
            text4.Text = account.UserFullName;

            receipt.SetDataSource(ds);

            rpt.crpReports.ReportSource = receipt;
            rpt.crpReports.RefreshReport();

            rpt.label1.Visible = true;
            rpt.label2.Visible = true;
            rpt.dtFrom.Visible = true;
            rpt.dtTo.Visible = true;
            rpt.btnSearch.Visible = true;
            rpt.label3.Visible = true;
            rpt.cboxCat.Visible = true;

            //account.Report_type = "Sales";
            rpt.Show(this);
            con.Close();

            //strLogs = "View Sales Report";
            //Logs2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            DataSet1 ds = new DataSet1();
            adpt = new MySqlDataAdapter("SELECT * FROM tblproduct ORDER BY ProdId ASC", con);
            adpt.Fill(ds.tblproduct);

            reports_Stocks receipt = new reports_Stocks();
            TextObject text4 = (TextObject)receipt.ReportDefinition.Sections["Section4"].ReportObjects["prepared"];
            text4.Text = account.UserFullName;
            receipt.SetDataSource(ds);

            frmReport_Print rpt = new frmReport_Print();
            rpt.crpReports.ReportSource = receipt;
            rpt.crpReports.RefreshReport();

            //account.Report_type = "Product";
            rpt.Show(this);
            con.Close();

            //strLogs = "View Stocks Report";
            //Logs2();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {

        }
    }
}
