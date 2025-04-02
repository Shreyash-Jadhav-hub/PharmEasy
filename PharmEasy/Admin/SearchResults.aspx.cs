using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_SearchResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query = Request.QueryString["q"];
            if (!string.IsNullOrEmpty(query))
            {
                PerformSearch(query);
            }
            else
            {
                ShowAlert("No search term provided.");
            }
        }
    }
    protected void ddlSearchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string query = Request.QueryString["q"];
        if (!string.IsNullOrEmpty(query))
        {
            PerformSearch(query);
        }
    }
    private void PerformSearch(string query)
    {
        string searchType = ddlSearchType.SelectedValue;
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string sql = "";
            if (searchType == "Patient")
            {
                // SQL query to search patients, returning the top 5 recent entries
                sql = @"
            SELECT TOP 5 
                PATIENT_NAME,
                ADDRESS,
                MOBILE,
                DR_NAME,
                CITY,
                (SELECT SUM(CR - DR) FROM [dbo].[tbl_TransactionDetail] WHERE PATIENT_ID = p.PATIENT_ID) AS BALANCE
            FROM 
                [dbo].[tbl_Patients] p
            WHERE 
                PATIENT_NAME LIKE @Query OR 
                MOBILE LIKE @Query OR 
                ADDRESS LIKE @Query OR 
                DR_NAME LIKE @Query OR 
                CITY LIKE @Query
            ORDER BY 
                p.PATIENT_ID DESC"; // Assuming PATIENT_ID indicates the most recent

                // Set GridView columns visibility for Patient search
                gvResults.Columns[0].Visible = true; // PATIENT_NAME
                gvResults.Columns[1].Visible = true; // ADDRESS
                gvResults.Columns[2].Visible = true; // MOBILE
                gvResults.Columns[3].Visible = true; // DR_NAME
                gvResults.Columns[4].Visible = true; // CITY
                gvResults.Columns[5].Visible = true; // BALANCE

                // Hide Product columns
                for (int i = 6; i < gvResults.Columns.Count; i++)
                {
                    gvResults.Columns[i].Visible = false;
                }
            }
            else if (searchType == "Product")
            {
                // SQL query to search products, returning the top 5 recent entries
                sql = @"
            SELECT TOP 5 
                p.PROD_NM,
                
                b.BRAND_NM AS BRAND_NAME,
                p.SHELF_NO,
                p.MANUFACTURER,
                p.CONTENT,
                d.EXPIRY,
                s.AVAIL_STOCK
            FROM 
                [dbo].[tbl_ProdMaster] p
                INNER JOIN [dbo].[tbl_BrandMaster] b ON p.BRAND_ID = b.BRAND_ID
                INNER JOIN [dbo].[tbl_ProdDetail] d ON p.PROD_ID = d.PROD_ID
                INNER JOIN [dbo].[tbl_StockDetail] s ON p.PROD_ID = s.PROD_ID
            WHERE 
                p.PROD_NM LIKE @Query OR 
                p.DESCRIPTION LIKE @Query OR 
                p.MANUFACTURER LIKE @Query OR 
                p.CONTENT LIKE @Query
            ORDER BY 
                d.EXPIRY DESC"; // Assuming EXPIRY indicates the most recent

                // Set GridView columns visibility for Product search
                for (int i = 0; i < 6; i++)
                {
                    gvResults.Columns[i].Visible = false;
                }

                gvResults.Columns[6].Visible = true; // PROD_NM
             
                gvResults.Columns[7].Visible = true; // BRAND_NAME
                gvResults.Columns[8].Visible = true; // SHELF_NO
                gvResults.Columns[9].Visible = true; // MANUFACTURER
                gvResults.Columns[10].Visible = true; // CONTENT
                gvResults.Columns[11].Visible = true; // EXPIRY
                gvResults.Columns[12].Visible = true; // AVAIL_STOCK
            }

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Query", "%" + query + "%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    gvResults.DataSource = reader;
                    gvResults.DataBind();
                }
                else
                {
                    gvResults.DataSource = null;
                    gvResults.DataBind();
                    lblMessage.Text = "No results found.";
                }
            }
        }
    }

    private void ShowAlert(string message)
    {
        string script = $"alert('{message}');";
        ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
     
    }
}