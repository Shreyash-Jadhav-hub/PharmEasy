using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EmpDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
            BindGrid();
    }
    private void BindGrid()
    {
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("USER_DETAIL", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(txtSearchFromDate.Text) ? (object)DBNull.Value : txtSearchFromDate.Text);
                cmd.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(txtSearchToDate.Text) ? (object)DBNull.Value : txtSearchToDate.Text);
                cmd.Parameters.AddWithValue("@Username", string.IsNullOrEmpty(txtSearchUsername.Text) ? (object)DBNull.Value : txtSearchUsername.Text);
                cmd.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(txtSearchName.Text) ? (object)DBNull.Value : txtSearchName.Text);
                cmd.Parameters.AddWithValue("@Role", ddlSearchRole.SelectedValue == "0" ? (object)DBNull.Value : ddlSearchRole.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewEmployees.DataSource = dt;
                GridViewEmployees.DataBind();
            }
        }
    }
    protected void GridViewEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewEmployees.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}