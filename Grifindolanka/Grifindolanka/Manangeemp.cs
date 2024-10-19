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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Grifindolanka
{
    public partial class Manangeemp : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public Manangeemp()
        {
            InitializeComponent();
            LoadEmployees();
            LoadAdmin();
        }

        private void LoadEmployees()
        {
            string query = "SELECT * FROM Employee WHERE Role = 'Employee'";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Bind the data to DataGridView
            }
        }

        private void LoadAdmin()
        {
            string query = "SELECT * FROM Employee WHERE Role = 'Admin'";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable; // Bind the data to DataGridView
            }
        }

        private int GetSelectedEmployeeID()
        {
            if (dataGridView1.CurrentRow != null)
            {
                return Convert.ToInt32(dataGridView1.CurrentRow.Cells["EmployeeID"].Value);
            }
            return 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFname.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtPosition.Text) ||
                string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                cbRole.SelectedItem == null)
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
                    string insertQuery = @"INSERT INTO Employee (FullName, Username, PasswordHash, Position, Department, Role, DateJoined, RoasterStartTime, RoasterEndTime) 
                                   VALUES (@FullName, @Username, @PasswordHash, @Position, @Department, @Role, @DateJoined, @RoasterStartTime, @RoasterEndTime)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connect))
                    {
                        // Adding parameters to the SQL command
                        cmd.Parameters.AddWithValue("@FullName", txtFname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@PasswordHash", txtPass.Text.Trim()); 
                        cmd.Parameters.AddWithValue("@Position", txtPosition.Text.Trim());
                        cmd.Parameters.AddWithValue("@Department", txtDepartment.Text.Trim());
                        cmd.Parameters.AddWithValue("@Role", cbRole.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DateJoined", dtpJoindate.Value);
                        cmd.Parameters.AddWithValue("@RoasterStartTime", dtpRosters.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@RoasterEndTime", dtpRoserE.Value.TimeOfDay);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployees();
                            LoadAdmin(); 
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
            if (string.IsNullOrWhiteSpace(txtFname.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtPosition.Text) ||
                string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                cbRole.SelectedItem == null)
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
                    string updateQuery = @"UPDATE Employee 
                                   SET FullName = @FullName, 
                                       Username = @Username, 
                                       PasswordHash = @PasswordHash, 
                                       Position = @Position, 
                                       Department = @Department, 
                                       Role = @Role, 
                                       DateJoined = @DateJoined 
                                   WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(txtid.Text.Trim()));
                        cmd.Parameters.AddWithValue("@FullName", txtFname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@PasswordHash", txtPass.Text.Trim()); 
                        cmd.Parameters.AddWithValue("@Position", txtPosition.Text.Trim());
                        cmd.Parameters.AddWithValue("@Department", txtDepartment.Text.Trim());
                        cmd.Parameters.AddWithValue("@Role", cbRole.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DateJoined", dtpJoindate.Value); 
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
                    LoadEmployees(); 
                    LoadAdmin(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = "SELECT * FROM Employee WHERE Username = @Username";

            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = new SqlCommand(searchQuery, connect))
                {
                    try
                    {
                        connect.Open();
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim()); // Trim to remove any leading/trailing spaces

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    txtFname.Text = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty;
                                    txtUsername.Text = reader["Username"] != DBNull.Value ? reader["Username"].ToString() : string.Empty;
                                    txtPass.Text = reader["PasswordHash"] != DBNull.Value ? reader["PasswordHash"].ToString() : string.Empty;
                                    txtPosition.Text = reader["Position"] != DBNull.Value ? reader["Position"].ToString() : string.Empty;
                                    txtDepartment.Text = reader["Department"] != DBNull.Value ? reader["Department"].ToString() : string.Empty;
                                    string role = reader["Role"] != DBNull.Value ? reader["Role"].ToString() : string.Empty;
                                    if (cbRole.Items.Contains(role))
                                    {
                                        cbRole.SelectedItem = role;
                                    }
                                    else
                                    {
                                        cbRole.SelectedIndex = -1; 
                                    }

                                    // DateJoined
                                    if (reader["DateJoined"] != DBNull.Value)
                                    {
                                        DateTime dateJoined;
                                        if (DateTime.TryParse(reader["DateJoined"].ToString(), out dateJoined))
                                        {
                                            dtpJoindate.Value = dateJoined;
                                        }
                                    }
                                    if (reader["RoasterStartTime"] != DBNull.Value)
                                    {
                                        TimeSpan roasterStartTime;
                                        if (TimeSpan.TryParse(reader["RoasterStartTime"].ToString(), out roasterStartTime))
                                        {
                                            dtpRosters.Value = DateTime.Today.Add(roasterStartTime);
                                        }
                                    }
                                    if (reader["RoasterEndTime"] != DBNull.Value)
                                    {
                                        TimeSpan roasterEndTime;
                                        if (TimeSpan.TryParse(reader["RoasterEndTime"].ToString(), out roasterEndTime))
                                        {
                                            dtpRoserE.Value = DateTime.Today.Add(roasterEndTime);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No employee found with the provided username.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (InvalidCastException ex)
                    {
                        MessageBox.Show($"Data type mismatch: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadEmployees();
                    LoadAdmin();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtid.Text))
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
                string deleteApplicationsQuery = "DELETE FROM LeaveApplications WHERE EmployeeID = @EmployeeID";
                using (SqlCommand cmd = new SqlCommand(deleteApplicationsQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", txtid.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                string deleteEmployeeQuery = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                using (SqlCommand cmd = new SqlCommand(deleteEmployeeQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", txtid.Text.Trim());
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
                    LoadEmployees();
                    LoadAdmin();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFname.Clear();
            txtUsername.Clear();
            txtPass.Clear();
            txtPosition.Clear();
            txtDepartment.Clear();
            cbRole.SelectedIndex = -1; // Reset ComboBox
            dtpJoindate.Value = DateTime.Now;
            dtpRosters.Value = DateTime.Now;
            dtpRoserE.Value = DateTime.Now;
        }
    }
}
