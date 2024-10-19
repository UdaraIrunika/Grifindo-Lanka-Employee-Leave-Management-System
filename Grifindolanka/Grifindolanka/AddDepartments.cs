using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grifindolanka
{
    public partial class AddDepartments : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public AddDepartments()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            string query = "SELECT * FROM Departments";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Bind the data to DataGridView
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDid.Clear();
            txtDname.Clear();
            txtDdetails.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDid.Text) ||
               string.IsNullOrWhiteSpace(txtDname.Text) ||
               string.IsNullOrWhiteSpace(txtDdetails.Text))
            {
                MessageBox.Show("Please fill all required fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    string insertQuery = @"INSERT INTO Departments (ID,Dname, Description) 
                                   VALUES (@ID, @Dname, @Details)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connect))
                    {
                        // Adding parameters to the SQL command
                        cmd.Parameters.AddWithValue("@ID", txtDid.Text.Trim());
                        cmd.Parameters.AddWithValue("@Dname", txtDname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Details", txtDdetails.Text.Trim());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDepartments();
                        }
                        else
                        {
                            MessageBox.Show("Error adding employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(txtDid.Text) ||
               string.IsNullOrWhiteSpace(txtDname.Text) ||
               string.IsNullOrWhiteSpace(txtDdetails.Text))
            {
                MessageBox.Show("Please fill all required fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Establish connection
                using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    // Update query for Employee
                    string updateQuery = @"UPDATE Departments SET Dname = @Dname, Description = @Details WHERE ID = @DeparmentsID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                    {
                        cmd.Parameters.AddWithValue("@DeparmentsID", int.Parse(txtDid.Text.Trim()));
                        cmd.Parameters.AddWithValue("@Dname", txtDname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Details", txtDdetails.Text.Trim());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No employee found with the provided Employee ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    LoadDepartments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDid.Text))
            {
                MessageBox.Show("Please enter the Deparments ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int applicationID;
            if (!int.TryParse(txtDid.Text.Trim(), out applicationID))
            {
                MessageBox.Show("Deparments ID must be a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Departments WHERE ID = @DeparmentsID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DeparmentsID", int.Parse(txtDid.Text.Trim()));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dataGridView1.DataSource = null;
                            dataGridView1.Rows.Clear();
                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("No records found for the provided Application ID.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDid.Text))
            {
                MessageBox.Show("Please enter the Employee ID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                string deleteApplicationsQuery = "DELETE FROM Departments WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(deleteApplicationsQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@ID", txtDid.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                string deleteEmployeeQuery = "DELETE FROM Departments WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(deleteEmployeeQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", txtDid.Text.Trim());
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Employee not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                    LoadDepartments();
                }
            }
        }
    }
}
