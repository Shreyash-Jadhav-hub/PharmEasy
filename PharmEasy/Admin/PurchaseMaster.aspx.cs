using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PurchaseMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSuppliers();
            //Bind_Data();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_Data();
    }
    private void BindSuppliers()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                // Adjust your query to match your database structure for suppliers
                string query = "SELECT DISTINCT SUPPLIER_NM FROM dbo.tbl_PurchaseMaster";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlSearchSupplierName.DataSource = reader;
                        ddlSearchSupplierName.DataTextField = "SUPPLIER_NM";
                        ddlSearchSupplierName.DataValueField = "SUPPLIER_NM"; // Use appropriate value field
                        ddlSearchSupplierName.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string script = $"alert('Error: {ex.Message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "LoadSuppliersError", script, true);
            }
            finally
            {
                conn.Close();
            }
        }

        // Add the default "--All Suppliers--" option at index 0
        ddlSearchSupplierName.Items.Insert(0, new ListItem("--All Suppliers--", "0"));
    }
    public void Bind_Data()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PURCHASE_LIST", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                int statement = 1; // Default: All purchases

                if (!string.IsNullOrEmpty(txtSearchInvoiceNo.Text) && ddlSearchSupplierName.SelectedValue == "0")
                {
                    statement = 2;
                    cmd.Parameters.AddWithValue("@INVOICE_NO", txtSearchInvoiceNo.Text);
                }
                else if (ddlSearchSupplierName.SelectedValue != "0" && string.IsNullOrEmpty(txtSearchInvoiceNo.Text))
                {
                    statement = 3;
                    cmd.Parameters.AddWithValue("@SUPPLIER_NM", ddlSearchSupplierName.SelectedItem.Text);
                }
                else if (!string.IsNullOrEmpty(txtSearchInvoiceNo.Text) && ddlSearchSupplierName.SelectedValue != "0")
                {
                    statement = 2;
                    cmd.Parameters.AddWithValue("@INVOICE_NO", txtSearchInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("@SUPPLIER_NM", ddlSearchSupplierName.SelectedItem.Text);
                }

                if (!string.IsNullOrEmpty(ddlSearchPaymentMode.SelectedValue) && ddlSearchPaymentMode.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@PAYTYPE", ddlSearchPaymentMode.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PAYTYPE", DBNull.Value); // Default: All payment modes
                }

                if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
                {
                    DateTime fromDate, toDate;
                    if (DateTime.TryParse(txtSearchFromDate.Text, out fromDate) && DateTime.TryParse(txtSearchToDate.Text, out toDate))
                    {
                        if (ddlSearchSupplierName.SelectedValue != "0")
                        {
                            statement = 7; // Filter by supplier name and date range
                            cmd.Parameters.AddWithValue("@FROM_DATE", fromDate);
                            cmd.Parameters.AddWithValue("@TO_DATE", toDate);
                        }
                        else
                        {
                            statement = 6; // Filter by date range
                            cmd.Parameters.AddWithValue("@FROM_DATE", fromDate);
                            cmd.Parameters.AddWithValue("@TO_DATE", toDate);
                        }
                    }
                    else
                    {
                        // Handle invalid date format if necessary
                    }
                }
                else if (!string.IsNullOrEmpty(txtSearchFromDate.Text) || !string.IsNullOrEmpty(txtSearchToDate.Text))
                {
                    DateTime singleDate;
                    if (DateTime.TryParse(txtSearchFromDate.Text, out singleDate))
                    {
                        statement = 5; // Filter by single purchase date
                        cmd.Parameters.AddWithValue("@PURCHASE_DATE", singleDate);
                    }
                    else if (DateTime.TryParse(txtSearchToDate.Text, out singleDate))
                    {
                        statement = 5; // Filter by single purchase date
                        cmd.Parameters.AddWithValue("@PURCHASE_DATE", singleDate);
                    }
                    else
                    {
                        // Handle invalid date format if necessary
                    }
                }

                cmd.Parameters.AddWithValue("@STATEMENT", statement);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    GridViewPurchases.DataSource = dt;
                    GridViewPurchases.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GridViewPurchases.DataSource = dt;
                    GridViewPurchases.DataBind();
                    GridViewPurchases.Rows[0].Cells.Clear();
                    GridViewPurchases.Rows[0].Cells.Add(new TableCell());
                    GridViewPurchases.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GridViewPurchases.Rows[0].Cells[0].Text = "No Record Found";
                    GridViewPurchases.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void GridViewPurchases_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewPurchases.PageIndex = e.NewPageIndex;
        Bind_Data();
    }
}