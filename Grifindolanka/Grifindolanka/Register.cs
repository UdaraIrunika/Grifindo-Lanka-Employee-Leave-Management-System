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
using System.Security.Cryptography;


namespace Grifindolanka
{
    public partial class Register : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-P8R14E4\SQLEXPRESS;Initial Catalog=GrifindoDB;Integrated Security=True;TrustServerCertificate=True");

        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Check if passwords match
            if (txtPass.Text != txtCpass.Text)
            {
                MessageBox.Show("Passwords do not match!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if all required fields are filled
            if (string.IsNullOrEmpty(txtFname.Text) ||
                string.IsNullOrEmpty(txtUsername.Text) ||
                string.IsNullOrEmpty(txtPass.Text) ||
                string.IsNullOrEmpty(txtCpass.Text) ||
                string.IsNullOrEmpty(txtDepartment.Text) ||
                cbRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connect.Open();

                // Check if the username already exists
                string checkUsername = "SELECT * FROM Employee WHERE Username = @username";
                using (SqlCommand checkUser = new SqlCommand(checkUsername, connect))
                {
                    checkUser.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count >= 1)
                    {
                        MessageBox.Show(txtUsername.Text + " already exists", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Insert new employee data
                        string insertData = "INSERT INTO Employee (FullName, Username, PasswordHash, Department, Role, DateJoined) " +
                                            "VALUES(@fullName, @username, @password, @department, @role, @dateJoined)";
                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@fullName", txtFname.Text.Trim());
                            cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                            cmd.Parameters.AddWithValue("@password", txtPass.Text.Trim()); // Directly store the password
                            cmd.Parameters.AddWithValue("@department", txtDepartment.Text.Trim());
                            cmd.Parameters.AddWithValue("@role", cbRole.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@dateJoined", dtpDate.Value.Date);  // Use DateTimePicker's value

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Registered successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Switch to the login form
                            login lForm = new login();
                            lForm.Show();
                            this.Hide();
                        }
                    }
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


        private void btnLogin_Click(object sender, EventArgs e)
        {
            login sForm = new login();
            sForm.Show();
            this.Hide();
        }

        private void cbShowpass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowpass.Checked)
            {
                txtPass.PasswordChar = '*';
            }
            else
            {
                txtPass.PasswordChar = '\0';
            }
            if (cbShowpass.Checked)
            {
                txtCpass.PasswordChar = '*';
            }
            else
            {
                txtCpass.PasswordChar = '\0';
            }
        }
    }
}
