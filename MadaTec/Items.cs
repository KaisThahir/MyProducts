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
    public partial class Items : Form
    {
        public Items()
        {
            InitializeComponent();
        }
        Int16 ItemID;
        Int16 DataGridSelectedIndex;
        double cost;
        private void button1_Click(object sender, EventArgs e)
        {
            string cmdstr = "INSERT INTO `madatec`.`items` (`NameItem`, `ModelItem`, `PriceItem`, `Note`) VALUES ('"+textBox1.Text+"', '"+textBox3.Text+"', '"+textBox4.Text+"', '"+textBox5.Text+ "');select LAST_INSERT_ID();";
            Class1 myinfo = new Class1();
            MySqlConnection con = new MySqlConnection( myinfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ItemID = reader.GetInt16(0);
            }
            con.Close();
            MessageBox.Show("تمت الاضافة بنجاح يرجى ادراج المكونات");
        }

        private void Items_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("", "المادة");
            dataGridView1.Columns.Add("", "الكمية");
            dataGridView1.Columns.Add("", "الملاحظات");
            comboBox1.Items.Clear();
            AutoCompleteStringCollection col1 = new AutoCompleteStringCollection();
            AutoCompleteStringCollection col2 = new AutoCompleteStringCollection();
            Class1 myInfo = new Class1();
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "SELECT NameElement,NameItem FROM madatec.elements,madatec.items;";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                col1.Add(reader.GetString(0));
                col2.Add(reader.GetString(1));
                comboBox1.Items.Add(reader.GetString(0));
            }
            textBox6.AutoCompleteCustomSource = col1;
            textBox1.AutoCompleteCustomSource = col2;
            con.Close();
        }

        private void combo1ItemSelected(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Class1 myinfo = new Class1();
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            string cmdstr = "INSERT INTO `madatec`.`itemelement` (`IDItem`, `IDElement`, `Quantity`, `Note`) VALUES ('" + ItemID + "',(select IDElement from madatec.elements where elements.NameElement='" + textBox6.Text + "'), '" + textBox7.Text + "', '" + textBox8.Text + "');";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            FillDataGrid(textBox6.Text, Convert.ToDouble(textBox7.Text), textBox8.Text);
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox6.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string cmdStr = "SELECT * FROM madatec.items where NameItem = '"+textBox1.Text+"';";
            Class1 myInfo = new Class1();
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdStr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader.GetString("NameItem");
                textBox3.Text = reader.GetString("ModelItem");
                textBox4.Text = reader.GetString("PriceItem");
                textBox5.Text = reader.GetString("Note");
                ItemID = reader.GetInt16("IDItem");
            }
            con.Close();
            string cmdStr2 = "SELECT * FROM madatec.itemelement,elements where itemelement.IDElement= elements.IDElement and IDItem = " + ItemID+";";
            MySqlCommand cmd2 = new MySqlCommand(cmdStr2, con);
            con.Open();
            reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                FillDataGrid(reader.GetString("NameElement"), reader.GetDouble("Quantity"), reader.GetString("Note"));
            }
            con.Close();
            textBox2.Text = Convert.ToString(myInfo.itemCost(textBox1.Text,System.DateTime.Today.Year,System.DateTime.Today.Month));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Class1 myinfo = new Class1();
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            for(int i=0; i < dataGridView1.Rows.Count; i++)
            {
                string cmdstr = "INSERT INTO `madatec`.`itemelement` (`IDItem`, `IDElement`, `Quantity`, `Note`) VALUES ('"+ItemID+"',(select IDElement from madatec.elements where elements.NameElement='"+dataGridView1[0,i].Value+"'), '"+ dataGridView1[1, i].Value + "', '"+ dataGridView1[2, i].Value + "');";
                MySqlCommand cmd = new MySqlCommand(cmdstr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridSelectedIndex = Convert.ToInt16( dataGridView1.CurrentRow.Index);
            textBox9.Text = dataGridView1[0, DataGridSelectedIndex].Value.ToString();
            textBox10.Text = dataGridView1[1, DataGridSelectedIndex].Value.ToString();
            textBox11.Text = dataGridView1[2, DataGridSelectedIndex].Value.ToString();
        }

        private void FillDataGrid (Int16 indx, string ItemName,double Quantity,string Note)
        {
            dataGridView1.Rows.Add();
            dataGridView1[0, indx].Value = ItemName;
            dataGridView1[1, indx].Value = Quantity;
            dataGridView1[2, indx].Value = Note;
        }
        private void FillDataGrid( string ItemName, double Quantity, string Note)
        {
            dataGridView1.Rows.Add();
            dataGridView1[0, dataGridView1.Rows.Count - 1].Value = ItemName;
            dataGridView1[1, dataGridView1.Rows.Count - 1].Value = Quantity;
            dataGridView1[2, dataGridView1.Rows.Count - 1].Value = Note;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FillDataGrid(DataGridSelectedIndex, textBox9.Text, Convert.ToDouble(textBox10.Text), textBox11.Text);
            Class1 myinfo = new Class1();
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            string cmdstr = "update itemelement set Quantity= "+textBox10.Text+",Note ='"+textBox11.Text+"' where IDItem = "+ItemID+ " and IDElement = (select IDElement from elements where NameElement = '"+textBox9.Text+"');";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Class1 myinfo = new Class1();
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            string cmdstr = "DELETE FROM `madatec`.`itemelement` WHERE IDItem =" + ItemID + " and IDElement = (select IDElement from elements where NameElement = '" + textBox9.Text + "');";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            double price = 0;
            Class1 myinfo = new Class1();
            int elementid = myinfo.getIdOfElement(textBox9.Text);
            price= myinfo.FindElementPrice(elementid, dateTimePicker1.Value.Year, dateTimePicker1.Value.Month);
            MessageBox.Show(Convert.ToString(price));
            
        }

        //private double findCost()
        //{
        //    Class1 myinfo = new Class1();
        //    cost = myinfo.itemCost(textBox1.Text,System.DateTime.Today.Year,System.DateTime.Today.Month);
        //    return cost;

        //}

        private void text6_leave(object sender, EventArgs e)
        {
            textBox7.Focus();
        }

        private void text7_leave(object sender, EventArgs e)
        {
            textBox8.Focus();
        }

        private void text8_leave(object sender, EventArgs e)
        {
            button5.Focus();
        }

 
    }
}
