using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin_RptPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }

    }
    [WebMethod]
    public static List<string> GetReceiverNames(string prefixText, int count)
    {
        List<string> receiverNames = new List<string>();
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = @"
            SELECT DISTINCT u.NAME
            FROM [dbo].[tbl_Users] u
            LEFT JOIN [dbo].[tbl_Receipt] r ON u.UID = r.UID
            WHERE u.NAME LIKE @SearchText + '%' AND r.UID IS NULL";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                conn.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        receiverNames.Add(sdr["NAME"].ToString());
                    }
                }

                conn.Close();
            }
        }

        return receiverNames;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("PAYMENT_LIST", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Determine the statement based on the inputs
                int statement = 1; // Default to "All"
                if (!string.IsNullOrEmpty(txtSearchReceiver.Text))
                {
                    statement = 2; // By Receiver Name
                }
                else if (!string.IsNullOrEmpty(txtSearchDescription.Text))
                {
                    statement = 3; // By Description
                }
                else if (rbCash.Checked)
                {
                    statement = 4; // By Payment Type - Cash
                }
                else if (rbCredit.Checked)
                {
                    statement = 4; // By Payment Type - Credit
                }
                else if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
                {
                    statement = 6; // By Payment Date Range
                }
                else if (!string.IsNullOrEmpty(txtSearchFromDate.Text))
                {
                    statement = 5; // By Single Payment Date
                }

                // Add parameters to the command
                cmd.Parameters.AddWithValue("@STATEMENT", statement);
                cmd.Parameters.AddWithValue("@RECEIVER", txtSearchReceiver.Text);
                cmd.Parameters.AddWithValue("@DESCRIPTION", txtSearchDescription.Text);
                cmd.Parameters.AddWithValue("@PAYTYPE", rbCash.Checked ? "Cash" : rbCredit.Checked ? "Credit" : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PAYMENT_DATE", string.IsNullOrEmpty(txtSearchFromDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtSearchFromDate.Text));
                cmd.Parameters.AddWithValue("@FROM_DATE", string.IsNullOrEmpty(txtSearchFromDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtSearchFromDate.Text));
                cmd.Parameters.AddWithValue("@TO_DATE", string.IsNullOrEmpty(txtSearchToDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtSearchToDate.Text));

                // Execute the command and bind the result to the GridView
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridViewPayments.DataSource = dt;
                GridViewPayments.DataBind();
            }
        }
    }
    protected void GridViewPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewPayments.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}