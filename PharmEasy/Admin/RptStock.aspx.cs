using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_RptStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProductDropdown();
        }
    }
    private void LoadProductDropdown()
    {
        string constr = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT PROD_ID, PROD_NM FROM tbl_ProdMaster", con))
            {
                con.Open();
                ddlProduct.DataSource = cmd.ExecuteReader();
                ddlProduct.DataTextField = "PROD_NM";
                ddlProduct.DataValueField = "PROD_ID";
                ddlProduct.DataBind();
                con.Close();
            }
        }
        ddlProduct.Items.Insert(0, new ListItem("--Select Product--", "0"));
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
        if (gvStockDetails.Rows.Count == 0)
        {
            string script = "alert('Error: No stock details found for the selected criteria.');";
            ClientScript.RegisterStartupScript(this.GetType(), "NoStockFoundError", script, true);
        }
    }
    private void BindGrid()
    {
        string constr = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("STOCK_DETAIL", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(txtFromDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(txtToDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtToDate.Text));
                cmd.Parameters.AddWithValue("@ProductName", ddlProduct.SelectedIndex == 0 ? (object)DBNull.Value : ddlProduct.SelectedItem.Text);

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();
                }
            }
        }
    }


    protected void gvStockDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvStockDetails.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}