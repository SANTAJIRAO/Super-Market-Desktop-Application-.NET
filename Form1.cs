using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GoMartDB
{
    public partial class LoginFrom : Form
    {
        DBConnect dbcon = new DBConnect();
        public static string loginame, logintype;
        public LoginFrom()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            combRole.SelectedIndex = 1;
            txtUserName.Text = "Atul";
            txtPassword.Text = "123";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (combRole.SelectedIndex > 0)
                {
                    if (txtUserName.Text == String.Empty)
                    {
                        MessageBox.Show("Please Enter valid UserName", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUserName.Focus();
                        return;
                    }
                    if (txtPassword.Text == String.Empty)
                    {
                        MessageBox.Show("Please Enter Valid Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Focus();
                        return;
                    }
                    if (combRole.SelectedIndex > 0 && txtUserName.Text != String.Empty && txtPassword.Text != String.Empty)
                    {
                        if (combRole.Text == "Admin")
                        {
                            SqlCommand cmd = new SqlCommand("SELECT top 1 AdminID,[Password],FullName from tablAdmin where AdminId=@AdminId And [Password] = @Password", dbcon.Getcon());
                            cmd.Parameters.AddWithValue("@AdminID", txtUserName.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            dbcon.Opencon();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Login Successfully Welcome to Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginame = txtUserName.Text;
                                logintype = combRole.Text;
                                clrValue();
                                this.Hide();
                                frmMain fm = new frmMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login Please Check UserName and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (combRole.Text == "Seller")
                        {
                            SqlCommand cmd = new SqlCommand("SELECT top 1 sellerName,sellerPassword from tblSeller WHERE sellerName=@sellerName and SellerPassword=@SellerPassword", dbcon.Getcon());
                            cmd.Parameters.AddWithValue("@sellerName", txtUserName.Text);
                            cmd.Parameters.AddWithValue("@SellerPassword", txtPassword.Text);
                            dbcon.Opencon();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Login Successfully Welcome to Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginame = txtUserName.Text;
                                logintype = combRole.Text;
                                clrValue();
                                this.Hide();
                                frmMain fm = new frmMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login Please Check UserName and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter UserName or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clrValue();
                    }
                }
                else
                {
                    MessageBox.Show("Please select any role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clrValue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clrValue()
        {
            combRole.SelectedIndex = 0;
            txtUserName.Clear();
            txtPassword.Clear();
        }
    }
}
