using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddCat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            BindParentCategories();
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

    private void BindParentCategories()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT CAT_ID, CAT_NM FROM tbl_CatMaster WHERE [PARENT_CID] IS NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlParentCategory.DataSource = reader;
                        ddlParentCategory.DataTextField = "CAT_NM";
                        ddlParentCategory.DataValueField = "CAT_ID";
                        ddlParentCategory.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string script = $"alert('Error: {ex.Message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "LoadParentCategoriesError", script, true);
            }
            finally
            {
                conn.Close();
            }
        }

        ddlParentCategory.Items.Insert(0, new ListItem("--Select Parent Category--", "0"));
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string categoryName = txtCategoryName.Text.Trim();
        string description = txtDescription.Text.Trim();
        string parentCategoryId = ddlParentCategory.SelectedValue;
        bool isActive = chkIsActive.Checked;

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO [dbo].[tbl_CatMaster] ([CAT_NM], [PARENT_CID], [DESCRIPTION], [IS_ACTIVE], [DATE]) VALUES (@CategoryName, @ParentCategoryID, @Description, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    if(ddlParentCategory.SelectedValue != "0")
                    {
                        cmd.Parameters.AddWithValue("@ParentCategoryID", parentCategoryId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ParentCategoryID", DBNull.Value);
                    }
                    
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Category added successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("CatMaster.aspx"); // Redirect to IvnCat.aspx after adding
                    }
                    else
                    {
                        lblMessage.Text = "Failed to add category.";
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
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "UPDATE [dbo].[tbl_CatMaster] SET [CAT_NM] = @CategoryName, [PARENT_CID] = @ParentCategoryID, [DESCRIPTION] = @Description, [IS_ACTIVE] = @IsActive WHERE [CAT_ID] = @CategoryId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(Request.QueryString["id"]));
                    cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                    cmd.Parameters.AddWithValue("@ParentCategoryID", ddlParentCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Category updated successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("CatMaster.aspx"); 
                    }
                    else
                    {
                        lblMessage.Text = "Failed to update category.";
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
                SqlCommand cmd = new SqlCommand("SELECT [CAT_NM], [PARENT_CID], [DESCRIPTION], [IS_ACTIVE] FROM [dbo].[tbl_CatMaster] WHERE [CAT_ID]=@CATID", conn);
                cmd.Parameters.AddWithValue("@CATID", Convert.ToInt32(Request.QueryString["id"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ddlParentCategory.SelectedValue = dt.Rows[0]["PARENT_CID"].ToString();
                    txtCategoryName.Text = dt.Rows[0]["CAT_NM"].ToString();
                    txtDescription.Text = dt.Rows[0]["DESCRIPTION"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IS_ACTIVE"]);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.CssClass = "error-message";
        }
        finally
        {
        }
    }
}