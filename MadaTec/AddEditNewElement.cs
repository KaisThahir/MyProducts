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
    public partial class AddEditNewElement : Form
    {
        public AddEditNewElement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cmdstr = "INSERT INTO `madatec`.`elements` (`NameElement`, `StoredElement`, `type`) VALUES ('" + textBox1.Text + "', '" + textBox3.Text + "', '" + comboBox1.Text + "');";
            Class1 myinfo = new Class1();
            //string cmdstr = "INSERT INTO `madatec`.`shopes` (`NameShope`, `TellShope`, `Address`, `Balance`) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "');";
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("تمت الاضافة بنجاح");
            this.Close();

        }
    }
}
