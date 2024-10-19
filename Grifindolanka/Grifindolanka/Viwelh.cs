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
    public partial class Viwelh : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public Viwelh()
        {
            InitializeComponent();
            LoadData();  // Load data when form initializes
        }
        private void LoadData()
        {
            using (SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True")) ;
            {
                connect.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM LeaveHistory", connect);
                DataTable dt = new DataTable();
                sqlDa.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
