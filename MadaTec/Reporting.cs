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
    public partial class Reporting : Form
    {
        Class1 myInfo = new Class1();
        public Reporting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(textBoxCustomerName.Text == "")) {
                CustomerReportForm frm = new CustomerReportForm(Convert.ToString(myInfo.getIdOfCustomr(textBoxCustomerName.Text)));
                frm.Show();
            }

        }

        private void Reporting_Load(object sender, EventArgs e)
        {
            // انشاء قائمتين ذاتية للزبائن والمواد
            AutoCompleteStringCollection col1 = new AutoCompleteStringCollection();
            AutoCompleteStringCollection col2 = new AutoCompleteStringCollection();
            AutoCompleteStringCollection col3 = new AutoCompleteStringCollection();
            //استرداد قيم القائمتين اعلاه من قاعدة البيانات
            Class1 myInfo = new Class1();

            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "SELECT NameCustomer,NameItem,ModelItem,NameElement FROM madatec.customers,madatec.items,madatec.elements;";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                col1.Add(reader.GetString("NameCustomer"));
                col2.Add(reader.GetString("NameItem"));
                col3.Add(reader.GetString("NameElement"));
            }
            con.Close();

            textBoxCustomerName.AutoCompleteCustomSource = col1;
            textBoxItemName.AutoCompleteCustomSource = col2;
            textBoxElementName.AutoCompleteCustomSource = col3;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SalesReportForm openForm = new SalesReportForm(dateTimePickerFrom.Value,dateTimePickerTo.Value);
            openForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            costReportForm nForm = new costReportForm(myInfo.getIdOfItem(textBoxItemName.Text));
            nForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bayElementReportForm nForm = new bayElementReportForm(myInfo.getIdOfElement(textBoxElementName.Text));
            nForm.Show();
        }

        private void btnGeneral_Click(object sender, EventArgs e)
        {
            GeneralReportForm frm = new GeneralReportForm(dateTimePickerFrom2.Value,dateTimePickerTo2.Value);
            frm.Show();

        }
    }
}
