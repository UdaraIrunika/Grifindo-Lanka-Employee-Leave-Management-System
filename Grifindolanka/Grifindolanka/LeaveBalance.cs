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
    public partial class LeaveBalance : UserControl
    {
        private string connectionString = "Data Source=DESKTOP-P8R14E4\\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True";
        private int currentEmployeeID;

        public LeaveBalance()
        {
            InitializeComponent();
            currentEmployeeID = 0;
            LoadLeaveProgress();
        }
        public LeaveBalance(int employeeID)
        {
            InitializeComponent();
            currentEmployeeID = employeeID;
            LoadLeaveProgress();
        }

        private void LoadLeaveProgress()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            lt.LeaveTypeName,
                            ISNULL(SUM(lb.LeaveBalance), 0) AS LeavesTaken,
                            CASE 
                                WHEN lt.LeaveTypeName = 'Annual' THEN lt.MaxAnnualLeave
                                WHEN lt.LeaveTypeName = 'Casual' THEN lt.MaxCasualLeave
                                WHEN lt.LeaveTypeName = 'Short' THEN lt.MaxShortLeavePerMonth * 12 -- Adjust as per business rules
                                ELSE 0
                            END AS MaxLeaves
                        FROM LeaveTypes lt
                        LEFT JOIN LeaveBalances lb ON lt.LeaveTypeID = lb.LeaveTypeID
                            AND lb.EmployeeID = @EmployeeID
                            AND lb.Year = YEAR(GETDATE())
                        WHERE lt.LeaveTypeName IN ('Annual', 'Casual', 'Short')
                        GROUP BY lt.LeaveTypeName, 
                                 CASE 
                                    WHEN lt.LeaveTypeName = 'Annual' THEN lt.MaxAnnualLeave
                                    WHEN lt.LeaveTypeName = 'Casual' THEN lt.MaxCasualLeave
                                    WHEN lt.LeaveTypeName = 'Short' THEN lt.MaxShortLeavePerMonth * 12
                                    ELSE 0
                                 END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", currentEmployeeID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string leaveType = reader["LeaveTypeName"].ToString();
                                int leavesTaken = Convert.ToInt32(reader["LeavesTaken"]);
                                int maxLeaves = Convert.ToInt32(reader["MaxLeaves"]);

                                double progressPercentage = (maxLeaves == 0) ? 0 : ((double)leavesTaken / maxLeaves) * 100;

                                UpdateProgressBar(leaveType, progressPercentage, leavesTaken, maxLeaves);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading leave progress: " + ex.Message);
            }
        }

        private void UpdateProgressBar(string leaveType, double percentage, int taken, int max)
        {
            switch (leaveType)
            {
                case "Annual":
                    ProgressAnnual.Value = (int)Math.Min(percentage, 100);
                    label1.Text = $"Annual Leave: {taken}/{max} ({percentage:F1}%)";
                    break;

                case "Casual":
                    ProgressCassual.Value = (int)Math.Min(percentage, 100);
                    label2.Text = $"Casual Leave: {taken}/{max} ({percentage:F1}%)";
                    break;

                case "Short":
                    ProgessShort.Value = (int)Math.Min(percentage, 100);
                    label3.Text = $"Short Leave: {taken}/{max} ({percentage:F1}%)";
                    break;

                default:
                    break;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLeaveProgress();
        }
    }
}
