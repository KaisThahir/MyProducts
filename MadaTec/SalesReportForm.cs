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
    public partial class SalesReportForm : Form
    {
        Class1 myInfo = new Class1();
        DateTime startDay;
        DateTime endDay;
        public SalesReportForm()
        {
            InitializeComponent();
        }
        public SalesReportForm(DateTime startDay, DateTime endDay)
        {
            this.startDay = startDay;
            this.endDay = endDay;
            InitializeComponent();
        }

        private void SalesReportForm_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string sql = "SELECT lists.IDList,lists.DateList,lists.Cache,lists.IDCustomer, invoices.IDItem,invoices.Price,invoices.Quantity,items.NameItem,customers.NameCustomer,invoices.Price*invoices.Quantity as total FROM lists inner join customers on lists.IDCustomer = customers.IDCustomer left join invoices ON lists.IDList = invoices.IDList left join items on invoices.IDItem = items.IDItem where lists.DateList between '"+myInfo.SqlDateFormat(startDay)+ "' and '" + myInfo.SqlDateFormat(endDay) + "'; ";

            //end of sql statment
            using (con)
            {
                saleReportDataSet ds = new saleReportDataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                adapter.Fill(ds.Tables["DataTable1"]);
                //adapter.Fill(ds.DataTable1);
                salesCrystalReport3 report = new salesCrystalReport3();
                report.SetDataSource(ds.Tables["DataTable1"]);
                //for (int i =0; i < ds.DataTable1.Rows.Count; i++) { MessageBox.Show(ds.DataTable1[i][10].ToString); }
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();

            }
        }
    }
}
