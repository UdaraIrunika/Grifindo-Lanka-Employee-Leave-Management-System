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
    public partial class ManageLeatyp : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public ManageLeatyp()
        {
            InitializeComponent();
            LodeData();
        }

        private void LodeData()
        {
            string query = "SELECT * FROM LeaveTypes";
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Bind the data to DataGridView
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate user inputs
            if (!ValidateInputs())
            {
                return; // If validation fails, exit the method
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();

                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to add this new leave type?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string insertQuery = @"INSERT INTO LeaveTypes 
                                               (LeaveTypeName, MaxAnnualLeave, MaxCasualLeave, MaxShortLeavePerMonth) 
                                               VALUES 
                                               (@LeaveTypeName, @MaxAnnualLeave, @MaxCasualLeave, @MaxShortLeavePerMonth)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.Add("@LeaveTypeName", SqlDbType.VarChar, 50).Value = txtLeaveType.Text.Trim();
                            cmd.Parameters.Add("@MaxAnnualLeave", SqlDbType.Int).Value = int.Parse(txtMaxanual.Text.Trim());
                            cmd.Parameters.Add("@MaxCasualLeave", SqlDbType.Int).Value = int.Parse(txtMaxcassual.Text.Trim());
                            cmd.Parameters.Add("@MaxShortLeavePerMonth", SqlDbType.Int).Value = int.Parse(txtMaxshort.Text.Trim());

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Leave Type added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadLeaveTypes(); 
                                ClearInputs();   
                            }
                            else
                            {
                                MessageBox.Show("Failed to add the Leave Type. Please try again.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Operation canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException formatEx)
            {
                MessageBox.Show($"Input Format Error: {formatEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtLeaveType.Text))
            {
                MessageBox.Show("Please enter the Leave Type Name.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLeaveType.Focus();
                return false;
            }

            if (!int.TryParse(txtMaxanual.Text.Trim(), out int maxAnnual) || maxAnnual < 0)
            {
                MessageBox.Show("Please enter a valid positive integer for Max Annual Leave.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaxanual.Focus();
                return false;
            }

            if (!int.TryParse(txtMaxcassual.Text.Trim(), out int maxCasual) || maxCasual < 0)
            {
                MessageBox.Show("Please enter a valid positive integer for Max Casual Leave.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaxcassual.Focus();
                return false;
            }

            if (!int.TryParse(txtMaxshort.Text.Trim(), out int maxShort) || maxShort < 0)
            {
                MessageBox.Show("Please enter a valid positive integer for Max Short Leave Per Month.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaxshort.Focus();
                return false;
            }

            return true; 
        }
        private void LoadLeaveTypes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
                {
                    string selectQuery = "SELECT LeaveTypeID, LeaveTypeName, MaxAnnualLeave, MaxCasualLeave, MaxShortLeavePerMonth FROM LeaveTypes";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridView1.DataSource = dt;

                            dataGridView1.Columns["LeaveTypeID"].HeaderText = "ID";
                            dataGridView1.Columns["LeaveTypeName"].HeaderText = "Leave Type";
                            dataGridView1.Columns["MaxAnnualLeave"].HeaderText = "Annual Leave";
                            dataGridView1.Columns["MaxCasualLeave"].HeaderText = "Casual Leave";
                            dataGridView1.Columns["MaxShortLeavePerMonth"].HeaderText = "Short Leave/Month";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load Leave Types: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearInputs()
        {
            txtLeaveType.Clear();
            txtMaxanual.Clear();
            txtMaxcassual.Clear();
            txtMaxshort.Clear();
            txtID.Clear();
        }







        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conn.Open();

                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to update this leave type?", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
         
                        string updateQuery = "UPDATE LeaveTypes " +
                                             "SET LeaveTypeName = @LeaveTypeName, " +
                                             "    MaxAnnualLeave = @LeaveMaxAnnual, " +
                                             "    MaxCasualLeave = @LeaveMaxCasual, " +
                                             "    MaxShortLeavePerMonth = @LeaveMaxShort " +
                                             "WHERE LeaveTypeID = @LeaveTypeID"; 

                    
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@LeaveTypeID", Convert.ToInt32(txtID.Text));
                            cmd.Parameters.AddWithValue("@LeaveTypeName", txtLeaveType.Text);
                            cmd.Parameters.AddWithValue("@LeaveMaxAnnual", txtMaxanual.Text); 
                            cmd.Parameters.AddWithValue("@LeaveMaxCasual", txtMaxcassual.Text);
                            cmd.Parameters.AddWithValue("@LeaveMaxShort", txtMaxshort.Text);   

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Leave Type Updated Successfully");
                            }
                            else
                            {
                                MessageBox.Show("No record was updated. Please check if the LeaveTypeID exists.");
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Update canceled.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM LeaveTypes WHERE LeaveTypeID = @LeaveType";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LeaveType", txtID.Text); // Assuming txtLeaveTypeID holds the LeaveTypeID

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtID.Text = reader["LeaveTypeID"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Leave Type Not Found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM LeaveTypes WHERE LeaveTypeID = @LeaveTypeID";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Leave Type Deleted Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ManageLeatyp_Load(object sender, EventArgs e)
        {

        }
    }
}
