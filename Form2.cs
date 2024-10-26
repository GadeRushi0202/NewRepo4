using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;
using System.Reflection;

namespace ConnectedDemo
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                List<Dept> list = new List<Dept>();
                string qry = "Select * from Dept";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dept dept = new Dept();
                        dept.did = Convert.ToInt32(reader["did"]);
                        dept.Dname = reader["dname"].ToString();
                        list.Add(dept);
                    }
                }
                comboBoxDepartment.DataSource = list;
                comboBoxDepartment.DisplayMember = "Dname";
                comboBoxDepartment.ValueMember = "did";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //ClearFields();
            }
            finally
            {
                con.Close();

            }


        }
        

        private void buttonSave_Click(object sender, EventArgs e)
        {
           

        }
    }
}
