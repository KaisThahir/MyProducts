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
    public partial class GeneralReportForm : Form
    {
        Class1 myInfo = new Class1();
        DateTime startDate;
        DateTime endDate;
        public GeneralReportForm()
        {
            InitializeComponent();
        }
        public GeneralReportForm(DateTime From,DateTime To)
        {
            this.startDate = From;
            this.endDate = To;
            InitializeComponent();
        }

        private void GeneralReportForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show(" نواعم "+Convert.ToString( myInfo.totalBayOfType(startDate,endDate,"نواعم")));
            MessageBox.Show(" نفقات " + Convert.ToString(myInfo.totalBayOfType(startDate, endDate, "نفقات")));
            MessageBox.Show(" مكونات " + Convert.ToString(myInfo.totalBayOfType(startDate, endDate, "مكونات")));
            double totalGain = 0;
            double totalPureGain = 0;

            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string sql = "SELECT lists.IDList,lists.DateList,lists.Cache,lists.IDCustomer, invoices.IDItem,invoices.Price,invoices.Quantity,items.NameItem as SaledItem,customers.NameCustomer as CustomerName,invoices.Price*invoices.Quantity as total FROM lists inner join customers on lists.IDCustomer = customers.IDCustomer left join invoices ON lists.IDList = invoices.IDList left join items on invoices.IDItem = items.IDItem where lists.DateList between '" + myInfo.SqlDateFormat(startDate) + "' and '" + myInfo.SqlDateFormat(endDate) + "'; ";
            GeneralDataSet ds = new GeneralDataSet();
            //end of sql statment
            GeneralCrystalReport report = new GeneralCrystalReport();
            using (con)
            {
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                adapter.Fill(ds.Tables["SaleDataTable"]);

            }

            foreach (DataRow row in ds.SaleDataTable)
            {
                
                if (row["SaledItem"].ToString() != "") {
                    Int32 year = endDate.Year;
                    Int32 month = endDate.Month;
                    string itemName = row["SaledItem"].ToString();
                    double totalCost = myInfo.itemCost(itemName, year, month);
                    row["Cost"] = myInfo.itemCost(itemName, year, month) * Convert.ToDouble(row["Quantity"]);
                    row["Gain"] = Convert.ToDouble( row["Total"]) - Convert.ToDouble( row["Cost"]);
                    totalGain =totalGain + Convert.ToDouble( row["Gain"]);
                
                }

            }
            totalPureGain = totalGain - myInfo.totalBayOfType(startDate, endDate, "نواعم");
            double Nemes = myInfo.totalBayOfType(startDate, endDate, "نواعم");
            double Expenses=myInfo.totalBayOfType(startDate, endDate, "نفقات");
            MessageBox.Show("total Gain " + totalGain);
            MessageBox.Show("total pure Gain " + totalPureGain);
            ds.GeneralData.AddGeneralDataRow(Nemes,Expenses );
            //report.SetDataSource(ds.Tables["SaleDataTable"]);
            //report.SetDataSource(ds.Tables["GeneralData"]);
            report.SetDataSource(ds);
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
            
        }
    }
}
