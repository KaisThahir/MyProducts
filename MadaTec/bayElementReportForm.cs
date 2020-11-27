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

namespace MadaTec
{
    public partial class bayElementReportForm : Form
    {
        Int32 ElementID;
        Class1 myInfo = new Class1();
        public bayElementReportForm()
        {
            InitializeComponent();
        }
        public bayElementReportForm(Int32 Id)
        {
            ElementID = Id;
            InitializeComponent();
        }

        private void bayElementReportForm_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string sql = "SELECT elements.NameElement,elements.Type,bayelement.price,bayelement.Quantity,baylists.IDBay,baylists.ListNo,baylists.ListDate,shopes.NameShope,bayelement.Price*bayelement.Quantity as total FROM elements inner join bayelement on elements.IDElement = bayelement.IDElement inner join baylists ON bayelement.IDBay= baylists.IDBay inner join shopes on baylists.IDShope = shopes.IDShope where elements.IDElement= "+ElementID+" order by baylists.ListDate;";

            //end of sql statment
            using (con)
            {
                bayElementReporDataSet ds = new bayElementReporDataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                adapter.Fill(ds.Tables["DataTable1"]);
                //adapter.Fill(ds.DataTable1);
                bayElementCrystalReport report = new bayElementCrystalReport();
                report.SetDataSource(ds.Tables["DataTable1"]);
                //for (int i =0; i < ds.DataTable1.Rows.Count; i++) { MessageBox.Show(ds.DataTable1[i][10].ToString); }
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();

            }
        }
    }
}
