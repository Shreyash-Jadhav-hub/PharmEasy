using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddBrand : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = true;
                    Bind_Data();
                }
                else
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = false;
                }
            }
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string brandName = txtBrandName.Text.Trim();
        string description = txtDescription.Text.Trim();
        bool isActive = chkIsActive.Checked;

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO [dbo].[tbl_BrandMaster] ([BRAND_NM], [DESCRIPTION], [IS_ACTIVE], [DATE]) " +
                               "VALUES (@BrandName, @Description, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BrandName", brandName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Brand added successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("BrandMaster.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Failed to add brand.";
                        lblMessage.CssClass = "error-message";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.CssClass = "error-message";
            }
            finally
            {
                conn.Close();
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string brandName = txtBrandName.Text.Trim();
        string description = txtDescription.Text.Trim();
        bool isActive = chkIsActive.Checked;

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "UPDATE [dbo].[tbl_BrandMaster] " +
                               "SET [BRAND_NM] = @BrandName, [DESCRIPTION] = @Description, [IS_ACTIVE] = @IsActive " +
                               "WHERE [BRAND_ID] = @BrandId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BrandId", Convert.ToInt32(Request.QueryString["id"]));
                    cmd.Parameters.AddWithValue("@BrandName", brandName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Brand updated successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("BrandMaster.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Failed to update unit.";
                        lblMessage.CssClass = "error-message";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.CssClass = "error-message";
            }
            finally
            {
                conn.Close();
            }
        }
    }
    protected void Bind_Data()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT [BRAND_NM], [DESCRIPTION], [IS_ACTIVE] FROM [dbo].[tbl_BrandMaster] WHERE [BRAND_ID]=@BRANDID", conn);
                cmd.Parameters.AddWithValue("@BrandId", Convert.ToInt32(Request.QueryString["id"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtBrandName.Text = dt.Rows[0]["BRAND_NM"].ToString();
                    txtDescription.Text = dt.Rows[0]["DESCRIPTION"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IS_ACTIVE"]);
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
