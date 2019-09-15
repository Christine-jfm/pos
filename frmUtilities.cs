using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSFM
{
    public partial class frmUtilities : Form
    {
        public frmUtilities()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDbBackup frm = new frmDbBackup();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmDbRestore frm = new frmDbRestore();
            frm.ShowDialog();
        }
    }
}
