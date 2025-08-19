using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GoMartDB
{
    public partial class frmCategory : Form
    {
        DBConnect dbcon = new DBConnect();

        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            lblCatID.Visible = false;
            BindCategory();
        }

        private void btnAddCat_Click(object sender, EventArgs e)
        {
            if (txtCatname.Text == String.Empty)
            {
                MessageBox.Show("Please Enter valid CategoryName", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCatname.Focus();
                return;
            }
            if (rbtCatDesc.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Category Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rbtCatDesc.Focus();
                return;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select CategoryName from tblCategory where CategoryName=@CategoryName", dbcon.Getcon());
                cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                dbcon.Opencon();

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show("CategoryName already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                }
                else
                {
                    cmd = new SqlCommand("spCatInsert", dbcon.Getcon());
                    cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                    cmd.Parameters.AddWithValue("@CategoryDesc", rbtCatDesc.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Category Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindCategory();
                    }
                }
                dbcon.Closecon();
            }
        }

        private void txtClear()
        {
            txtCatname.Clear();
            rbtCatDesc.Clear();
        }

        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("select CatID as CategoryID, CategoryName, CategoryDesc as CategoryDescription from tblCategory", dbcon.Getcon());
            dbcon.Opencon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.Closecon();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblCatID.Visible = true;
            btnAddCat.Visible = false;

            lblCatID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtCatname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            rbtCatDesc.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please select CategoryID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatname.Focus();
                    return;
                }

                if (txtCatname.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Valid CategoryName", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatname.Focus();
                    return;
                }
                if (rbtCatDesc.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Category Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rbtCatDesc.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select CategoryName from tblCategory where CategoryName=@CategoryName", dbcon.Getcon());
                    cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                    dbcon.Opencon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("CategoryNamealready exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spCatUpdate", dbcon.Getcon());
                        cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(lblCatID.Text));
                        cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                        cmd.Parameters.AddWithValue("@CategoryDesc", rbtCatDesc.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        dbcon.Closecon();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindCategory();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAddCat.Visible = true;
                            lblCatID.Visible = false;
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
                if (lblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please select CategoryID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lblCatID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spCatDelete",dbcon.Getcon());
                        cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(lblCatID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                       dbcon.Opencon();

                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindCategory();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAddCat.Visible = true;
                            lblCatID.Visible = false;



                        }
                        else
                        {
                            MessageBox.Show(" Delete Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();

                        }
                        dbcon.Closecon();
                    }


                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
