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
    public partial class MainFormad : Form
    {
        public MainFormad()
        {
            InitializeComponent();
        }

        private void btnManageem_Click(object sender, EventArgs e)
        {
            manangeemp1.Visible = true;
            manageLeatyp1.Visible = false;
            epmanalea1.Visible = false;
            viwelh1.Visible = false;
            addDepartments1.Visible = false;
        }

        private void btnManaginglea_Click(object sender, EventArgs e)
        {
            manangeemp1.Visible = false;
            manageLeatyp1.Visible = false;
            epmanalea1.Visible = true;
            viwelh1.Visible = false;
            addDepartments1.Visible = false;
        }

        private void btnManagingLeaTyp_Click(object sender, EventArgs e)
        {
            manangeemp1.Visible = false;
            manageLeatyp1.Visible = true;
            epmanalea1.Visible = false;
            viwelh1.Visible = false;
            addDepartments1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            { 
                this.Hide(); 
                login loginForm = new login();
                loginForm.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            manangeemp1.Visible = false;
            manageLeatyp1.Visible = false;
            epmanalea1.Visible = false;
            viwelh1.Visible = true;
            addDepartments1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            manangeemp1.Visible = false;
            manageLeatyp1.Visible = false;
            epmanalea1.Visible = false;
            viwelh1.Visible = false;
            addDepartments1.Visible = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
