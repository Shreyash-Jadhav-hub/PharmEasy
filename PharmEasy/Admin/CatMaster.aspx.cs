using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CatMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindParentCategories();
            Bind_Data();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_Data();
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
                        ddlSearchParentCategory.DataSource = reader;
                        ddlSearchParentCategory.DataTextField = "CAT_NM";
                        ddlSearchParentCategory.DataValueField = "CAT_ID";
                        ddlSearchParentCategory.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string script = $"alert('Error: {ex.Message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "LoadParentCategoriesError", script, true);
            }
        }

        ddlSearchParentCategory.Items.Insert(0, new ListItem("--All Categories--", "0"));
    }
    public void Bind_Data()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("CATEGORY_LIST", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(txtSearchCategoryName.Text) && ddlSearchParentCategory.SelectedValue == "0" && ddlSearchIsActive.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 2);
                    cmd.Parameters.AddWithValue("@CATEGORY_NM", txtSearchCategoryName.Text);
                }
                else if (string.IsNullOrEmpty(txtSearchCategoryName.Text) && ddlSearchParentCategory.SelectedValue != "0" && ddlSearchIsActive.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 3);
                    cmd.Parameters.AddWithValue("@PARENT_CATEGORY", ddlSearchParentCategory.SelectedValue);
                }
                else if (string.IsNullOrEmpty(txtSearchCategoryName.Text) && ddlSearchParentCategory.SelectedValue == "0" && ddlSearchIsActive.SelectedValue != "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 4);
                    cmd.Parameters.AddWithValue("@ACTIVE", ddlSearchIsActive.SelectedValue);
                }
                else if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 5);
                    cmd.Parameters.AddWithValue("@FROM_DATE", txtSearchFromDate.Text);
                    cmd.Parameters.AddWithValue("@TO_DATE", txtSearchToDate.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 1);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    GridViewCategories.DataSource = dt;
                    GridViewCategories.DataBind();
                }
                else
                {
                    GridViewCategories.DataSource = null;
                    GridViewCategories.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            string script = $"alert('Error: {ex.Message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "SearchError", script, true);
        }
    }
    protected void GridViewCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewCategories.PageIndex = e.NewPageIndex;
        Bind_Data();
    }
}