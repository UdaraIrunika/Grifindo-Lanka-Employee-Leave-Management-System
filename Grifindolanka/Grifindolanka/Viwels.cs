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
    public partial class Viwels : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public Viwels()
        {
            InitializeComponent();
            LoadData();  
        }


        private void LoadData()
        {
            string connectionString = @"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True";
            string query = "SELECT ApplicationID, EmployeeID, LeaveTypeID, LeaveStartDate, LeaveEndDate, ApplicationDate, Status, Remarks FROM LeaveApplications";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connect);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading leave applications: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
