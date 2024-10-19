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
    public partial class viweleavehistory : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");
        public viweleavehistory()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                connect.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM LeaveHistory", connect);

                DataTable dt = new DataTable();
                sqlDa.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

    }
}
