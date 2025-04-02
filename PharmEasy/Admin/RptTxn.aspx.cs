using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_RptTxn : System.Web.UI.Page
{
    private string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPayTypeDropdown();
            /* BindTransactionGrid(); */// Load all transactions by default
            BindNameDropdown();
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            BindTransactionGrid();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("An error occurred while filtering the data. Please try again.");
        }
    }
    private void BindTransactionGrid()
    {
        try
        {
            DateTime? fromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
            DateTime? toDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);
            string payType = ddlPayType.SelectedValue;
            int? name = string.IsNullOrEmpty(ddlName.SelectedValue) ? (int?)null : Convert.ToInt32(ddlName.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("TRANSACTION_DETAIL", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PayType", string.IsNullOrEmpty(payType) ? (object)DBNull.Value : payType);
                    cmd.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value); // Pass integer or NULL

                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvTransactionDetails.DataSource = dt;
                        gvTransactionDetails.DataBind();
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            ShowErrorMessage("A database error occurred while retrieving transaction data. Please try again later.");
        }
        catch (FormatException ex)
        {
            ShowErrorMessage("Invalid date format. Please enter valid dates.");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("An unexpected error occurred while binding transaction data. Please try again later.");
        }
    }
    private void BindPayTypeDropdown()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT PAYTYPE FROM dbo.tbl_TransactionDetail", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlPayType.DataSource = reader;
                        ddlPayType.DataTextField = "PAYTYPE";
                        ddlPayType.DataValueField = "PAYTYPE";
                        ddlPayType.DataBind();
                    }
                }
            }

            // Insert a default item at the top of the dropdown list
            ddlPayType.Items.Insert(0, new ListItem("Select Payment Type", ""));
        }
        catch (SqlException ex)
        {
            ShowErrorMessage("A database error occurred while retrieving payment types. Please try again later.");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("An unexpected error occurred while binding payment types. Please try again later.");
        }
    }
    private void BindNameDropdown()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT PATIENT_ID AS ID, PATIENT_NAME AS NAME FROM dbo.tbl_Patients", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlName.DataSource = reader;
                        ddlName.DataTextField = "NAME";
                        ddlName.DataValueField = "ID"; 
                        ddlName.DataBind();
                    }
                }
            }

            // Insert a default item at the top of the dropdown list
            ddlName.Items.Insert(0, new ListItem("Select Name", ""));
        }
        catch (SqlException ex)
        {
            ShowErrorMessage("A database error occurred while retrieving names. Please try again later.");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("An unexpected error occurred while binding names. Please try again later.");
        }
    }
    private void ShowErrorMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message.Replace("'", "\\'") + "');", true);
    }
    protected void gvTransactionDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransactionDetails.PageIndex = e.NewPageIndex;
        BindTransactionGrid(); 
    }
}