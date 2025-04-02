using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Admin_CustDetail : System.Web.UI.Page
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
        string query = @"
            SELECT p.PATIENT_ID, p.PATIENT_NAME, p.ADDRESS, p.MOBILE, p.DR_NAME, p.CITY, 
                   ISNULL(t.BALANCE, 0) AS BALANCE
            FROM tbl_Patients p
            LEFT JOIN (
                SELECT PATIENT_ID, SUM(BALANCE) AS BALANCE
                FROM tbl_TransactionDetail
                GROUP BY PATIENT_ID
            ) t ON p.PATIENT_ID = t.PATIENT_ID
            WHERE 1=1";

        if (!string.IsNullOrEmpty(txtSearchPatientName.Text))
        {
            query += " AND p.PATIENT_NAME LIKE @PatientName";
        }
        if (!string.IsNullOrEmpty(txtSearchMobile.Text))
        {
            query += " AND p.MOBILE LIKE @Mobile";
        }
        if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
        {
            query += " AND t.DATE BETWEEN @FromDate AND @ToDate";
        }

        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (!string.IsNullOrEmpty(txtSearchPatientName.Text))
                {
                    cmd.Parameters.AddWithValue("@PatientName", "%" + txtSearchPatientName.Text + "%");
                }
                if (!string.IsNullOrEmpty(txtSearchMobile.Text))
                {
                    cmd.Parameters.AddWithValue("@Mobile", "%" + txtSearchMobile.Text + "%");
                }
                if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
                {
                    cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(txtSearchFromDate.Text));
                    cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(txtSearchToDate.Text));
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridViewCustomers.DataSource = dt;
                GridViewCustomers.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("An error occurred while fetching data: " + ex.Message);
        }
    }
    protected void GridViewCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewCustomers.PageIndex = e.NewPageIndex;
        BindGrid();
    }
    private void ShowErrorMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message.Replace("'", "\\'") + "');", true);
    }
}