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
    public partial class CustomerReportForm : Form
    {
        Int64 CustomerID;
        Class1 myInfo = new Class1();
        public CustomerReportForm()
        {
            InitializeComponent();
        }
        public CustomerReportForm(string customerID)
        {
            InitializeComponent();
            this.CustomerID = Convert.ToInt64(customerID);
        }

        private void CustomerReportForm_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string sql = "SELECT lists.IDList,lists.DateList,lists.Cache,invoices.Price,invoices.Quantity,items.NameItem,customers.NameCustomer,invoices.Price*invoices.Quantity as Total FROM lists inner join customers on lists.IDCustomer = customers.IDCustomer left join invoices ON lists.IDList = invoices.IDList left join items on invoices.IDItem = items.IDItem where lists.IDCustomer = " + CustomerID + "; ";

            //end of sql statment
            using (con) {
                CustomerReportDataSet ds = new CustomerReportDataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                adapter.Fill(ds.Tables["DataTable1"]);
                //adapter.Fill(ds.DataTable1);
                CustomerCrystalReport3 report = new CustomerCrystalReport3();
                report.SetDataSource(ds.Tables["DataTable1"]);
               //for (int i =0; i < ds.DataTable1.Rows.Count; i++) { MessageBox.Show(ds.DataTable1[i][10].ToString); }
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }

        }
    }
}
