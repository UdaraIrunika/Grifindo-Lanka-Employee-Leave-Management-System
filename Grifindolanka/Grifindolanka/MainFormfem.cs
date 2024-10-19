using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grifindolanka
{
    public partial class MainFormfem : Form
    {
        public MainFormfem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            applyleave1.Visible = true;
            viwelh1.Visible = false;
            viwels1.Visible = false;
            leaveBalance1.Visible = false;
        }

        private void btnLs_Click(object sender, EventArgs e)
        {
            applyleave1.Visible = false;
            viwelh1.Visible = false;
            viwels1.Visible = true;
            leaveBalance1.Visible = false;
        }

        private void btnLh_Click(object sender, EventArgs e)
        {
            applyleave1.Visible = false;
            viwelh1.Visible = true;
            viwels1.Visible = false;
            leaveBalance1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Optionally, confirm with the user before logging out
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Clear any user session data here (if applicable)
                // For example: UserSession.CurrentUser = null; 

                // Optionally, close the current form
                this.Hide(); // Hides the current form

                // Show the login form (assuming you have a LoginForm)
                login loginForm = new login();
                loginForm.Show();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            applyleave1.Visible = false;
            viwelh1.Visible = false;
            viwels1.Visible = false;
            leaveBalance1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            applyleave1.Visible = false;
            viwelh1.Visible = false;
            viwels1.Visible = false;
            leaveBalance1.Visible = true;
        }
    }
}
