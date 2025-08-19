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
    public partial class AddProduct : Form
    {
        DBConnect dbcon = new DBConnect();
        public AddProduct()
        {
            InitializeComponent();
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            BindCategory();
            lblProdID.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            SearchByCategory();

        }
        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbcon.Getcon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbcon.Opencon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CatID";
            dbcon.Closecon();
        }
        private void SearchByCategory()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbcon.Getcon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbcon.Opencon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbSearch.DataSource = dt;
            cmbSearch.DisplayMember = "CategoryName";
            cmbSearch.ValueMember = "CatID";
            dbcon.Closecon();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {


                if (txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Product name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;
                }
                else if (Convert.ToInt32(txtPrice.Text) < 0 || txtPrice.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter valid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                if (txtQty.Text == String.Empty || Convert.ToInt32(txtQty.Text) < 0)
                {
                    MessageBox.Show("Please Enter Q", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbcon.Getcon());
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbcon.Opencon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Product Name already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spInsertProduct", dbcon.Getcon());
                        cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                        cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQty.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
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
        private void BindProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList", dbcon.Getcon());
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.Opencon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                dbcon.Closecon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void txtClear()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblProdID.Text == "" && txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Product ID and name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;
                }
                else if (txtPrice.Text == String.Empty && Convert.ToInt32(txtPrice.Text) >= 0)
                {
                    MessageBox.Show("Please Enter Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                if (txtQty.Text == String.Empty && Convert.ToInt32(txtQty.Text) >= 0)
                {
                    MessageBox.Show("Please Enter Q", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else
                {
                    /*SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbconn.GetConn());
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbconn.OpenConn();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Product Name already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        


                    }*/
                    SqlCommand cmd = new SqlCommand("spUpdateProduct", dbcon.Getcon());
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQty.Text));
                    cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProdID.Text));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbcon.Opencon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Product Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindProductList();
                        lblProdID.Visible = false;
                        btnAdd.Visible = true;
                        btnUpdate.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Updation Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    dbcon.Closecon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                lblProdID.Visible = true;
                btnAdd.Visible = false;

                lblProdID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtProdName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                cmbCategory.SelectedValue = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtPrice.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtQty.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
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
                if (lblProdID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Product ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lblProdID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spDeleteProduct", dbcon.Getcon());
                        cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProdID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbcon.Opencon();

                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            btnAdd.Visible = true;
                            lblProdID.Visible = false;



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

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Searched_ProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_SearchByCat", dbcon.Getcon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbSearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.Opencon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                dbcon.Closecon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Searched_ProductList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BindProductList();
        }
    }
}




    
