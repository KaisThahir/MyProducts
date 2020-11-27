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
    public partial class searchedBayList : Form
    {
        public searchedBayList()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        public static int listNo;
        Class1 myInfo = new Class1();
        private void searchedBayList_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(listNo.ToString());

            formatDataGridView();
            searchBayList();

        }

        private void searchBayList()
        {
            string cmdstr = "SELECT * FROM madatec.baylists,madatec.shopes where baylists.ListNo ='"+listNo+"' and baylists.IDShope= shopes.IDShope;";
            MySqlConnection con = new MySqlConnection(myInfo.ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fillDataGrid(listNo, reader.GetString("NameShope"), reader.GetDateTime("ListDate"), reader.GetDouble("Cashe"), reader.GetInt32("IDBay"));
            }
            con.Close();
        }
        private void formatDataGridView()
        {
            dataGridView1.Columns.Add("", "الرقم");
            dataGridView1.Columns.Add("", "المحل");
            dataGridView1.Columns.Add("", "التاريخ");
            dataGridView1.Columns.Add("", "المبلغ");
            dataGridView1.Columns.Add("", "الرمز");

            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
            //dataGridView1.Rows.Add("");
        }
        private void fillDataGrid(int No , string shopeName, DateTime listDate,double cashe,int BayListID)
        {
            dataGridView1.Rows.Add();
            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
            dataGridView1[0, dataGridView1.Rows.Count - 1].Value = No;
            dataGridView1[1, dataGridView1.Rows.Count - 1].Value = shopeName;
            dataGridView1[2, dataGridView1.Rows.Count - 1].Value = listDate;
            dataGridView1[3, dataGridView1.Rows.Count - 1].Value = cashe;
            dataGridView1[4, dataGridView1.Rows.Count - 1].Value = BayListID;
            

        }

        private void DGDoubleClick(object sender, EventArgs e)
        {
            Elements f = new Elements();

            int sel = dataGridView1.CurrentRow.Index;
            int idlist = Convert.ToInt32(dataGridView1[4, sel].Value);
            Elements.BayListID = idlist;
            Elements.idShope = myInfo.getIdOfShope(dataGridView1[1, sel].Value.ToString());
            Elements.listDate = Convert.ToDateTime (dataGridView1[2, sel].Value);
            Elements.listCashe = Convert.ToDouble(dataGridView1[3, sel].Value);
            //Elements.searchlist();

            this.Close();
        }
    }
}
