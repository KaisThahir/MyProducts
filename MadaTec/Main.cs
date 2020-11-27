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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Elements toOpen = new Elements() ;
            toOpen.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Items toOpen = new Items ();
            toOpen.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Customers toOpen = new Customers();
            toOpen.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            CelList toOpen = new CelList();
            toOpen.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Class1 myinfo = new Class1();
            string file = "D:\\MadaTec\\db\\dbbackup"+ myinfo.SqlDateFormat( System.DateTime.Now)+".sql";
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            MySqlBackup bm = new MySqlBackup(cmd);
            con.Open();
            bm.ExportToFile(file);
            con.Close();
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Class1 myinfo = new Class1();
            string file = "D:\\MadaTec\\db\\dbbackup" + myinfo.SqlDateFormat(System.DateTime.Now) + ".sql";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            file = ofd.FileName;
            MySqlConnection con = new MySqlConnection(myinfo.ConStr);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            MySqlBackup bm = new MySqlBackup(cmd);
            con.Open();
            bm.ImportFromFile(file);
            con.Close();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            calculatoins newform = new calculatoins();
            newform.Show();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Reporting reports = new Reporting();
            reports.Show();
        }
    }
}
