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

namespace GoMartDB
{
    public partial class frmAddNewseller : Form
    {
        DBConnect dbcon = new DBConnect();
        public frmAddNewseller()
        {
            InitializeComponent();
        }

        private void frmAddNewseller_Load(object sender, EventArgs e)
        {
            lblSellerID.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            BindSeller();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtSellerName.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Seller name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSellerName.Focus();
                return;
            }
            if (txtPassword.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select  SellerName from tblSeller where sellerName=@sellerName", dbcon.Getcon());
                cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                dbcon.Opencon();

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show(string.Format("Seller Name already exist"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtClear();
                }
                else
                {
                    cmd = new SqlCommand("spSellerInsert", dbcon.Getcon());
                    cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                    cmd.Parameters.AddWithValue("@SellerAge", Convert.ToInt32(txtAge.Text));
                    cmd.Parameters.AddWithValue("@SellerPhone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@SellerPassword", txtPassword.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Seller Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindSeller();
                    }
                }
                dbcon.Closecon();
            }
        }
        private void txtClear()
        {
            txtSellerName.Clear();
            txtAge.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblSellerID.Text == String.Empty)
                {
                    MessageBox.Show("Please select SellerID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (txtSellerName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerName.Focus();
                    return;
                }
                if (txtPassword.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select  SellerName from tblSeller where sellerName=@sellerName", dbcon.Getcon());
                    cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);

                    dbcon.Opencon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Seller Name already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spSellerUpdate", dbcon.Getcon());
                        cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt32(lblSellerID.Text));
                        cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                        cmd.Parameters.AddWithValue("@SellerAge", Convert.ToInt32(txtAge.Text));
                        cmd.Parameters.AddWithValue("@SellerPhone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@SellerPassword", txtPassword.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        dbcon.Closecon();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindSeller();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAdd.Visible = true;
                            lblSellerID.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show(" Updated Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();

                        }

                    }
                    dbcon.Closecon();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                if (lblSellerID.Text == String.Empty)
                {
                    MessageBox.Show("Please select CategoryID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lblSellerID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spSellerDelete", dbcon.Getcon());
                        cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt32(lblSellerID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbcon.Opencon();

                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindSeller();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAdd.Visible = true;
                            lblSellerID.Visible = false;



                        }
                        else
                        {
                            MessageBox.Show(" Delete Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();

                        }
                       dbcon.Closecon ();
                    }


                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BindSeller()
        {
            SqlCommand cmd = new SqlCommand("select * from tblSeller", dbcon.Getcon());
            dbcon.Opencon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.Closecon();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblSellerID.Visible = true;
            btnAdd.Visible = false;

            lblSellerID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtSellerName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtAge.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtPhone.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }
    }
}

