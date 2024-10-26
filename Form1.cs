using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;

namespace ConnectedDemo
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Dept> list = new List < Dept > ();
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
                ClearFields();
            }
            finally
            {
                con.Close();
              
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into Employee values(@Name,@Email,@Age,@salary,@did)";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Name", textBoxName.Text);
                cmd.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                cmd.Parameters.AddWithValue("@Age",Convert.ToInt32( textBoxAge.Text));
                cmd.Parameters.AddWithValue("@Salary", Convert.ToDouble(textBoxSalary.Text));
                cmd.Parameters.AddWithValue("@did", Convert.ToInt32(comboBoxDepartment.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result >= 1)
                {
                    MessageBox.Show("Record Inserted");
                    GetAllEmps();
                  
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close() ;
                ClearFields();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select e.*, d.dname from Employee e inner join dept d on d.did = e.did where e.id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxID.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        textBoxName.Text = reader["Name"].ToString();
                        textBoxEmail.Text = reader["Email"].ToString();
                        textBoxAge.Text = reader["Age"].ToString();
                        textBoxSalary.Text = reader["salary"].ToString();
                        comboBoxDepartment.Text = reader["dname"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                ClearFields();
            }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update Employee set Name=@Name,Email=@Email,Age=@Age,salary=@salary,did=@did where id=@id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToInt32(textBoxAge.Text));
                cmd.Parameters.AddWithValue("@salary", Convert.ToDouble(textBoxSalary.Text));
                cmd.Parameters.AddWithValue("@did", Convert.ToInt32(comboBoxDepartment.SelectedValue));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxID.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record updated");
                    ClearFields();


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                con.Close();
                ClearFields();
            }
            GetAllEmps();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from Employee where id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxID.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            finally
            {
                con.Close();
                ClearFields();
            }

        }
        private void GetAllEmps()
        {
            string qur = "select e.*,d.dname from Employee e inner join Dept d on d.did=e.did";
            cmd=new SqlCommand(qur, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource= table;
            con.Close();
        }
        private void ClearFields()
        {
            textBoxID.Clear();
            textBoxName.Clear();
            textBoxAge.Clear();
            textBoxEmail.Clear();
            textBoxSalary.Clear();
            comboBoxDepartment.Refresh();
        }
        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllEmps();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
