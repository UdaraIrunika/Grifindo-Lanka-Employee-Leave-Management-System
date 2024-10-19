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
    public partial class login : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");

        public login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Register sForm = new Register();
            sForm.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) ||
            string.IsNullOrEmpty(txtPass.Text) ||
            cbRole.SelectedItem == null || string.IsNullOrEmpty(cbRole.SelectedItem.ToString()))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        // Query to check user credentials and role
                        string selectData = "SELECT * FROM Employee WHERE Username = @username AND PasswordHash = @pass AND Role = @role";
                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {
                            // Adding parameters for Username, Password, and Role
                            cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());

                            // Hash the input password here if needed (not shown, assuming plain-text for now)
                            cmd.Parameters.AddWithValue("@pass", txtPass.Text.Trim());

                            // Role is a string from the Users table
                            cmd.Parameters.AddWithValue("@role", cbRole.SelectedItem.ToString());

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            // Check if any matching user is found
                            if (table.Rows.Count >= 1)
                            {
                                string userRole = cbRole.SelectedItem.ToString();

                                MessageBox.Show("Logged In successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Role-based form navigation
                                if (userRole == "Admin")
                                {
                                    MainFormad mainForm = new MainFormad();
                                    mainForm.Show();
                                }
                                else if (userRole == "Employee")
                                {
                                    MainFormfem mainFormfcolo = new MainFormfem();
                                    mainFormfcolo.Show();
                                }

                                this.Hide(); // Hide the current login form
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Username/Password/Role", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Connecting: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void cbShowpass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowpass.Checked)
            {
                txtPass.PasswordChar = '\0';
            }
            else
            {
                txtPass.PasswordChar = '*';
            }
        }
    }
}
