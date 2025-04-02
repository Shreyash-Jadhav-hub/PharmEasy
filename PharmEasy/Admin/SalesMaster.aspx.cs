using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_SalesMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSalesGrid();
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindSalesGrid();
    }
    private void BindSalesGrid()
    {
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("SALES_LIST", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(txtFromDate.Text) ? (object)DBNull.Value : txtFromDate.Text);
                cmd.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(txtToDate.Text) ? (object)DBNull.Value : txtToDate.Text);
                cmd.Parameters.AddWithValue("@BillNo", string.IsNullOrEmpty(txtSearchBillNo.Text) ? (object)DBNull.Value : Convert.ToInt32(txtSearchBillNo.Text));
                cmd.Parameters.AddWithValue("@PatientName", ddlSearchPatientName.SelectedItem.Text == "--All Patients--" ? (object)DBNull.Value : ddlSearchPatientName.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@PaymentMode", ddlSearchPaymentMode.SelectedItem.Text == "--All Modes--" ? (object)DBNull.Value : ddlSearchPaymentMode.SelectedItem.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewSales.DataSource = dt;
                GridViewSales.DataBind();
            }
        }
    }

    private void PopulatePatientNames()
    {
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT PATIENT_ID, PATIENT_NAME FROM tbl_Patients", conn))
            {
                conn.Open();
                ddlSearchPatientName.DataSource = cmd.ExecuteReader();
                ddlSearchPatientName.DataTextField = "PATIENT_NAME";
                ddlSearchPatientName.DataValueField = "PATIENT_ID";
                ddlSearchPatientName.DataBind();
            }
        }

        ddlSearchPatientName.Items.Insert(0, new ListItem("--All Patients--", "0"));
    }

    protected void GridViewSales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      GridViewSales.PageIndex = e.NewPageIndex;
        BindSalesGrid();
    }
}