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
    public partial class Epmanalea : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public Epmanalea()
        {
            InitializeComponent();
            LoadEmployees();
            LoadEmployees1();
            LeavetypeID();
        }

        private void LoadEmployees()
        {
            string query = "SELECT * FROM LeaveApplications";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Bind the data to DataGridView
            }
        }

        private void LoadEmployees1()
        {
            string query = "SELECT EmployeeID,FullName,Position,Department,DateJoined FROM Employee";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable; // Bind the data to DataGridView
            }
        }

        private void LeavetypeID()
        {
            string query = "SELECT * FROM LeaveTypes";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView3.DataSource = dataTable; 
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicID.Text))
            {
                MessageBox.Show("Please enter the Application ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbPermition.SelectedItem == null)
            {
                MessageBox.Show("Please select a permission option.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int applicationID;
            if (!int.TryParse(txtApplicID.Text.Trim(), out applicationID))
            {
                MessageBox.Show("Application ID must be a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string permission = cbPermition.SelectedItem.ToString();
            if (!(permission == "Pending" || permission == "Approved" || permission == "Rejected"))
            {
                MessageBox.Show("Invalid permission value selected. Please choose from Pending, Approved, or Rejected.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE LeaveApplications SET Status = @Permition WHERE ApplicationID = @ApplicationID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                        cmd.Parameters.AddWithValue("@Permition", permission);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Leave Application Updated Successfully");
                        }
                        else
                        {
                            MessageBox.Show("No records updated. Please check the Application ID.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("SQL Error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                LoadEmployees();
            }
        }

            private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicID.Text))
            {
                MessageBox.Show("Please enter the Application ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int applicationID;
            if (!int.TryParse(txtApplicID.Text.Trim(), out applicationID))
            {
                MessageBox.Show("Application ID must be a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM LeaveApplications WHERE ApplicationID = @ApplicationID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
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
            // Ensure the user has selected or entered an ApplicationID
            if (string.IsNullOrEmpty(txtApplicID.Text))
            {
                MessageBox.Show("Please select or enter the ApplicationID to delete.");
                return;
            }

            int applicationId;
            if (!int.TryParse(txtApplicID.Text, out applicationId))
            {
                MessageBox.Show("Invalid ApplicationID. Please enter a valid number.");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this leave application?", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        conn.Open();

                        string deleteQuery = "DELETE FROM LeaveApplications WHERE ApplicationID = @ApplicationID";

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@ApplicationID", applicationId);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Leave application deleted successfully.");
                                LoadEmployees();
                            }
                            else
                            {
                                MessageBox.Show("No leave application found with the given ApplicationID.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the leave application: " + ex.Message);
                }
                LoadEmployees();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtApplicID.Clear();
            cbPermition.SelectedIndex = -1;
        }
    }
}
