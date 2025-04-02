using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddProd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack)
        {
            BindCategories();
            BindBrands();
            BindGST();

            if (Request.QueryString["id"] != null)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = true;
                BindData();
            }
            else
            {
                btnAdd.Visible = true;
                btnEdit.Visible = false;
            }
            if (Request.QueryString["redirect"] != null)
            {
                ViewState["RedirectSource"] = Request.QueryString["redirect"];
            }
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string productName = txtProductName.Text.Trim();
        string content = txtContent.Text.Trim();
        int brandId = int.Parse(ddlBrand.SelectedValue);
        string manufacturer = txtCompanyName.Text.Trim();
        string unit = txtType.Text.Trim();
        string shelfNo = txtShelfNo.Text.Trim();
        bool isActive = chkIsActive.Checked;
        int categoryId = int.Parse(ddlCategory.SelectedValue);
        int taxId = int.Parse(ddlGST.SelectedValue);

        // Check if the product name already exists
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Check if the product already exists
                string checkQuery = "SELECT COUNT(*) FROM tbl_ProdMaster WHERE PROD_NM = @ProductName";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ProductName", productName);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        ShowAlert("Product name already exists.");
                        return;
                    }
                }

                // Insert the new product
                string query = "INSERT INTO tbl_ProdMaster (PROD_NM, CAT_ID, BRAND_ID, SHELF_NO, IS_ACTIVE, DATE, DESCRIPTION, MANUFACTURER, CONTENT, UNIT, TAX_ID) " +
                               "VALUES (@ProductName, @CategoryId, @BrandId, @ShelfNo, @IsActive, GETDATE(), @Description, @Manufacturer, @Content, @Unit, @TaxId);" +
                               "SELECT SCOPE_IDENTITY();";

                int prodId;
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@BrandId", brandId);
                    cmd.Parameters.AddWithValue("@ShelfNo", shelfNo);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Description", content);
                    cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@Unit", unit);
                    cmd.Parameters.AddWithValue("@TaxId", taxId);

                    prodId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                lblMessage.Text = "Product added successfully!";
                lblMessage.CssClass = "success-message";
                string redirectSource = ViewState["RedirectSource"] as string;
                if (!string.IsNullOrEmpty(redirectSource) && redirectSource == "AddPurchase")
                {
                    Response.Redirect("AddPurchase.aspx");
                }
                else
                {
                    Response.Redirect("ProdMaster.aspx");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error: {ex.Message}");
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string productName = txtProductName.Text.Trim();
        string content = txtContent.Text.Trim();
        int brandId = int.Parse(ddlBrand.SelectedValue);
        string manufacturer = txtCompanyName.Text.Trim();
        string unit = txtType.Text.Trim();
        string shelfNo = txtShelfNo.Text.Trim();
        bool isActive = chkIsActive.Checked;
        int categoryId = int.Parse(ddlCategory.SelectedValue);
        int taxId = int.Parse(ddlGST.SelectedValue);

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "UPDATE tbl_ProdMaster SET PROD_NM = @ProductName, CAT_ID = @CategoryId, BRAND_ID = @BrandId, SHELF_NO = @ShelfNo, " +
                               "IS_ACTIVE = @IsActive, DESCRIPTION = @Description, MANUFACTURER = @Manufacturer, CONTENT = @Content, UNIT = @Unit, TAX_ID = @TaxId " +
                               "WHERE PROD_ID = @ProdId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProdId", Convert.ToInt32(Request.QueryString["id"]));
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@BrandId", brandId);
                    cmd.Parameters.AddWithValue("@ShelfNo", shelfNo);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Description", content);
                    cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@Unit", unit);
                    cmd.Parameters.AddWithValue("@TaxId", taxId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Product updated successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("ProdMaster.aspx"); // Redirect to ProdMaster.aspx after updating
                    }
                    else
                    {
                        lblMessage.Text = "Failed to update product.";
                        lblMessage.CssClass = "error-message";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.CssClass = "error-message";
            }
        }
    }
    private void BindCategories()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT CAT_ID, CAT_NM FROM tbl_CatMaster";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlCategory.DataSource = reader;
                        ddlCategory.DataTextField = "CAT_NM";
                        ddlCategory.DataValueField = "CAT_ID";
                        ddlCategory.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.CssClass = "error-message";
            }
        }
        ddlCategory.Items.Insert(0, new ListItem("--Select Category--", "0"));
    }
    private void BindGST()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT TAX_ID, PERCENTAGE FROM tbl_TaxMaster WHERE IS_ACTIVE = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlGST.DataSource = reader;
                        ddlGST.DataTextField = "PERCENTAGE";
                        ddlGST.DataValueField = "TAX_ID";
                        ddlGST.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.CssClass = "error-message";
            }
        }
        ddlGST.Items.Insert(0, new ListItem("--Select GST--", "0"));
    }
    protected void BindData()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PROD_NM, CAT_ID, BRAND_ID, SHELF_NO, IS_ACTIVE, DESCRIPTION, MANUFACTURER, CONTENT, UNIT, TAX_ID " +
                               "FROM tbl_ProdMaster WHERE PROD_ID = @ProdId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProdId", Convert.ToInt32(Request.QueryString["id"]));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtProductName.Text = reader["PROD_NM"].ToString();
                            ddlCategory.SelectedValue = reader["CAT_ID"].ToString();
                            ddlBrand.SelectedValue = reader["BRAND_ID"].ToString();
                            txtShelfNo.Text = reader["SHELF_NO"].ToString();
                            chkIsActive.Checked = Convert.ToBoolean(reader["IS_ACTIVE"]);
                            txtContent.Text = reader["DESCRIPTION"].ToString();
                            txtCompanyName.Text = reader["MANUFACTURER"].ToString();
                            txtContent.Text = reader["CONTENT"].ToString();
                            txtType.Text = reader["UNIT"].ToString();
                            ddlGST.SelectedValue = reader["TAX_ID"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = $"Error: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
    }
    private void BindBrands()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT BRAND_ID, BRAND_NM FROM tbl_BrandMaster";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlBrand.DataSource = reader;
                        ddlBrand.DataTextField = "BRAND_NM";
                        ddlBrand.DataValueField = "BRAND_ID";
                        ddlBrand.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.CssClass = "error-message";
            }
        }
        ddlBrand.Items.Insert(0, new ListItem("--Select Brand--", "0"));
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string productName = txtProductName.Text.Trim();
        if (string.IsNullOrEmpty(productName))
        {
            ShowAlert("Product name cannot be empty.");
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM tbl_ProdMaster WHERE PROD_NM = @ProductName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        ShowAlert("Product name already exists.");
                    }
                    else
                    {
                        lblMessage.Text = ""; // Clear the label if no error
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error: {ex.Message}");
            }
        }
    }

    private void ShowAlert(string message)
    {
        string script = $"alert('{message}');";
        ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);

        lblMessage.Text = message;
        lblMessage.CssClass = "error-message";
    }

}