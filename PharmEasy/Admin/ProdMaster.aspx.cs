using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ProdMaster : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCategoryDropdown();
        }
    }
    private void BindCategoryDropdown()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT [CAT_ID], [CAT_NM] FROM [tbl_CatMaster]";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlSearchCategory.DataSource = dt;
            ddlSearchCategory.DataTextField = "CAT_NM";
            ddlSearchCategory.DataValueField = "CAT_ID";
            ddlSearchCategory.DataBind();
            ddlSearchCategory.Items.Insert(0, new ListItem("--All Category--", "0"));
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchProducts();
    }
    private void SearchProducts()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("PROD_LIST", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@STATEMENT", 1); // Default to All Records
            if (!string.IsNullOrEmpty(txtSearchProductName.Text))
            {
                cmd.Parameters["@STATEMENT"].Value = 2;
                cmd.Parameters.AddWithValue("@PROD_NM", txtSearchProductName.Text);
            }
            else if (ddlSearchCategory.SelectedValue != "0")
            {
                cmd.Parameters["@STATEMENT"].Value = 3;
                cmd.Parameters.AddWithValue("@CAT_ID", ddlSearchCategory.SelectedValue);
            }
            else if (ddlSearchIsActive.SelectedValue != "2")
            {
                cmd.Parameters["@STATEMENT"].Value = 4;
                cmd.Parameters.AddWithValue("@IS_ACTIVE", ddlSearchIsActive.SelectedValue);
            }
            else if (!string.IsNullOrEmpty(txtSearchBrandName.Text))
            {
                cmd.Parameters["@STATEMENT"].Value = 5;
                cmd.Parameters.AddWithValue("@BRAND_NM", txtSearchBrandName.Text);
            }
            else if (!string.IsNullOrEmpty(txtSearchContentName.Text))
            {
                cmd.Parameters["@STATEMENT"].Value = 6;
                cmd.Parameters.AddWithValue("@CONTENT", txtSearchContentName.Text);
            }
            else if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                cmd.Parameters["@STATEMENT"].Value = 7;
                cmd.Parameters.AddWithValue("@PRODUCT_DATE", txtSearchFromDate.Text);
            }
            else if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                cmd.Parameters["@STATEMENT"].Value = 8;
                cmd.Parameters.AddWithValue("@FROM_DATE", txtSearchFromDate.Text);
                cmd.Parameters.AddWithValue("@TO_DATE", txtSearchToDate.Text);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridViewProducts.DataSource = dt;
            GridViewProducts.DataBind();
        }
    }

    protected void GridViewProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewProducts.PageIndex = e.NewPageIndex;
        SearchProducts();
    }
}