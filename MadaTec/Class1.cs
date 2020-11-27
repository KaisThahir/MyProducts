using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MadaTec
{
    class Class1
    {
        public string ConStr = "Server=localhost;Database=madatec;Uid=root;Pwd=12345678";

        public string SqlDateFormat(DateTime indate)
        {
            string newformat;
            string yyyy, MM, dd;
            yyyy = Convert.ToString(indate.Year);
            MM = Convert.ToString(indate.Month);
            dd = Convert.ToString(indate.Day);
            newformat = yyyy + "-" + MM + "-" + dd;
            return newformat;
        }
        public static double cost = 0;

        public double FindElementPrice(int elementid,int year, int month)
        {
            
            var price = new List<double> ();
            string cmdstr = "select price from bayelement where IDBay in (select IDBay from baylists where ListDate between '2019-07-01' and '2019-07-30') and IDElement= (select IDElement from elements where IDElement ='"+elementid+"');";
            string cmdstr2 = "select price from bayelement where IDBay in (select IDBay from baylists where year(ListDate) = '" + year+"' and month(ListDate) = '"+month+ "') and IDElement= '" + elementid+"';";
            MySqlConnection conn = new MySqlConnection(ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr2, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                price.Add(reader.GetInt32("price"));
            }
            conn.Close();
            if (price.Count == 0 )
            {
                if (month == 1)
                {
                    if (year > 2017)
                    {
                        price.Add(FindElementPrice(elementid, year - 1, 12));
                    }
                    else { price.Add(0); }
                }
                else
                {
                    price.Add(FindElementPrice(elementid, year, month - 1));
                }
              
            }
            //if (price.Count == 0) { return 0; }
            return price.Average();
        }

        public Int16 getIdOfElement(string ElementName)
        {
            Int16 id=0;
            MySqlConnection con = new MySqlConnection(ConStr);
            string cmdstr = "SELECT IDElement FROM madatec.elements where NameElement = '" + ElementName + "';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt16(0);
            }
            con.Close();
            return id;
        }
        public Int16 getIdOfItem(string ItemName)
        {
            Int16 id = 0;
            MySqlConnection con = new MySqlConnection(ConStr);
            string cmdstr = "SELECT IDItem FROM madatec.items where NameItem = '" + ItemName + "';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt16(0);
            }
            con.Close();
            return id;
        }
        
        public double findCustomerBalance(Int32 IDCustomer)
        {
            int totalCache = 0;
            float balance = 0;
            List<int> IDLists = new List<int>();
            string cmdstr = "select IDList,Cache from lists where IDCustomer ="+IDCustomer+";";
            MySqlConnection con = new MySqlConnection(ConStr);
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                totalCache = totalCache + reader.GetInt32("Cache");
                IDLists.Add(reader.GetInt32("IDList"));
            }
            con.Close();
            for (int i=0; i< IDLists.Count; i++)
            {
                cmdstr = "select * from invoices where IDList= "+IDLists[i] +";";
                cmd = new MySqlCommand(cmdstr, con);
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    balance = balance - (reader.GetInt32("Price") * reader.GetInt32("Quantity"));
                }
                con.Close();
            }
            balance = balance + totalCache;
            return balance;
        }

        public Int16 getIdOfCustomr(string Name)
        {
            Int16 id = 0;
            MySqlConnection con = new MySqlConnection(ConStr);
            string cmdstr = "SELECT IDCustomer FROM madatec.customers where NameCustomer='"+Name+"';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt16(0);
            }

            con.Close();
            return id;
        }
        public Int32 getIdOfShope(string Name)
        {
            Int32 id = 0;
            MySqlConnection con = new MySqlConnection(ConStr);
            string cmdstr = "SELECT IDShope FROM madatec.shopes where NameShope ='"+Name+"';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            con.Close();
            return id;
        }
        public string getNameOfShope(int id)
        {
            string name ="";
            MySqlConnection con = new MySqlConnection(ConStr);
            string cmdstr = "SELECT NameShope FROM madatec.shopes where IDShope ='" + id + "';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                name = reader.GetString(0);
            }
            con.Close();
            return name;
        }
        public double itemCost(Int32 itemID,Int32 year,Int32 month)
        {
            double cost = 0;
            double price;

            //Class1 myinfo = new Class1();
            MySqlConnection con = new MySqlConnection(ConStr);
            string cmdstr = "SELECT * FROM madatec.itemelement where IDItem = '"+itemID+"';";
            MySqlCommand cmd = new MySqlCommand(cmdstr, con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                Int32 thisElementID = reader.GetInt32("IDElement");
                price = FindElementPrice(thisElementID,System.DateTime.Today.Year,System.DateTime.Today.Month);
                cost = cost + (price * reader.GetDouble("Quantity"));
            
            
            
            }
            con.Close();

            return cost;
        }
        public double itemCost(string itemName, Int32 year, Int32 month)
        {
            double cost = 0;
            cost = itemCost(getIdOfItem(itemName),year,month);
            return cost;
        
        }
        public double totalBayOfType(DateTime fromDate,DateTime toDate,string type) {
            double total = 0;
            string sqlStr = "SELECT baylists.ListNo,baylists.ListDate,shopes.NameShope,bayelement.price,bayelement.Quantity,elements.NameElement,elements.Type from baylists inner join bayelement on baylists.IDBay=bayelement.IDBay inner join elements on bayelement.IDElement= elements.IDElement inner join shopes on baylists.IDShope = shopes.IDShope where baylists.ListDate between '"+SqlDateFormat( fromDate)+"' and '"+SqlDateFormat( toDate)+"' and elements.Type ='"+type+"';";
            using (MySqlConnection conn = new MySqlConnection(ConStr)) {
                conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sqlStr,conn)){
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) { 
                    total= total + (reader.GetDouble("price")*reader.GetDouble("Quantity"));
                }
                conn.Close();
            }
            }
            return total;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        
        }


    }
}
