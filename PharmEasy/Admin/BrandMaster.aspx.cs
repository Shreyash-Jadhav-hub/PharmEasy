using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_BrandMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind_Data();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_Data();
    }
    public void Bind_Data()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("BRAND_LIST", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(txtSearchBrandName.Text.Trim()) && ddlSearchIsActive.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 2);
                    cmd.Parameters.AddWithValue("@Brand_NM", txtSearchBrandName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ACTIVE", DBNull.Value);
                }
                else if (string.IsNullOrEmpty(txtSearchBrandName.Text.Trim()) && ddlSearchIsActive.SelectedValue != "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 3);
                    cmd.Parameters.AddWithValue("@Brand_NM", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ACTIVE", ddlSearchIsActive.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 1);
                    cmd.Parameters.AddWithValue("@Brand_NM", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ACTIVE", DBNull.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    GridViewBrands.DataSource = dt;
                    GridViewBrands.DataBind();
                }
                else
                {
                    GridViewBrands.DataSource = null;
                    GridViewBrands.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            string script = $"alert('Error: {ex.Message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "SearchError", script, true);
        }
    }
}