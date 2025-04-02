using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddUnit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("Login.aspx");
        }
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string unitName = txtUnitName.Text.Trim();
        string description = txtDescription.Text.Trim();
        bool isActive = chkIsActive.Checked;

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO [dbo].[tbl_UnitMaster] ([UNIT_NM], [DESCRIPTION], [IS_ACTIVE], [DATE]) " +
                               "VALUES (@UnitName, @Description, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UnitName", unitName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Unit added successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("UnitMaster.aspx"); 
                    }
                    else
                    {
                        lblMessage.Text = "Failed to add unit.";
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
        string unitName = txtUnitName.Text.Trim();
        string description = txtDescription.Text.Trim();
        bool isActive = chkIsActive.Checked;

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "UPDATE [dbo].[tbl_UnitMaster] " +
                               "SET [UNIT_NM] = @UnitName, [DESCRIPTION] = @Description, [IS_ACTIVE] = @IsActive " +
                               "WHERE [UNIT_ID] = @UnitId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UnitId", Convert.ToInt32(Request.QueryString["id"]));
                    cmd.Parameters.AddWithValue("@UnitName", unitName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Unit updated successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("UnitMaster.aspx"); 
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
                SqlCommand cmd = new SqlCommand("SELECT [UNIT_NM], [DESCRIPTION], [IS_ACTIVE] FROM [dbo].[tbl_UnitMaster] WHERE [UNIT_ID]=@UNITID", conn);
                cmd.Parameters.AddWithValue("@UNITID", Convert.ToInt32(Request.QueryString["id"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtUnitName.Text = dt.Rows[0]["UNIT_NM"].ToString();
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