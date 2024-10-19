using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grifindolanka
{
    
    public partial class Applyleave : UserControl
    {
        // Declare a static variable for the logged-in employee ID
        public static int LoggedInEmployeeId { get; set; }

        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");

        public Applyleave()
        {
            InitializeComponent();
            LoadLeaveTypes();
            LoadLeaveTypes();
            LoadEmpid(); // Add this line to load Employee IDs
        }

        private void LoadLeaveTypes()
        {
            string connectionString = @"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True;";
            string query = "SELECT LeaveTypeName FROM LeaveTypes";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cbLtype.Items.Clear();
                            while (reader.Read())
                            {
                                cbLtype.Items.Add(reader["LeaveTypeName"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadEmpid()
        {
   
            string connectionString = @"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True;";

            string query = "SELECT FullName FROM Employee WHERE Role = 'Employee'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cbCoose.Items.Clear();
                            while (reader.Read())
                            {
                                cbCoose.Items.Add(reader["FullName"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbLtype.SelectedIndex = -1;
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now;
        }
        public static void SetLoggedInEmployeeId(int employeeId)
        {
            LoggedInEmployeeId = employeeId;
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            // Validate input
            if (cbLtype.SelectedItem == null)
            {
                MessageBox.Show("Please select a leave type.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtpStart.Value.Date > dtpEnd.Value.Date)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Retrieve FullName from ComboBox 'cbCoose'
            string fullName = cbCoose.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Please select a valid Full Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True;"))
            {
                try
                {
                    connect.Open();

                    // Check if the Employee exists based on FullName
                    string checkEmployeeIdQuery = "SELECT COUNT(*) FROM Employee WHERE FullName = @FullName";
                    using (SqlCommand checkCmd = new SqlCommand(checkEmployeeIdQuery, connect))
                    {
                        checkCmd.Parameters.AddWithValue("@FullName", fullName);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Employee not found. Please check the Full Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Retrieve EmployeeID based on FullName
                    string getEmployeeIdQuery = "SELECT EmployeeID FROM Employee WHERE FullName = @FullName";
                    int employeeId;
                    using (SqlCommand getCmd = new SqlCommand(getEmployeeIdQuery, connect))
                    {
                        getCmd.Parameters.AddWithValue("@FullName", fullName);
                        var employeeIdObj = getCmd.ExecuteScalar();
                        if (employeeIdObj != null)
                        {
                            employeeId = Convert.ToInt32(employeeIdObj);
                        }
                        else
                        {
                            MessageBox.Show("Employee ID could not be retrieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string insertLeave = @"
                INSERT INTO LeaveApplications (EmployeeID, LeaveTypeID, LeaveStartDate, LeaveEndDate, ApplicationDate, Status, Remarks)
                VALUES (@employeeId, @leaveTypeId, @leaveStartDate, @leaveEndDate, GETDATE(), 'Pending', @remarks)";

                    using (SqlCommand cmd = new SqlCommand(insertLeave, connect))
                    {
                        cmd.Parameters.AddWithValue("@employeeId", employeeId);
                        cmd.Parameters.AddWithValue("@leaveStartDate", dtpStart.Value.Date);
                        cmd.Parameters.AddWithValue("@leaveEndDate", dtpEnd.Value.Date);

                        // Get LeaveTypeID based on selected LeaveTypeName
                        string leaveTypeName = cbLtype.SelectedItem.ToString();
                        string getLeaveTypeIdQuery = "SELECT LeaveTypeID FROM LeaveTypes WHERE LeaveTypeName = @leaveTypeName";
                        using (SqlCommand typeIdCommand = new SqlCommand(getLeaveTypeIdQuery, connect))
                        {
                            typeIdCommand.Parameters.AddWithValue("@leaveTypeName", leaveTypeName);
                            var leaveTypeIdObj = typeIdCommand.ExecuteScalar();

                            if (leaveTypeIdObj == null)
                            {
                                MessageBox.Show("Leave type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            int leaveTypeId = Convert.ToInt32(leaveTypeIdObj);
                            cmd.Parameters.AddWithValue("@leaveTypeId", leaveTypeId);
                        }
                        string remarks = ""; 
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Leave application submitted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }

            ClearFields(); 
        }
        private void ClearFields()
        {
            cbLtype.SelectedIndex = -1; 
            dtpStart.Value = DateTime.Now; 
            dtpEnd.Value = DateTime.Now; 
        }

        private void cbCoose_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
