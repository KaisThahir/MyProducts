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
    public partial class Elements : Form
    {
        public Elements()
        {
            InitializeComponent();
        }
        public static int BayListID;
        public static int idShope;
        public static DateTime listDate;
        public static double listCashe;
        Int32 DataGridSelectedIndex;
        Int32 selectedElementID;
        Int32 selectedBayElementID;
        double total;
        double oldBalance;
        double newBalance;
        Class1 myInfo = new Class1();
        //public string ShopeName = "";
        // الكود التالي لاضافة المواد حيث تم نقل الموجودات الى فورم خاص
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Class1 myInfo = new Class1();
        //    MySqlConnection con = new MySqlConnection (myInfo.ConStr);
        //    string cmdstr = "insert into Elements (NameElement,PriceElement,StoredElement) values ('"+textBox1.Text +"',"+textBox2.Text+","+textBox3.Text+")";
        //    MySqlCommand cmd = new MySqlCommand(cmdstr, con);
        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();


        //}

        private void Elements_Load(object sender, EventArgs e)
        {

            dataGridView1.Columns.Add("", "المحل");
            dataGridView1.Columns.Add("", "التاريخ");
            dataGridView1.Columns.Add("", "الرقم");
            dataGridView1.Columns.Add("", "المبلغ");
            dataGridView1.Columns.Add("", "الرمز");
            dataGridView1.Rows.Add("");
            dataGridView2.Columns.Add("", "المتبقي");
            dataGridView2.Columns.Add("", "");
            dataGridView2.Columns.Add("", "المجموع");
            dataGridView2.Rows.Add();
            dateTimePicker1.Value = System.DateTime.Today;
            AutoCompleteStringCollection col1 = new AutoCompleteStringCollection();
            AutoCompleteStringCollection col2 = new AutoCompleteStringCollection();
            //Class1 myInfo = new Class1();
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "select NameShope from shopes";
            MySqlCommand cmdshopes = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmdshopes.ExecuteReader();
            while (reader.Read())
            {
                col1.Add(reader.GetString(0));
            }
            textBox5.AutoCompleteCustomSource = col1;
            con.Close();
            //col.Clear();
            string CmdElementStr = "select NameElement from Elements";
            MySqlCommand cmdElement = new MySqlCommand(CmdElementStr, con);
            con.Open();
            MySqlDataReader readerElement = cmdElement.ExecuteReader();
            while (readerElement.Read())
            {
                col2.Add(readerElement.GetString(0));
            }
            textBox9.AutoCompleteCustomSource = col2;
            textBox1.AutoCompleteCustomSource = col2;
            //col.Clear();

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void text_leav(object sender, EventArgs e)
        {
            MessageBox.Show("this");
        }
        private Boolean exisList()
        {
            Boolean isExist = false;
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdcheckstr = "select baylists.IDBay from baylists where IDShope = (select IDShope from shopes where NameShope = '" + textBox5.Text + "') and ListNo = '" + textBox4.Text + "';";
            //string cmdstr = "INSERT INTO `madatec`.`baylists` (`ListNo`, `IDShope`, `ListDate`,`Cashe`) VALUES ('" + textBox4.Text + "',(select IDShope from shopes where NameShope ='" + textBox5.Text + "'), '" + myInfo.SqlDateFormat(dateTimePicker1.Value) + "','" + textBox7.Text + "');select LAST_INSERT_ID();";

            MySqlCommand cmd = new MySqlCommand(cmdcheckstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                isExist = true;
                //MessageBox.Show("القائمة مدخلة");
                //formatDataGrid();
                //BayListID = reader.GetInt16("IDBay");
            }

            con.Close();
            return isExist;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //Boolean existList = false;
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "INSERT INTO `madatec`.`baylists` (`ListNo`, `IDShope`, `ListDate`,`Cashe`) VALUES ('" + textBox4.Text + "',(select IDShope from shopes where NameShope ='" + textBox5.Text + "'), '" + myInfo.SqlDateFormat(dateTimePicker1.Value) + "','" + textBox7.Text + "');select LAST_INSERT_ID();";
            if (exisList())
            {
                MessageBox.Show("القائمة مدخلة");
                GetDataToGrid();
            }
            con.Open();
            if (!exisList())
            {
                MySqlCommand cmd = new MySqlCommand(cmdstr, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BayListID = reader.GetInt16(0);
                }
            }
            con.Close();
            if (!exisList())
            {
                
                formatDataGrid();
                addToBalance(Convert.ToInt32(textBox7.Text) * -1);
            }
            textBox9.Focus();
        }

        private void addToBalance(int amount)
        {
            string upDateString = "UPDATE `madatec`.`shopes` SET `Balance`= Balance+'" + amount+"' WHERE `IDShope`='"+idShope+"';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(upDateString, con);
            con.Open();
            cmd.ExecuteNonQuery();                         
            con.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {

            InsertElementToDB();
            //searchList();
            int price = Convert.ToInt32(textBox8.Text);
            int quantity = Convert.ToInt32(textBox10.Text);
            int amount = price * quantity;
            fillDataGrid(textBox9.Text, price,quantity,selectedBayElementID);
            textBox9.Focus();
            textBox10.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            addToBalance(amount);
        }
        private void InsertElementToDB()
        {
            string cmdstr = "INSERT INTO `madatec`.`bayelement` (`IDBay`, `IDElement`, `price`, `Quantity`) VALUES('" + BayListID + "','"+myInfo.getIdOfElement(textBox9.Text)+"', '" + textBox8.Text + "', '" + textBox10.Text + "'); SELECT LAST_INSERT_ID();";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            //searchList();
            while (reader.Read())
            {
                selectedBayElementID = reader.GetInt32(0);
            }
            con.Close();
            updateElementStored(Convert.ToInt32( textBox10.Text), myInfo.getIdOfElement(textBox9.Text));
            //fillDataGrid(textBox9.Text, Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox10.Text),selectedElementID);
        }
        private void updateElementStored(int no,int elementId)
        {
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "update elements set StoredElement = StoredElement + " + no + " where IDElement =" + elementId + ";";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {


        }

        private void text5Leave(object sender, EventArgs e)
        {
            //dateTimePicker1.Focus(); // تم الغاء هذا السطر لانه يسب تكرار عملية عرض الرسالة

            Boolean GoToShope = false;
            //MessageBox mymesag = new MessageBox("لا يوجد محل بهذا الاسم الرجاء قم باضافة بياناته");
            //Class1 myinfo = new Class1();
            string cmdstr = "SELECT NameShope,IDShope FROM madatec.shopes where NameShope ='" + textBox5.Text + "';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader chekreader = cmd.ExecuteReader();
            if (!chekreader.Read())
            {
                MessageBox.Show("لا يوجد محل بهذا الاسم الرجاء قم باضافة بياناته");
                GoToShope = true;
            }
            else
            {
                idShope = chekreader.GetInt32("IDShope");
            }
            con.Close();

            if (GoToShope)
            {
                shops newform = new shops();
                newform.textBox1.Text = textBox5.Text;
                newform.Show();
            }
           
        }

        private void text9Leave(object sender, EventArgs e)
        {
            if (textBox9.Text != "") { 

            Boolean GoToElement = false;
            //MessageBox mymesag = new MessageBox("لا يوجد محل بهذا الاسم الرجاء قم باضافة بياناته");
            //Class1 myinfo = new Class1();
            string cmdstr = "SELECT NameElement FROM madatec.elements where NameElement = '" + textBox9.Text + "';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader chekreader = cmd.ExecuteReader();
            if (!chekreader.Read())
            {
                MessageBox.Show("لا يوجد مادة بهذا الاسم الرجاء قم باضافة البيانات");
                GoToElement = true;
            }
            con.Close();

            if (GoToElement)
            {
                AddEditNewElement newform = new AddEditNewElement();
                newform.textBox1.Text = textBox9.Text;
                newform.textBox3.Text = "0";
                newform.Show();
            }
            }
            textBox8.Focus();
        }

        private void text7_leav(object sender, EventArgs e)
        {
            button3.Focus();
        }

        private void text10_leav(object sender, EventArgs e)
        {
            button2.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox3.Enabled = true;
        }
        private void fillDataGrid(string name, int price, int quantity,int id)
        {
            dataGridView1.Rows.Add();
            dataGridView1[0, dataGridView1.Rows.Count - 1].Value = name;
            dataGridView1[1, dataGridView1.Rows.Count - 1].Value = price;
            dataGridView1[2, dataGridView1.Rows.Count - 1].Value = quantity;
            double amount = price * quantity;
            total = total + amount;
            dataGridView1[3, dataGridView1.Rows.Count - 1].Value = amount;
            dataGridView1[4, dataGridView1.Rows.Count - 1].Value = id;
            dataGridView2[2, 0].Value = total;
            dataGridView2[0, 0].Value = findBalance(textBox5.Text);
        }
        private void formatDataGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            dataGridView1[2, 0].Value = textBox4.Text;
            dataGridView1[0, 0].Value = textBox5.Text;
            dataGridView1[1, 0].Value = dateTimePicker1.Value.ToString();
            //
            dataGridView1.Rows.Add();
            dataGridView1[2, 1].Value = "#######";
            dataGridView1[0, 1].Value = "#######";
            dataGridView1[1, 1].Value = "#######";
            //
            dataGridView1.Rows.Add();
            dataGridView1[2, 2].Value = "العدد";
            dataGridView1[0, 2].Value = "المادة";
            dataGridView1[1, 2].Value = "السعر";
            //cmd.ExecuteNonQuery();
            textBox9.Focus();
        }

        private void GetDataToGrid()
        {
            dataGridView1.Rows.Clear();
            formatDataGrid();
            string cmdcheckstr2 = "select elements.NameElement,bayelement.price,bayelement.Quantity,baylists.IDBay,bayelement.ID from baylists,bayelement,elements where IDShope = '"+myInfo.getIdOfShope(textBox5.Text)+"' and ListNo = '" + textBox4.Text + "' and baylists.IDBay= bayelement.IDBay and bayelement.IDElement= elements.IDElement;";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdcheckstr2, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fillDataGrid(reader.GetString("NameElement"), reader.GetInt32("price"), reader.GetInt32("Quantity"), reader.GetInt32("ID"));
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridSelectedIndex = Convert.ToInt16(dataGridView1.CurrentRow.Index);
            textBox1.Text = dataGridView1[0, DataGridSelectedIndex].Value.ToString();
            textBox2.Text = dataGridView1[1, DataGridSelectedIndex].Value.ToString();
            textBox3.Text = dataGridView1[2, DataGridSelectedIndex].Value.ToString();
            selectedBayElementID =Convert.ToInt32( dataGridView1[4, DataGridSelectedIndex].Value);
            string elementName = dataGridView1[0, DataGridSelectedIndex].Value.ToString();
            Class1 myinfo = new Class1();
            selectedElementID= myinfo.getIdOfElement(elementName);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index > 2)
            {
                int oldNo = Convert.ToInt32(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
                int newNo = Convert.ToInt32( textBox3.Text);
                MessageBox.Show(oldNo.ToString()+" to "+newNo.ToString());
                MySqlConnection con = new MySqlConnection(myInfo.ConStr);
                string cmdstr = "UPDATE `madatec`.`bayelement` SET `IDElement`='"+myInfo.getIdOfElement(textBox1.Text)+"', `price`='"+textBox2.Text+"', `Quantity`='"+textBox3.Text+"' WHERE ID ='"+selectedBayElementID+"';";
                MySqlCommand cmd = new MySqlCommand(cmdstr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetDataToGrid();
                updateElementStored(newNo - oldNo, myInfo.getIdOfElement(textBox1.Text));
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int noToDecreasment = Convert.ToInt32(dataGridView1[2, dataGridView1.CurrentRow.Index].Value) * -1;
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "DELETE FROM `madatec`.`bayelement` WHERE ID ='" + selectedBayElementID + "';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            GetDataToGrid();
            //MessageBox.Show(noOfElement.ToString() + " from " + selectedElementID);
            updateElementStored(noToDecreasment,selectedElementID);
        }

        private void dateTimePiker1Leave(object sender, EventArgs e)
        {
            textBox7.Focus();
        }

        private void button3_Leave(object sender, EventArgs e)
        {
            textBox9.Focus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            total = 0;
            formatDataGrid();
            //searchListId();//الغاء تفعيل ها السطر مؤقتا بالنظر للملاحظة المكتوبة في بداية الدالة 
            searchWichList();
            MessageBox.Show(BayListID.ToString());
           //BayListID = reader.GetInt16("IDBay");
            if (exisList())
            {

            }

        }

        private void searchWichList()
        {
            searchedBayList.listNo = Convert.ToInt32( textBox4.Text);
            Form search = new searchedBayList();
            search.Show();
            //this.Close();
            
        }
        public void searchList()
        {
            // اعتقد ان ستراتيجية عمل هذه الدالة خطاء سيتم استبدالها بالدالة searchForList()
            string cmdstr = "SELECT * FROM madatec.baylists,madatec.shopes,madatec.bayelement,elements where baylists.IDBay ='"+BayListID+"' and baylists.IDShope= shopes.IDShope and bayelement.IDBay= baylists.IDBay and bayelement.IDElement= elements.IDElement ;";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox5.Text = reader.GetString("NameShope");
                dateTimePicker1.Value = reader.GetDateTime("ListDate");
                textBox7.Text = reader.GetString("Cashe");
                fillDataGrid(reader.GetString("NameElement"), reader.GetInt32("price"), reader.GetInt32("Quantity"), reader.GetInt32("ID"));
            }
            con.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //searchedBayList();
            textBox5.Text = myInfo.getNameOfShope(idShope);
            dateTimePicker1.Value = listDate;
            textBox7.Text = listCashe.ToString();
            searchList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("idlist is:" + BayListID);
            string cmdstr = "UPDATE `madatec`.`baylists` SET `ListNo`='"+textBox4.Text+"', `IDShope`='"+myInfo.getIdOfShope(textBox5.Text)+"', `ListDate`='"+myInfo.SqlDateFormat(dateTimePicker1.Value)+"', `Cashe`='"+textBox7.Text+"' WHERE `IDBay`='"+BayListID+"';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("هل انت متاكد من حذف القيد!!");
            string cmdstr = "DELETE FROM `madatec`.`baylists` WHERE `IDBay`='"+BayListID+"';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            //لمسح محتويات القائمة (التفاصيل)
            for (int i = 3; i< dataGridView1.Rows.Count; i++)
            {
                cmdstr = "DELETE FROM `madatec`.`bayelement` WHERE `ID`='" + dataGridView1[4, i].Value + "';";
                cmd = new MySqlCommand(cmdstr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                updateElementStored(Convert.ToInt32( dataGridView1[2, i].Value)*-1,myInfo.getIdOfElement(dataGridView1[0,i].Value.ToString()));
            }
        }

        private void text4_leave(object sender, EventArgs e)
        {
            if (!button6.Focused) { textBox5.Focus(); }
        }

        private void text8_leave(object sender, EventArgs e)
        {
            textBox10.Focus();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "") { 
            findBalance(textBox5.Text);
            }
            else
            {
                MessageBox.Show("رجاء قم باختيار محل");
            }
        }

        public int findBalance(string shopName)
        {
            int balance = 0;
            int cashes = 0;
            string cmdAmounts = "select price,Quantity from bayelement where IDBay in (select baylists.IDBay from baylists where IDShope =(select IDShope from shopes where NameShope='"+shopName+"'));";
            string cmdCashes = "select cashe from baylists where IDShope = (select IDShope from shopes where NameShope='"+shopName+"');";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdAmounts, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                balance = balance + (reader.GetInt32("price") * reader.GetInt32("Quantity"));
            }
            con.Close();
            cmd.CommandText = cmdCashes;
            con.Open();
            MySqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                cashes = cashes + reader2.GetInt32("cashe");
            }
            con.Close();
            balance = balance - cashes;
            //MessageBox.Show(balance.ToString());
            return balance;
        }
    }
}
