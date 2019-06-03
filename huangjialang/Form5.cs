﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace huangjialang
{
    public partial class Form5 : Form
    {
        string server = "localhost";
        string database = "hjl";
        string uid = "root";
        string password = "123456";

        List<string> MemberList;//员工名单

        //DataSet ds;
        string connectionString;//= "datasource=" + server + ";"+"PORT=3306;"+ "DATABASE=" + database + ";" + "username=" + uid + ";" + "PASSWORD=" + password + ";";
        MySqlConnection conn;

        //record the origional content
        string oldID;
      

        public Form5()
        {
            InitializeComponent();
            connectionString = "datasource=" + server + ";" + "PORT=3306;" + "DATABASE=" + database + ";" + "username=" + uid + ";" + "PASSWORD=" + password + ";";
            conn = new MySqlConnection(connectionString);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string startdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string enddate = dateTimePicker2.Value.ToString("yyyy-MM-dd");



            string search = textBox1.Text;
            string pass;
            if (!checkBox1.Checked)
            {
                pass = "SELECT * FROM hjl.`维修记录` where 车牌 like '%" + search + "%'" + "OR 型号 like '%" + search + "%'" + "OR 单号 like '%" + search + "%'";
            }
            else
            {

                pass = "SELECT * FROM hjl.`维修记录` where " + "(`日期` >= '" + startdate + "' and " + "`日期` <= '" + enddate + "') " + "and (车牌 like '%" + search + "%'" + "OR 型号 like '%" + search + "%'" + "OR 单号 like '%" + search + "%')";

            }



            if (search == "")
            { //MessageBox.Show("请输入关键字"); 
            }
            else
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(pass, conn);


                DataSet ds = new DataSet();
                adapter.Fill(ds, search);

                dataGridView1.DataSource = ds.Tables[search];
            }

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            

            loadOrderTable();
            oldID = "";
         
        }

        public void loadOrderTable()
        {

            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM hjl." + "维修记录", conn);
            adapter.Fill(ds, "维修记录");
            dataGridView1.DataSource = ds.Tables[0];

        }


        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int result = dateTimePicker2.Value.CompareTo(dateTimePicker1.Value);

            if (result < 0)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(30);

            }
            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //string name = MemberListcomboBox1.SelectedItem.ToString();
            //string phone = textBox4.Text.Replace(" ", "");
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];


                label11.Text = "NO." + row.Cells["ID"].Value.ToString();
                label17.Text = Convert.ToDateTime(row.Cells["日期"].Value.ToString()).ToString("MM/dd/yyyy");
                label8.Text = row.Cells["单号"].Value.ToString();
                label9.Text = row.Cells["车牌"].Value.ToString();
                label10.Text = row.Cells["型号"].Value.ToString();
                label15.Text = row.Cells["项目"].Value.ToString();
                label16.Text = row.Cells["维修人员"].Value.ToString();
               

                oldID = row.Cells["ID"].Value.ToString();

            }


        }
       
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            conn.Open();
            string startdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string enddate = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string pass;
            if (!checkBox1.Checked)
            {
                pass = "SELECT * FROM hjl.`维修记录` ";
            }
            else
            {

                pass = "SELECT * FROM hjl.`维修记录` where " + "(`日期` >= '" + startdate + "' and " + "`日期` <= '" + enddate + "') ";

            }


            
                MySqlDataAdapter adapter = new MySqlDataAdapter(pass, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "维修记录");

                dataGridView1.DataSource = ds.Tables["维修记录"];
            conn.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //维修记录` WHERE(`ID` = '65');
            //oldId  = id of selected id 

            string members = label16.Text;
            string ordernumber = label8.Text;
            string item = label15.Text;


            if (label8.Text == "201906XXX")
            {
                MessageBox.Show("请选择维修记录");
            }
            else
            {
                //memberlist =.ToList();
                char[] delimiter1 = new char[] { ' ' };
                List<string> memberlist = members.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string membername in memberlist)
                {

                    //get id from each member
                    conn.Open();
                    string sendcomm5 = "SELECT `ID` as ID FROM hjl.`" + membername.Replace(" ", "") + "` where `单号` = '" + ordernumber + "' and `项目` = '" + item + "'";
                    MySqlCommand command5 = new MySqlCommand(sendcomm5, conn);

                    MySqlDataReader reader5 = command5.ExecuteReader();
                    List<string> recordidlist = new List<string>();
                    while (reader5.Read())
                    {
                        if (!reader5.IsDBNull(0))
                        { recordidlist.Add(reader5.GetString("ID")); }

                    }
                    conn.Close();


                    foreach (string recordid in recordidlist)
                    {//DELETE FROM `hjl`.`维修记录` WHERE (`ID` = '65');
                        MessageBox.Show(membername + "6666" + recordid);
                        conn.Open();
                        string query6 = "DELETE FROM `hjl`.`" + membername + "` WHERE (`ID` = '" + recordid + "' )";
                        MySqlCommand command6 = new MySqlCommand(query6, conn);
                        command6.ExecuteNonQuery();
                        conn.Close();
                    }


                }

                conn.Open();
                string query = "DELETE FROM `hjl`.`维修记录` WHERE (`ID` = '" + oldID + "' )";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                conn.Close();

            }

            

            //reset the label
            label11.Text = "No.";
            label17.Text = "XXXX-XX-XX";
            label8.Text = "201906XXX";
            label9.Text = "粤XXXXXX";
            label10.Text = "XXXXXX";
            label15.Text = "XXXXX";
            label16.Text = "XXX XXX XXX";

            loadRecordTable();




        }


        public void loadRecordTable()
        {
            conn.Open();
            string startdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string enddate = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string pass;
            if (!checkBox1.Checked)
            {
                pass = "SELECT * FROM hjl.`维修记录` ";
            }
            else
            {

                pass = "SELECT * FROM hjl.`维修记录` where " + "(`日期` >= '" + startdate + "' and " + "`日期` <= '" + enddate + "') ";

            }



            MySqlDataAdapter adapter = new MySqlDataAdapter(pass, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "维修记录");

            dataGridView1.DataSource = ds.Tables["维修记录"];
        }
        /*
       private void button1_Click(object sender, EventArgs e)
       {
           //GetMemberList();
           string carname = CarNameTextbox.Text.Replace(" ", "");
           string carmodel = Carmodeltextbox.Text.Replace(" ", "");
           string recodedate = dateTimePicker3.Value.ToString("yyyy-MM-dd");
           string ordernumber = OrderNumbertextBox.Text.Replace(" ", "");
           string handler = HandlerTextbox.Text.Replace(" ", "");
           string ownername = OwnerNameTextbox.Text.Replace(" ", "");
           string ownerphone = OwnerPhoneTextbox.Text.Replace(" ", "");

           //UPDATE `hjl`.`单号信息` SET `日期` = '2019-08-13', `单号` = '201908102', `车牌` = '123', `型号` = 'dfsdf', `接单人` = 'sdf', `车主姓名` = 'sdf sdfsdf', `车主电话` = 'sdfsdf' WHERE(`ID` = '39');



           //update the record on 单号信息
           conn.Open();
           string query = "UPDATE `hjl`.`单号信息` SET `日期` = '" + recodedate + "', `单号` = '" + ordernumber + "', `车牌` = '" + carname + "', `型号` = '" + carmodel + "', `接单人` = '" + handler + "', `车主姓名` = '" + ownername + "', `车主电话` = '" + ownerphone + "' WHERE(`ID` = '" + oldID + "')";
           MySqlCommand command = new MySqlCommand(query, conn);
           command.ExecuteNonQuery();
           conn.Close();



           //get id from 维修记录
           conn.Open();
           string sendcomm = "SELECT `ID` as ID FROM hjl.维修记录 where `单号` = '" + oldordernumber + "'";
           MySqlCommand command2 = new MySqlCommand(sendcomm, conn);

           MySqlDataReader reader = command2.ExecuteReader();
           List<string> idlist = new List<string>();
           while (reader.Read())
           {
               if (!reader.IsDBNull(0))
               { idlist.Add(reader.GetString("ID")); }

           }
           conn.Close();
           //update record to 维修记录
           foreach (string id in idlist)
           {
               conn.Open();
               string query3 = "UPDATE `hjl`.`维修记录` SET `单号` = '" + ordernumber + "', `车牌` = '" + carname + "', `型号` = '" + carmodel + "' WHERE(`ID` = '" + id + "')";
               MySqlCommand command3 = new MySqlCommand(query3, conn);
               command3.ExecuteNonQuery();
               conn.Close();

               //update record to each member

               string members = "";

               //get member list 
               conn.Open();
               string sendcomm4 = "SELECT `维修人员` FROM hjl.维修记录 where `ID` = '" + id + "'";
               MySqlCommand command4 = new MySqlCommand(sendcomm4, conn);

               MySqlDataReader reader4 = command4.ExecuteReader();

               while (reader4.Read())
               {
                   if (!reader4.IsDBNull(0))
                   { members = reader4.GetString("维修人员"); }

               }
               conn.Close();

               //memberlist =.ToList();
               char[] delimiter1 = new char[] { ' ' };
               List<string> memberlist = members.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries).ToList();
               foreach (string membername in memberlist)
               {

                   //get id from each member
                   conn.Open();
                   string sendcomm5 = "SELECT `ID` as ID FROM hjl.`" + membername.Replace(" ", "") + "` where `单号` = '" + oldordernumber + "'";
                   MySqlCommand command5 = new MySqlCommand(sendcomm5, conn);

                   MySqlDataReader reader5 = command5.ExecuteReader();
                   List<string> recordidlist = new List<string>();
                   while (reader5.Read())
                   {
                       if (!reader5.IsDBNull(0))
                       { recordidlist.Add(reader5.GetString("ID")); }

                   }
                   conn.Close();


                   foreach (string recordid in recordidlist)
                   {
                       conn.Open();
                       string query6 = "UPDATE `hjl`.`" + membername + "` SET `单号` = '" + ordernumber + "', `车牌` = '" + carname + "', `型号` = '" + carmodel + "' WHERE(`ID` = '" + recordid + "')";
                       MySqlCommand command6 = new MySqlCommand(query6, conn);
                       command6.ExecuteNonQuery();
                       conn.Close();
                   }


               }


           }


           MessageBox.Show("修改成功");

       }
   */
    }
}
