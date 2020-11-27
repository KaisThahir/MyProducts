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
using Microsoft.Office.Interop.Excel;
using __Excel = Microsoft.Office.Interop.Excel;

namespace MadaTec
{
    public partial class CelList : Form
    {
        public CelList()
        {
            InitializeComponent();
        }
        Int32 IDList;
        Int32 IDCustomer;
        double OldBalance;
        double newBalance;
        double total = 0;
        Class1 myInfo = new Class1();

        private void CelList_Load(object sender, EventArgs e)
        {
            //تهيئة الجدول
            dataGridView1.Columns.Add("item", "المادة");
            dataGridView1.Columns.Add("price", "السعر");
            dataGridView1.Columns.Add("quantity", "الكمية");
            dataGridView1.Columns.Add("amount", "المبلغ");
            dataGridView1.Columns.Add("amount", "id");
            dataGridView1.Rows.Add();

            // انشاء قائمتين ذاتية للزبائن والمواد
            AutoCompleteStringCollection col1 = new AutoCompleteStringCollection();
            AutoCompleteStringCollection col2 = new AutoCompleteStringCollection();
            //استرداد قيم القائمتين اعلاه من قاعدة البيانات
            Class1 myInfo = new Class1();

            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "SELECT NameCustomer,NameItem,ModelItem FROM madatec.customers,madatec.items;";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                col1.Add(reader.GetString("NameCustomer"));
                col2.Add(reader.GetString("NameItem"));
            }
            con.Close();

            textBox1.AutoCompleteCustomSource = col1;
            textBox4.AutoCompleteCustomSource = col2;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            insertList();
            textBox4.Focus();

        }
        private void insertList()
        {
            string cmdstr = "INSERT INTO `madatec`.`lists` (`NoList`, `DateList`, `IDCustomer`, `Cache`, `Note`) VALUES ('0','" + myInfo.SqlDateFormat(dateTimePicker1.Value) + "', "+ IDCustomer +", '" + textBox7.Text + "', ' ');select LAST_INSERT_ID();";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                IDList = reader.GetInt32(0);
            }
            con.Close();
            textBox3.Text = IDList.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!(IDList > 0)) { insertList(); }//اضافة قائمة في قاعدة بيانات اذا لم يتم تحديد قائمة
            FillDataGrid(textBox4.Text, Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox6.Text),0);// املاء الجدول
            
            insertItemToDBInvoice();// اضافة المادة لجدول انفويز في قاعدة البيانات
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox4.Focus();
        }
        private void FillDataGrid(string item, int price, int quantity, int id)
        {
            // في هذه الدالة فقط املاء الجدول لفقرة واحد بكل استدعاء
            dataGridView1.Rows.Add();
            dataGridView1[0, dataGridView1.Rows.Count - 2].Value = item;
            dataGridView1[1, dataGridView1.Rows.Count - 2].Value = price;
            dataGridView1[2, dataGridView1.Rows.Count - 2].Value = quantity;
            dataGridView1[3, dataGridView1.Rows.Count - 2].Value = price * quantity;
            dataGridView1[4, dataGridView1.Rows.Count - 2].Value = id;

            total = total + (price * quantity);
            dataGridView1[2, dataGridView1.Rows.Count - 1].Value = "المجموع";
            dataGridView1[3, dataGridView1.Rows.Count - 1].Value = total;
        }
        private void FillDataGrid(int indx, string item, int price, int quantity)
        {
            dataGridView1[0, indx].Value = item;
            dataGridView1[1, indx].Value = price;
            dataGridView1[2, indx].Value = quantity;
            dataGridView1[3, indx].Value = price * quantity;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(IDList > 0)) { insertList(); }//اضافة قائمة في قاعدة بيانات اذا لم يتم تحديد قائمة
            newBalance = calculateBalance();
            // لتحديث رصيد الزبون
            // تم الغاء حقل رصيد الزبون من قاعدة
            //البيانات لذلك الكود ادناه لا يعمل وتم الاعتماد على دالة بالكلاس لايجاد رصيد الزبون 
            //MySqlConnection con2 = new MySqlConnection(myInfo.ConStr);
            //string cmdstr2 = "UPDATE `madatec`.`customers` SET `BalanceCustomer`='" + newBalance + "' WHERE `IDCustomer`='" + IDCustomer + "';";
            //MySqlCommand cmd2 = new MySqlCommand(cmdstr2, con2);
            //con2.Open();
            //cmd2.ExecuteNonQuery();
            //con2.Close();
            printList();
            dataGridView1.Rows.Clear();
            

        }
       
        private void text1_leave(object sender, EventArgs e)
        {
           
            if (textBox1.Text != "") { 
            IDCustomer = myInfo.getIdOfCustomr(textBox1.Text);
            if(IDCustomer == 0)
            {
                MessageBox.Show("هذا الزبون غير مسجل!   رجائا قم باضافته");
                Customers newcustomer = new Customers();
                newcustomer.textBox1.Text = textBox1.Text;
                newcustomer.Show();
                    //نحتاج الى ستراكت او اوبجكت ةقتي ويكون مشارك لخزن معلومات الزبون الجديد
                    // struct newcustomer 
                    //جاري دراسة الموضوع
                    //تم ايقاف هذه النافذه عند وجود زبون جديد لضمان تحميل بياناته عند فتحها ثانية
                    this.Close();
                    
            }
            
            IDCustomer = myInfo.getIdOfCustomr(textBox1.Text);
            OldBalance = myInfo.findCustomerBalance(IDCustomer);
            textBox2.Text = OldBalance.ToString();
            }
            dateTimePicker1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //formating to use excel

            string path = "E:\\mada tech\\test.xlsx";
            Workbook wb;
            Worksheet ws;
            __Excel.Application excel2 = new __Excel.Application();

            wb = excel2.Workbooks.Open(path);
            ws = wb.Worksheets[1];


            writeCell(3, 2, wb, ws, excel2,"test");

        }

        private double calculateBalance()
        {
            string CustomerName = textBox1.Text;
            double cache = Convert.ToDouble(textBox7.Text);
            double insertToBalance = cache - total;
            double newBalancenow = OldBalance + insertToBalance;
            return newBalancenow;

        }

        private void printList()
        {
            MessageBox.Show("printing");
            string path = "E:\\mada tech\\";
            __Excel.Application excel = new __Excel.Application();
            Workbook wb = excel.Workbooks.Open(path+ "form.xlsx");
            Worksheet ws = wb.Worksheets[1];
             writeCell(3, 6, wb, ws, excel, IDList.ToString());// رقم القائمة
            writeCell(4, 3, wb, ws, excel, textBox1.Text);// اسم الزبون
            writeCell(5, 3, wb, ws, excel, dateTimePicker1.Value.ToString());//التاريخ
            writeCell(20, 5, wb, ws, excel,OldBalance.ToString());//الدين السابق
            writeCell(19, 5, wb, ws, excel, textBox7.Text);// الواصل
            writeCell(21, 5, wb, ws, excel,newBalance.ToString());//المتبقي
            if (dataGridView1.Rows.Count > 0) { 
                writeCell(18, 5, wb, ws, excel, total.ToString());//مجموع القائمة الحالية
                for (int i = 8; i < dataGridView1.Rows.Count +7; i++)
                {
                    writeCell(i, 2, wb, ws, excel, dataGridView1[0,i-8].Value.ToString());//المادة
                    writeCell(i, 3, wb, ws, excel, dataGridView1[2, i - 8].Value.ToString());//العدد
                    writeCell(i, 4, wb, ws, excel, dataGridView1[1, i - 8].Value.ToString());//السعر
                }
            }
            saveAS(wb, path);
            System.Diagnostics.Process.Start(path);
        }

        private void getListFromDatagridToExcel()
        {
            //writeCell(3, 6, wb, ws, excel, textBox3.Text);
        }
        private string readCell(int i, int j, Worksheet ws)
        {
            i++;
            j++;
            if (ws.Cells[i, j].value2 != null)
            {
                return ws.Cells[i, j].value;
            }
            else
            {
                return "";
            }
        }

        private void writeCell(int i, int j, Workbook wb, Worksheet ws, _Application excel, string text)
        {
            ws.Cells[i, j].value2 = text;
        }
        private void saveAS(Workbook wb,string path)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = path;
            sfd.FileName = textBox1.Text + textBox3.Text;
            sfd.ShowDialog();
            string fileName = sfd.FileName;

            wb.SaveAs(fileName);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            printList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            searchAndFillDG(Convert.ToInt32(textBox9.Text));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            MessageBox.Show(total.ToString());
            double oldtotal = total;
            chekIfChanged();
            searchAndFillDG(Convert.ToInt32(textBox9.Text));
            MessageBox.Show(total.ToString());
            double insertToBalance = total - oldtotal;
            calculateBalance();
        }
        private void searchAndFillDG(int SearchedList)
        {
            total = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            string cmdStr = "SELECT * FROM madatec.invoices,madatec.lists,customers,items where invoices.IDList= lists.IDList and lists.IDCustomer= customers.IDCustomer and invoices.IDItem= items.IDItem and lists.IDList =" + SearchedList + " ;";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdStr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // reader contain the fields of lists and invoice with this idlist
                IDList = SearchedList;
                textBox1.Text = reader.GetString("NameCustomer");
                dateTimePicker1.Value = reader.GetDateTime("DateList");
                textBox3.Text = IDList.ToString();
                textBox7.Text = reader.GetString("Cache");
                IDCustomer = reader.GetInt32("IDCustomer");
                textBox2.Text = myInfo.findCustomerBalance(IDCustomer).ToString();
                FillDataGrid(reader.GetString("NameItem"), reader.GetInt32("Price"), reader.GetInt32("Quantity"), reader.GetInt32("IDInvoice"));

            }
            con.Close();
        }
        private void chekIfChanged()
        {

            List<string> ItemNames = new List<string>();
            List<int> InvoisIds1 = new List<int>();
            List<int> quantitys = new List<int>();
            List<int> InvoisIds2 = new List<int>();
            List<int> prices = new List<int>();
            List<int> InvoisIds3 = new List<int>();

            Int16 indx = 0;
            Boolean NameChange = false;
            Boolean DateChange = false;
            Boolean CacheChange = false;
            string cmdStr = "SELECT * FROM madatec.invoices,madatec.lists,customers,items where invoices.IDList= lists.IDList and lists.IDCustomer= customers.IDCustomer and invoices.IDItem= items.IDItem and lists.IDList =" + textBox9.Text + " ;";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdStr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        while (reader.Read())
            {
                // فحص التغيرات الرئيسية في القائمة
                if (textBox1.Text != reader.GetString("NameCustomer"))
                {
                    NameChange = true;
                }

                if (dateTimePicker1.Value != reader.GetDateTime("DateList"))
                {
                    DateChange = true;
                }
               
                if (textBox7.Text != reader.GetString("Cache"))
                {
                    CacheChange = true;
                }
                //فحص في التغييرات للمواد
                string thisItemName = reader.GetString("NameItem");
                int thisItemQuantity = reader.GetInt32("Quantity");
                int thisItemPrice = reader.GetInt32("Price");
                if (dataGridView1[0,indx].Value.ToString()!= thisItemName)
                {
                    ItemNames.Add(dataGridView1[0, indx].Value.ToString());
                    InvoisIds1.Add(reader.GetInt32("IDInvoice"));
                }
                
                if (Convert.ToInt32( dataGridView1[2, indx].Value) != thisItemQuantity)
                {
                    quantitys.Add(Convert.ToInt32( dataGridView1[2, indx].Value));
                    InvoisIds2.Add(reader.GetInt32("IDInvoice"));
                }
                    
                if (Convert.ToInt32( dataGridView1[1, indx].Value) != thisItemPrice)
                {
                    prices.Add(Convert.ToInt32(dataGridView1[1, indx].Value));
                    InvoisIds3.Add(reader.GetInt32("IDInvoice"));
                }
                    
                indx++;
            }
            con.Close();
            if (NameChange) { updateCustomerName(); }
            if (DateChange) { updateDate(); }
            if (CacheChange) { updateCache(); }
            for (int i = 1; i<=ItemNames.Count; i++)
            {
                updateNameOfIndex(ItemNames[i - 1], InvoisIds1[i - 1]);
            }
            for (int i = 1; i <= quantitys.Count; i++)
            {
                updateQuantityOfIndex(quantitys[i - 1], InvoisIds2[i - 1]);
            }
            for (int i = 1; i <= prices.Count; i++)
            {
                updatePriceOfIndex(prices[i - 1], InvoisIds3[i - 1]);
            }
        }
        private void updateCustomerName()
        {
            
            string cmdstr = "UPDATE `madatec`.`lists` SET `IDCustomer`=(select IDCustomer from customers where NameCustomer ='"+textBox1.Text+"') WHERE `IDList`='"+IDList+"';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("name is changed" + IDList);

        }
        private void updateCustomerBalance()
        {
            string cmdstr = "UPDATE `madatec`.`customers` SET `BalanceCustomer`='-"+total+ "' WHERE `IDCustomer`=(select IDCustomer from customers where NameCustomer ='" + textBox1.Text + "');";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void updateDate()
        {
            string cmdstr = "UPDATE `madatec`.`lists` SET `DateList`='"+myInfo.SqlDateFormat(dateTimePicker1.Value)+"' WHERE `IDList`='" + IDList + "';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("date is changed" + IDList);
        }
        private void updateCache()
        {
            string cmdstr = "UPDATE `madatec`.`lists` SET `Cache`='" +textBox7.Text+ "' WHERE `IDList`='" + IDList + "';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("cache is changed" + IDList);
        }
        private void updateNameOfIndex(string newName,int invoisID)
        {
            string cmdstr = "UPDATE `madatec`.`invoices` SET `IDItem`=(select IDItem from items where NameItem ='"+newName+"') WHERE `IDInvoice`='"+invoisID+"';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void updateQuantityOfIndex(int newQuantity, int invoisID)
        {
            string cmdstr = "UPDATE `madatec`.`invoices` SET `Quantity`='"+newQuantity+"' WHERE `IDInvoice`='" + invoisID + "';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void updatePriceOfIndex(int newPrice,int invoisID)
        {
            string cmdstr = "UPDATE `madatec`.`invoices` SET `Price`='" + newPrice + "' WHERE `IDInvoice`='" + invoisID + "';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void insertItemToDBInvoice()
        {
            //اضافة المادة في قاعدة البيانات
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            string cmdstr = "INSERT INTO `madatec`.`invoices` (`IDItem`, `Price`, `Quantity`, `IDList`) VALUES ((select IDItem from items where NameItem ='" + textBox4.Text + "'), '" + Convert.ToDouble( textBox5.Text )+ "', '" + textBox6.Text + "', '" + IDList + "');select LAST_INSERT_ID();";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);

            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1[4, dataGridView1.Rows.Count-2].Value = reader.GetInt32(0);// اضافة الاي دي الخاص بهذا الريكورد
            }
            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int selectedIvoID = Convert.ToInt32( dataGridView1[4, dataGridView1.CurrentRow.Index].Value);
            string cmdstr = "DELETE FROM `madatec`.`invoices` WHERE `IDInvoice`='"+selectedIvoID+"';";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        private void text4_leave(object sender, EventArgs e)
        {
            textBox5.Focus();
        }

        private void text5_leave(object sender, EventArgs e)
        {
            textBox6.Focus();
        }

        private void text6_leave(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_leave(object sender, EventArgs e)
        {
            textBox7.Focus();
        }

        private void textbox7_leave(object sender, EventArgs e)
        {
            button3.Focus();

        }

        private void button3_leave(object sender, EventArgs e)
        {
            textBox4.Focus();
        }
    }
}
