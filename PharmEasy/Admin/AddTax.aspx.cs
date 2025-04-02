using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddTax : System.Web.UI.Page
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
        string taxName = txtTaxName.Text.Trim();
        string percentageText = txtPercentage.Text.Trim();
        bool isActive = chkIsActive.Checked;

        decimal percentage;
        if (string.IsNullOrEmpty(taxName) || !decimal.TryParse(percentageText, out percentage))
        {
            lblMessage.Text = "Please enter a valid tax name and percentage.";
            lblMessage.CssClass = "error-message";
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO [dbo].[tbl_TaxMaster] ([TAX_NM], [PERCENTAGE], [IS_ACTIVE], [DATE]) VALUES (@TaxName, @Percentage, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaxName", taxName);
                    cmd.Parameters.AddWithValue("@Percentage", percentage);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Tax added successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("TaxMaster.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Failed to add tax.";
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
        string taxName = txtTaxName.Text.Trim();
        string percentageText = txtPercentage.Text.Trim();
        bool isActive = chkIsActive.Checked;

        decimal percentage;
        if (string.IsNullOrEmpty(taxName) || !decimal.TryParse(percentageText, out percentage))
        {
            lblMessage.Text = "Please enter a valid tax name and percentage.";
            lblMessage.CssClass = "error-message";
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "UPDATE [dbo].[tbl_TaxMaster] SET [TAX_NM] = @TaxName, [PERCENTAGE] = @Percentage, [IS_ACTIVE] = @IsActive WHERE [TAX_ID] = @TaxId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaxId", Convert.ToInt32(Request.QueryString["id"]));
                    cmd.Parameters.AddWithValue("@TaxName", taxName);
                    cmd.Parameters.AddWithValue("@Percentage", percentage);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Tax updated successfully!";
                        lblMessage.CssClass = "success-message";
                        Response.Redirect("TaxMaster.aspx"); 
                    }
                    else
                    {
                        lblMessage.Text = "Failed to update tax.";
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
                SqlCommand cmd = new SqlCommand("SELECT [TAX_NM], [PERCENTAGE], [IS_ACTIVE] FROM [dbo].[tbl_TaxMaster] WHERE [TAX_ID]=@TAXID", conn);
                cmd.Parameters.AddWithValue("@TAXID", Convert.ToInt32(Request.QueryString["id"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtTaxName.Text = dt.Rows[0]["TAX_NM"].ToString();
                    txtPercentage.Text = dt.Rows[0]["PERCENTAGE"].ToString();
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