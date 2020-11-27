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
    public partial class costReportForm : Form
    {
        Class1 myInfo = new Class1();
        int ItemID;
        public costReportForm()
        {
            InitializeComponent();
        }
        public costReportForm(int ItemID)
        {
            this.ItemID = ItemID;
            InitializeComponent();
        }

        private void costReportForm_Load(object sender, EventArgs e)
        {
            costCrystalReport3 report = new costCrystalReport3();
            costDataSet ds = new costDataSet();
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            //string sql = "SELECT items.IDItem,items.NameItem FROM madatec.items;";
            string sqlItemsElements = "SELECT items.IDItem,items.NameItem,elements.NameElement,elements.IDElement,itemelement.Quantity from items inner join itemelement on items.IDItem = itemelement.IDItem inner join elements on itemelement.IDElement= elements.IDElement where items.IDItem = " + ItemID + ";";
            //end of sql statment
            using (con)
            {

                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlItemsElements, con);
                adapter.Fill(ds.Tables["DataTable1"]);
                //adapter.Fill(ds.DataTable1);
            
                

                report.SetDataSource(ds.Tables["DataTable1"]);
            }
            //DataTable dt = ds.DataTable1;
            foreach (DataRow row in ds.DataTable1) {
                //MessageBox.Show(Convert.ToString(row["Quantity"]));
                double quantity = Convert.ToDouble(row["Quantity"]);
                double price = myInfo.FindElementPrice(Convert.ToInt32(row["IDElement"]),System.DateTime.Today.Year,System.DateTime.Today.Month);
                
                double priceAvereg = quantity * price;
                row["PriceAvereg"] = priceAvereg;
            }
            //foreach (DataRow row in ds.DataTable1) {
            //    MessageBox.Show(row["PriceAvereg"].ToString());
            //}
                //for (int i =0; i < ds.DataTable1.Rows.Count; i++) { MessageBox.Show(ds.DataTable1[i][10].ToString); }
            report.SetDataSource(ds.Tables["DataTable1"]);
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();

            
            //this.reportViewer1.RefreshReport();
        }
    }
}
