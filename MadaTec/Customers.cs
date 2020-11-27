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
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 myinfo = new Class1();
            string cmdstr = "INSERT INTO `madatec`.`customers` (`NameCustomer`, `TelCustomer`, `AddressCustomer`) VALUES ('"+textBox1.Text+"', '"+textBox2.Text+"', '"+textBox3.Text+"');SELECT LAST_INSERT_ID();";
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            
            con.Close();
            MessageBox.Show("تمت الاضافة بنجاح");
            this.Close();

        }

        private void Customers_Load(object sender, EventArgs e)
        {
           
        }
    }
}
