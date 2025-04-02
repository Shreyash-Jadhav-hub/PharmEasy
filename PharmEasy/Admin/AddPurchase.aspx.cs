using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Admin_AddPurchase : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack)
        {
            
            if (Request.QueryString["id"] != null)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = true;
                BindPurchaseData();
            }
            else
            {
                btnAdd.Visible = true;
                btnEdit.Visible = false;
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataTable dt = CreateDataTable();
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                gvProductDetails.DataSource = dt;
                gvProductDetails.DataBind();
            }
        }
    }
    [WebMethod]
    public static List<string> GetProductNames(string prefixText)
    {
        List<string> productNames = new List<string>();

        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT PROD_NM FROM tbl_ProdMaster WHERE PROD_NM LIKE @SearchText + '%'", conn))
            {
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        productNames.Add(sdr["PROD_NM"].ToString());
                    }
                }
                conn.Close();
            }
        }

        return productNames;
    }
    private void ShowAlert(string message)
    {
        string script = $"alert('{message}');";
        ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);

        lblMessage.Text = message;
        lblMessage.CssClass = "error-message";
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        try
        {
            // Add a new row to gvProductDetails
            AddNewRowToGrid();
        }
        catch (Exception ex)
        {
            string script = $"alert('Error adding product row: {ex.Message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
            lblMessage.Text = $"Error adding product row: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
    }
    protected void AddNewRowToGrid()
    {
        DataTable dt = CreateDataTable();

        // Loop through each existing row in the GridView and add it to the DataTable
        foreach (GridViewRow row in gvProductDetails.Rows)
        {
            DataRow dr = dt.NewRow();
            dr["ProductName"] = ((TextBox)row.FindControl("txtProductName")).Text;
            dr["BatchNo"] = ((TextBox)row.FindControl("txtBatchNo")).Text;
            dr["Expiry"] = ((TextBox)row.FindControl("txtExpiry")).Text;
            dr["MRP"] = ((TextBox)row.FindControl("txtMRP")).Text;
            dr["Rate"] = ((TextBox)row.FindControl("txtRate")).Text;
            dr["Qty"] = ((TextBox)row.FindControl("txtQty")).Text;
            dr["Free"] = ((TextBox)row.FindControl("txtFree")).Text;
            dr["SchDisc"] = ((TextBox)row.FindControl("txtSchDisc")).Text;
            dr["GST"] = ((TextBox)row.FindControl("txtGST")).Text;
            dr["Amount"] = ((TextBox)row.FindControl("txtAmount")).Text;
            dt.Rows.Add(dr);
        }

        // Add a new empty row to the DataTable
        DataRow newRow = dt.NewRow();
        dt.Rows.Add(newRow);

        // Bind the updated DataTable to the GridView
        gvProductDetails.DataSource = dt;
        gvProductDetails.DataBind();
    }
    private DataTable CreateDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("ProductName", typeof(string));
        dt.Columns.Add("BatchNo", typeof(string));
        dt.Columns.Add("Expiry", typeof(string));
        dt.Columns.Add("MRP", typeof(string));
        dt.Columns.Add("Rate", typeof(string));
        dt.Columns.Add("Qty", typeof(string));
        dt.Columns.Add("Free", typeof(string));
        dt.Columns.Add("SchDisc", typeof(string));
        dt.Columns.Add("GST", typeof(string));
        dt.Columns.Add("Amount", typeof(string));

        return dt;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string day = "01";
        string month = string.Empty;
        string year = string.Empty;
        string exp_date = string.Empty;
        DateTime Expiry_Date;

        try
        {
            if (!ValidateInput())
                return;

            // Retrieve input values
            string salesmanPassword = txtSalesmanPassword.Text.Trim();
            string supplierName = txtSupplierName.Text.Trim();
            string invoiceNo = txtInvoiceNo.Text.Trim();
            DateTime purchaseDate = DateTime.Parse(txtDate.Text.Trim());
            string paymentMode = ddlPaymentMode.SelectedValue;

            // Retrieve additional fields
            decimal partyDiscount = decimal.Parse(txtPartyDiscount.Text.Trim());
            decimal schDiscount = decimal.Parse(txtSchDiscount.Text.Trim());
            decimal totalDiscount = decimal.Parse(txtTotalDiscount.Text.Trim());
            decimal grossAmount = decimal.Parse(txtGrossAmt.Text.Trim());
            decimal totalGST = decimal.Parse(txtTotalGST.Text.Trim());
            decimal netAmount = decimal.Parse(txtNetAmt.Text.Trim());
            string description = txtNarration.Text.Trim(); // Use txtNarration as DESCRIPTION

            // Insert into database logic
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Insert into Purchase Master table (tbl_PurchaseMaster)
                string insertPurchaseMasterQuery = @"
            INSERT INTO [dbo].[tbl_PurchaseMaster] ([INVOICE_NO], [SUPPLIER_NM], [DATE], [PAYTYPE], [TTL_SCH_DISC], [PARTY_DISC], [TTL_DISC], [GROSS_AMT], [TTL_GST], [NET_AMT], [DESCRIPTION])
            VALUES (@InvoiceNo, @SupplierName, @PurchaseDate, @PaymentMode, @SchDiscount, @PartyDiscount, @TotalDiscount, @GrossAmt, @TotalGST, @NetAmt, @Description);
            SELECT SCOPE_IDENTITY();"; // Retrieve the newly inserted PUR_ID

                int purId;
                using (SqlCommand cmd = new SqlCommand(insertPurchaseMasterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                    cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                    cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                    cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
                    cmd.Parameters.AddWithValue("@SchDiscount", schDiscount);
                    cmd.Parameters.AddWithValue("@PartyDiscount", partyDiscount);
                    cmd.Parameters.AddWithValue("@TotalDiscount", totalDiscount);
                    cmd.Parameters.AddWithValue("@GrossAmt", grossAmount);
                    cmd.Parameters.AddWithValue("@TotalGST", totalGST);
                    cmd.Parameters.AddWithValue("@NetAmt", netAmount);
                    cmd.Parameters.AddWithValue("@Description", description); // Use txtNarration for DESCRIPTION

                    purId = Convert.ToInt32(cmd.ExecuteScalar()); // Get the PUR_ID
                }

                // Loop through each row in the GridView and insert into Purchase Detail table (tbl_PurchaseDetail)
                foreach (GridViewRow row in gvProductDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string productName = ((TextBox)row.FindControl("txtProductName")).Text.Trim();
                        int qty = int.Parse(((TextBox)row.FindControl("txtQty")).Text.Trim());
                        decimal mrp = decimal.Parse(((TextBox)row.FindControl("txtMRP")).Text.Trim());
                        decimal rate = decimal.Parse(((TextBox)row.FindControl("txtRate")).Text.Trim());
                        int free = int.Parse(((TextBox)row.FindControl("txtFree")).Text.Trim());
                        decimal schDisc = decimal.Parse(((TextBox)row.FindControl("txtSchDisc")).Text.Trim());
                        decimal gst = decimal.Parse(((TextBox)row.FindControl("txtGST")).Text.Trim());
                        string expiry = ((TextBox)row.FindControl("txtExpiry")).Text.Trim();
                        string batchNo = ((TextBox)row.FindControl("txtBatchNo")).Text.Trim(); // New Batch No field

                        day = "01";
                        month = expiry.Substring(0, 2);
                        year = expiry.Substring(expiry.Length - 2);
                        exp_date = day + "/" + month + "/" + year;
                        Expiry_Date = Convert.ToDateTime(exp_date);

                        // Validate product existence
                        int prodId = GetProductId(productName);
                        if (prodId == -1)
                        {
                            string errorMessage = "Product does not exist in the database.";
                            string script = $"alert('{errorMessage}');";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", script, true);
                            return;
                        }

                        // Insert into Purchase Detail table
                        string insertPurchaseDetailQuery = @"
                    INSERT INTO [dbo].[tbl_PurchaseDetail] ([PUR_ID], [PROD_ID], [QTY], [MRP], [RATE], [FREE], [SCH_DISC], [TAX_ID], [EXPIRY], [AMOUNT], [BATCH_NO])
                    VALUES (@PurId, @ProdId, @Qty, @Mrp, @Rate, @Free, @SchDisc, @TaxId, @Expiry, @Amount, @BatchNo)"; // Include Batch No

                        using (SqlCommand detailCmd = new SqlCommand(insertPurchaseDetailQuery, conn))
                        {
                            detailCmd.Parameters.AddWithValue("@PurId", purId);
                            detailCmd.Parameters.AddWithValue("@ProdId", prodId);
                            detailCmd.Parameters.AddWithValue("@Qty", qty);
                            detailCmd.Parameters.AddWithValue("@Mrp", mrp);
                            detailCmd.Parameters.AddWithValue("@Rate", rate);
                            detailCmd.Parameters.AddWithValue("@Free", free);
                            detailCmd.Parameters.AddWithValue("@SchDisc", schDisc);

                            // Example: Retrieve Tax ID based on GST
                            int taxId = GetTaxIdByGST(gst);
                            detailCmd.Parameters.AddWithValue("@TaxId", taxId);

                            detailCmd.Parameters.AddWithValue("@Expiry", Expiry_Date);

                            // Calculate and set the amount
                            decimal amount = rate * qty;
                            detailCmd.Parameters.AddWithValue("@Amount", amount);

                            // Add Batch No parameter
                            detailCmd.Parameters.AddWithValue("@BatchNo", batchNo);

                            detailCmd.ExecuteNonQuery();
                        }
                    }
                }

                string successMessage = "Purchase added successfully!";
                string successScript = $"alert('{successMessage}');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", successScript, true);
                ClearFields();
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error: {ex.Message}";
            string script = $"alert('{errorMessage}');";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", script, true);
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateInput())
                return;

            // Retrieve input values
            string salesmanPassword = txtSalesmanPassword.Text.Trim();
            string supplierName = txtSupplierName.Text.Trim();
            string invoiceNo = txtInvoiceNo.Text.Trim();
            DateTime purchaseDate;
            if (!DateTime.TryParse(txtDate.Text.Trim(), out purchaseDate))
            {
                ShowAlert("Invalid Purchase Date.");
                return;
            }
            string paymentMode = ddlPaymentMode.SelectedValue;

            // Retrieve additional fields
            decimal partyDiscount, schDiscount, totalDiscount, grossAmount, totalGST, netAmount;
            if (!Decimal.TryParse(txtPartyDiscount.Text.Trim(), out partyDiscount) ||
                !Decimal.TryParse(txtSchDiscount.Text.Trim(), out schDiscount) ||
                !Decimal.TryParse(txtTotalDiscount.Text.Trim(), out totalDiscount) ||
                !Decimal.TryParse(txtGrossAmt.Text.Trim(), out grossAmount) ||
                !Decimal.TryParse(txtTotalGST.Text.Trim(), out totalGST) ||
                !Decimal.TryParse(txtNetAmt.Text.Trim(), out netAmount))
            {
                ShowAlert("Invalid numeric values.");
                return;
            }

            // Retrieve the PUR_ID using the invoice number
            int purId = GetPurchaseIdByInvoiceNo(invoiceNo);
            if (purId == -1)
            {
                ShowAlert("Purchase not found.");
                return;
            }

            // Update the Purchase Master table (tbl_PurchaseMaster)
            string updatePurchaseMasterQuery = @"
        UPDATE [dbo].[tbl_PurchaseMaster]
        SET [SUPPLIER_NM] = @SupplierName, [DATE] = @PurchaseDate, [PAYTYPE] = @PaymentMode, 
            [TTL_SCH_DISC] = @SchDiscount, [PARTY_DISC] = @PartyDiscount, [TTL_DISC] = @TotalDiscount, 
            [GROSS_AMT] = @GrossAmt, [TTL_GST] = @TotalGST, [NET_AMT] = @NetAmt, [DESCRIPTION] = @Description
        WHERE [PUR_ID] = @PurId";

            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Update Purchase Master
                using (SqlCommand cmd = new SqlCommand(updatePurchaseMasterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@PurId", purId);
                    cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                    cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                    cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
                    cmd.Parameters.AddWithValue("@SchDiscount", schDiscount);
                    cmd.Parameters.AddWithValue("@PartyDiscount", partyDiscount);
                    cmd.Parameters.AddWithValue("@TotalDiscount", totalDiscount);
                    cmd.Parameters.AddWithValue("@GrossAmt", grossAmount);
                    cmd.Parameters.AddWithValue("@TotalGST", totalGST);
                    cmd.Parameters.AddWithValue("@NetAmt", netAmount);
                    cmd.Parameters.AddWithValue("@Description", txtNarration.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                // Process Purchase Details
                foreach (GridViewRow row in gvProductDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Retrieve data from row controls
                        string productName = ((TextBox)row.FindControl("txtProductName")).Text.Trim();
                        string batchNo = ((TextBox)row.FindControl("txtBatchNo")).Text.Trim();
                        string expiry = ((TextBox)row.FindControl("txtExpiry")).Text.Trim();
                        decimal mrp, rate, schDisc, gst, amount;
                        int qty, free;

                        // Convert expiry to DateTime
                        DateTime expiryDate;
                        if (!DateTime.TryParseExact(expiry, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out expiryDate))
                        {
                            ShowAlert("Invalid expiry date format. Use MM/yy.");
                            return;
                        }

                        if (!Decimal.TryParse(((TextBox)row.FindControl("txtMRP")).Text.Trim(), out mrp) ||
                            !Decimal.TryParse(((TextBox)row.FindControl("txtRate")).Text.Trim(), out rate) ||
                            !Int32.TryParse(((TextBox)row.FindControl("txtQty")).Text.Trim(), out qty) ||
                            !Int32.TryParse(((TextBox)row.FindControl("txtFree")).Text.Trim(), out free) ||
                            !Decimal.TryParse(((TextBox)row.FindControl("txtSchDisc")).Text.Trim(), out schDisc) ||
                            !Decimal.TryParse(((TextBox)row.FindControl("txtGST")).Text.Trim(), out gst) ||
                            !Decimal.TryParse(((TextBox)row.FindControl("txtAmount")).Text.Trim(), out amount))
                        {
                            ShowAlert("Invalid data in product details.");
                            return;
                        }

                        // Validate product existence
                        int prodId = GetProductId(productName);
                        if (prodId == -1)
                        {
                            ShowAlert("Product does not exist in the database.");
                            return;
                         
                        }

                        // Check if the purchase detail already exists
                        bool detailExists = PurchaseDetailExists(purId, prodId);
                        if (detailExists)
                        {
                            // Update existing detail
                            string updatePurchaseDetailQuery = @"
                        UPDATE [dbo].[tbl_PurchaseDetail]
                        SET [BATCH_NO] = @BatchNo, [EXPIRY] = @Expiry, [MRP] = @Mrp, [RATE] = @Rate, [QTY] = @Qty,
                            [FREE] = @Free, [SCH_DISC] = @SchDisc, [TAX_ID] = @TaxId, [AMOUNT] = @Amount
                        WHERE [PUR_ID] = @PurId AND [PROD_ID] = @ProdId";

                            using (SqlCommand detailCmd = new SqlCommand(updatePurchaseDetailQuery, conn))
                            {
                                detailCmd.Parameters.AddWithValue("@PurId", purId);
                                detailCmd.Parameters.AddWithValue("@ProdId", prodId);
                                detailCmd.Parameters.AddWithValue("@BatchNo", batchNo);
                                detailCmd.Parameters.AddWithValue("@Expiry", expiryDate);
                                detailCmd.Parameters.AddWithValue("@Mrp", mrp);
                                detailCmd.Parameters.AddWithValue("@Rate", rate);
                                detailCmd.Parameters.AddWithValue("@Qty", qty);
                                detailCmd.Parameters.AddWithValue("@Free", free);
                                detailCmd.Parameters.AddWithValue("@SchDisc", schDisc);

                                // Retrieve Tax ID based on GST
                                int taxId = GetTaxIdByGST(gst);
                                detailCmd.Parameters.AddWithValue("@TaxId", taxId);

                                // Calculate and set the amount
                                decimal totalAmount = rate * qty; // Renamed 'amount' to 'totalAmount'
                                detailCmd.Parameters.AddWithValue("@Amount", totalAmount);

                                detailCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Insert new detail
                            string insertPurchaseDetailQuery = @"
                        INSERT INTO [dbo].[tbl_PurchaseDetail] ([PUR_ID], [PROD_ID], [BATCH_NO], [EXPIRY], [MRP], [RATE], [QTY], [FREE], [SCH_DISC], [TAX_ID], [AMOUNT])
                        VALUES (@PurId, @ProdId, @BatchNo, @Expiry, @Mrp, @Rate, @Qty, @Free, @SchDisc, @TaxId, @Amount)";

                            using (SqlCommand detailCmd = new SqlCommand(insertPurchaseDetailQuery, conn))
                            {
                                detailCmd.Parameters.AddWithValue("@PurId", purId);
                                detailCmd.Parameters.AddWithValue("@ProdId", prodId);
                                detailCmd.Parameters.AddWithValue("@BatchNo", batchNo);
                                detailCmd.Parameters.AddWithValue("@Expiry", expiryDate);
                                detailCmd.Parameters.AddWithValue("@Mrp", mrp);
                                detailCmd.Parameters.AddWithValue("@Rate", rate);
                                detailCmd.Parameters.AddWithValue("@Qty", qty);
                                detailCmd.Parameters.AddWithValue("@Free", free);
                                detailCmd.Parameters.AddWithValue("@SchDisc", schDisc);

                                // Retrieve Tax ID based on GST
                                int taxId = GetTaxIdByGST(gst);
                                detailCmd.Parameters.AddWithValue("@TaxId", taxId);

                                detailCmd.Parameters.AddWithValue("@Amount", amount);

                                detailCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                ShowAlert("Purchase updated successfully!");
                ClearFields();
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Error: {ex.Message}");
        }
    }
    private int GetPurchaseIdByInvoiceNo(string invoiceNo)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT [PUR_ID] FROM [dbo].[tbl_PurchaseMaster] WHERE [INVOICE_NO] = @InvoiceNo";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return -1;
                }
            }
        }
    }
    private bool PurchaseDetailExists(int purId, int prodId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [dbo].[tbl_PurchaseDetail] WHERE [PUR_ID] = @PurId AND [PROD_ID] = @ProdId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PurId", purId);
                cmd.Parameters.AddWithValue("@ProdId", prodId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
    private int GetTaxIdByGST(decimal gst)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT [TAX_ID] FROM [dbo].[tbl_TaxMaster] WHERE [PERCENTAGE] = @GST";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@GST", gst);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return -1;
                }
            }
        }
    }
    protected void BindPurchaseData()
    {
        try
        {
            int purId = Convert.ToInt32(Request.QueryString["id"]);
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Fetch data from Purchase Master table
                SqlCommand cmdMaster = new SqlCommand("SELECT [SUPPLIER_NM], [INVOICE_NO], [DATE], [PAYTYPE], [DESCRIPTION] FROM [dbo].[tbl_PurchaseMaster] WHERE [PUR_ID] = @PurId", conn);
                cmdMaster.Parameters.AddWithValue("@PurId", purId);
                SqlDataAdapter daMaster = new SqlDataAdapter(cmdMaster);
                DataTable dtMaster = new DataTable();
                daMaster.Fill(dtMaster);

                if (dtMaster.Rows.Count > 0)
                {
                    txtSupplierName.Text = dtMaster.Rows[0]["SUPPLIER_NM"].ToString();
                    txtInvoiceNo.Text = dtMaster.Rows[0]["INVOICE_NO"].ToString();
                    txtDate.Text = Convert.ToDateTime(dtMaster.Rows[0]["DATE"]).ToString("yyyy-MM-dd");
                    ddlPaymentMode.SelectedValue = dtMaster.Rows[0]["PAYTYPE"].ToString();
                    txtNarration.Text = dtMaster.Rows[0]["DESCRIPTION"].ToString();
                }

                // Fetch data from Purchase Detail table, ensure columns are correct
                SqlCommand cmdDetail = new SqlCommand("SELECT" +
                    "(SELECT [PROD_NM] FROM [dbo].[tbl_ProdMaster] WHERE [PROD_ID]=PD.[PROD_ID]) AS ProductName," +
                    " PD.[BATCH_NO] AS BatchNo, FORMAT(PD.[EXPIRY],'MM/yy') AS EXPIRY, PD.[MRP], PD.[RATE], PD.[QTY], PD.[FREE], PD.[SCH_DISC] AS SchDisc, " +
                    "(SELECT [PERCENTAGE] FROM [dbo].[tbl_TaxMaster] WHERE [TAX_ID]=PD.[TAX_ID]) AS GST, " +
                    "PD.[AMOUNT] " +
                    "FROM [dbo].[tbl_PurchaseDetail] AS PD " +
                    "WHERE PD.[PUR_ID]=@PURID", conn);
                cmdDetail.Parameters.AddWithValue("@PURID", purId);
                SqlDataAdapter daDetail = new SqlDataAdapter(cmdDetail);
                DataTable dtDetail = new DataTable();
                daDetail.Fill(dtDetail);

                gvProductDetails.DataSource = dtDetail;
                gvProductDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Error: {ex.Message}");
            lblMessage.Text = $"Error: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
    }
    protected void BindGridViewPurchaseDetails()
    {
        try
        {
            int purId = Convert.ToInt32(Request.QueryString["id"]); // Assuming PUR_ID is passed via query string
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query to fetch purchase details and associated product information
                string query = "SELECT" +
                    "(SELECT [PROD_NM] FROM [dbo].[tbl_ProdMaster] WHERE [PROD_ID]=PD.[PROD_ID]) AS ProductName," +
                    " PD.[BATCH_NO] AS BatchNo, FORMAT(PD.[EXPIRY],'MM/yy') AS EXPIRY, PD.[MRP], PD.[RATE], PD.[QTY], PD.[FREE], PD.[SCH_DISC] AS SchDisc, " +
                    "(SELECT [PERCENTAGE] FROM [dbo].[tbl_TaxMaster] WHERE [TAX_ID]=PD.[TAX_ID]) AS GST, " +
                    "PD.[AMOUNT] " +
                    "FROM [dbo].[tbl_PurchaseDetail] AS PD " +
                    "WHERE PD.[PUR_ID]=@PURID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PurId", purId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the data to the GridView
                gvProductDetails.DataSource = dt;
                gvProductDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Error: {ex.Message}");
            lblMessage.Text = $"Error: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
    }
    private decimal GetGstByTaxId(int taxId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT [PERCENTAGE] FROM [dbo].[tbl_TaxMaster] WHERE [TAX_ID] = @TaxId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TaxId", taxId);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToDecimal(result);
                }
                else
                {
                    return 0; // Default value in case of missing GST data
                }
            }
        }
    }
    private bool ValidateInput()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        if (string.IsNullOrWhiteSpace(txtSalesmanPassword.Text))
        {
            ShowAlert("Salesman password is required.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
        {
            ShowAlert("Supplier name is required.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
        {
            ShowAlert("Invoice number is required.");
            return false;
        }

        if (!DateTime.TryParse(txtDate.Text, out DateTime purchaseDate))
        {
            ShowAlert("Invalid date.");
            return false;
        }

        foreach (GridViewRow row in gvProductDetails.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                string productName = ((TextBox)row.FindControl("txtProductName")).Text.Trim();
                if (string.IsNullOrWhiteSpace(productName))
                {
                    ShowAlert("Product name is required.");
                    return false;
                }

                if (!int.TryParse(((TextBox)row.FindControl("txtQty")).Text.Trim(), out int qty) || qty <= 0)
                {
                    ShowAlert("Invalid quantity.");
                    return false;
                }

                if (!decimal.TryParse(((TextBox)row.FindControl("txtMRP")).Text.Trim(), out decimal mrp) || mrp <= 0)
                {
                    ShowAlert("Invalid MRP.");
                    return false;
                }

                if (!decimal.TryParse(((TextBox)row.FindControl("txtRate")).Text.Trim(), out decimal rate) || rate <= 0)
                {
                    ShowAlert("Invalid rate.");
                    return false;
                }
            }
        }

        string salesmanPassword = txtSalesmanPassword.Text.Trim();
        string query = "SELECT COUNT(*) FROM [dbo].[tbl_Users] WHERE [PASSWORD] = @Password";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", salesmanPassword);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        ShowAlert("Invalid salesman password.");
                        return false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Error validating input: {ex.Message}");
            return false;
        }

        return true;
    }
    protected int GetProductId(string productName)
    {
        int prodId = -1; // Default value if product is not found

        string query = "SELECT [PROD_ID] FROM [dbo].[tbl_ProdMaster] WHERE [PROD_NM] = @ProductName AND [IS_ACTIVE] = 1;";

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProductName", productName);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    prodId = Convert.ToInt32(result);
                }
            }
        }

        return prodId;
    }
   protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        double amt = 0;
        try
        {
            foreach (GridViewRow grdrow in gvProductDetails.Rows)
            {
                TextBox txtrate = (TextBox)grdrow.Cells[4].FindControl("txtRate");
                TextBox txtqty = (TextBox)grdrow.Cells[5].FindControl("txtqty");
                TextBox txtAmount = (TextBox)grdrow.Cells[9].FindControl("txtAmount");
                TextBox txtFree = (TextBox)grdrow.Cells[6].FindControl("txtFree");

                if (txtrate.Text != string.Empty && txtqty.Text != string.Empty)
                {
                    amt = Convert.ToDouble(txtrate.Text) * Convert.ToDouble(txtqty.Text);
                    txtAmount.Text = amt.ToString(); txtFree.Focus();
                }
                else
                {
                    string script = "alert('Enter Rate & Quantity of Product.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
                    lblMessage.Text = "Enter Rate & Quantity of Product.";
                    lblMessage.CssClass = "error-message";
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = $"Error adding product row: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
    }
    protected void txtPartyDiscount_TextChanged(object sender, EventArgs e)
    {
        CalculateTotals();
    }
    private void CalculateTotals()
    {
        double partyDiscount = 0;
        double grossAmount = 0;
        double totalSchDiscount = 0;
        double totalGST = 0;

        if (double.TryParse(txtPartyDiscount.Text, out double parsedPartyDiscount))
        {
            partyDiscount = parsedPartyDiscount;
        }

        foreach (GridViewRow grdrow in gvProductDetails.Rows)
        {
            TextBox txtrate = (TextBox)grdrow.Cells[4].FindControl("txtRate");
            TextBox txtqty = (TextBox)grdrow.Cells[5].FindControl("txtqty");
            TextBox txtgst = (TextBox)grdrow.Cells[7].FindControl("txtGST");
            TextBox txtSchDisc = (TextBox)grdrow.Cells[8].FindControl("txtSchDisc");

            if (double.TryParse(txtrate.Text, out double rate) &&
                double.TryParse(txtqty.Text, out double quantity) &&
                double.TryParse(txtgst.Text, out double gst) &&
                double.TryParse(txtSchDisc.Text, out double schDiscount))
            {
                double amount = rate * quantity;
                grossAmount += amount;
                totalSchDiscount += schDiscount;
                totalGST += (amount * gst / 100);
            }
        }

        double totalDiscount = totalSchDiscount + partyDiscount;
        txtGrossAmt.Text = grossAmount.ToString("F2");
        txtSchDiscount.Text = totalSchDiscount.ToString("F2");
        txtTotalDiscount.Text = totalDiscount.ToString("F2");
        txtTotalGST.Text = totalGST.ToString("F2");

        double netAmount = (grossAmount + totalGST) - totalDiscount;
        txtNetAmt.Text = netAmount.ToString("F2");
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtProductName = sender as TextBox;
            GridViewRow row = txtProductName.NamingContainer as GridViewRow;
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Retrieve product details along with TAX_ID
                string query = @"
                SELECT 
                    COUNT([PROD_NM]) AS ProductCount, 
                    [TAX_ID] 
                FROM 
                    [dbo].[tbl_ProdMaster] 
                WHERE 
                    [PROD_NM] = @PRD_NAME
                GROUP BY 
                    [TAX_ID]";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PRD_NAME", txtProductName.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int productCount = Convert.ToInt32(reader["ProductCount"]);
                        if (productCount > 0)
                        {
                            int taxId = Convert.ToInt32(reader["TAX_ID"]);
                            decimal gstPercentage = GetGstByTaxId(taxId);

                            // Assuming you have a TextBox to display GST percentage in the GridView
                            TextBox txtGst = row.FindControl("txtGst") as TextBox;
                            if (txtGst != null)
                            {
                                txtGst.Text = gstPercentage.ToString();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Product details not found.";
                            lblMessage.CssClass = "error-message";
                            string script = "if(confirm('Product not found. Do you want to create a new product?')) { window.location='AddProd.aspx'; }";
                            ClientScript.RegisterStartupScript(this.GetType(), "redirectScript", script, true);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Product details not found.";
                        lblMessage.CssClass = "error-message";
                        string script = "if(confirm('Product not found. Do you want to create a new product?')) { window.location='AddProd.aspx'; }";
                        ClientScript.RegisterStartupScript(this.GetType(), "redirectScript", script, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = $"Error: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
    }
    private ProductDetails GetProductDetailsByName(string productName)
    {
        ProductDetails productDetails = null;
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        string query = @"
        SELECT TOP 1
            PD.QTY,
            PD.MRP,
            PD.RATE,
            PD.FREE,
            PD.SCH_DISC,
            TM.PERCENTAGE AS GST,
            PD.EXPIRY,
            PD.AMOUNT
        FROM
            [dbo].[tbl_PurchaseDetail] PD
        LEFT JOIN
            [dbo].[tbl_TaxMaster] TM ON PD.TAX_ID = TM.TAX_ID
        WHERE
            PD.PROD_ID = (SELECT PROD_ID FROM [dbo].[tbl_ProdMaster] WHERE PROD_NM = @ProductName)";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            productDetails = new ProductDetails
                            {
                                Quantity = reader.IsDBNull(reader.GetOrdinal("QTY")) ? 0 : reader.GetInt32(reader.GetOrdinal("QTY")),
                                MRP = reader.IsDBNull(reader.GetOrdinal("MRP")) ? 0 : (decimal)reader.GetFloat(reader.GetOrdinal("MRP")),
                                Rate = reader.IsDBNull(reader.GetOrdinal("RATE")) ? 0 : (decimal)reader.GetFloat(reader.GetOrdinal("RATE")),
                                Free = reader.IsDBNull(reader.GetOrdinal("FREE")) ? 0 : reader.GetInt32(reader.GetOrdinal("FREE")),
                                SchDiscount = reader.IsDBNull(reader.GetOrdinal("SCH_DISC")) ? 0 : (decimal)reader.GetFloat(reader.GetOrdinal("SCH_DISC")),
                                GST = reader.IsDBNull(reader.GetOrdinal("GST")) ? 0 : (decimal)reader.GetFloat(reader.GetOrdinal("GST")),
                                ExpiryDate = reader.IsDBNull(reader.GetOrdinal("EXPIRY")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EXPIRY")),
                                Amount = reader.IsDBNull(reader.GetOrdinal("AMOUNT")) ? 0 : (decimal)reader.GetFloat(reader.GetOrdinal("AMOUNT"))
                            };
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            foreach (SqlError error in ex.Errors)
            {
                if (error.Message.Contains("Invalid column name 'ProductName'"))
                {
                    throw new Exception("Invalid product name entered.");
                }
            }

            throw;
        }

        return productDetails;
    }
    private class ProductDetails
    {
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Rate { get; set; }
        public int Free { get; set; }
        public decimal SchDiscount { get; set; }
        public decimal GST { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Amount { get; set; }
    }
    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        string invoiceNo = txtInvoiceNo.Text.Trim();

        if (string.IsNullOrEmpty(invoiceNo))
        {
            lblMessage.Text = "Invoice number cannot be empty.";
            lblMessage.CssClass = "error-message";
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string checkInvoiceQuery = "SELECT COUNT(*) FROM [dbo].[tbl_PurchaseMaster] WHERE [INVOICE_NO] = @InvoiceNo";
            using (SqlCommand cmd = new SqlCommand(checkInvoiceQuery, conn))
            {
                cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    string script = "alert('Invoice number already exists. Please enter a unique invoice number.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);

                    //lblMessage.Text = "Invoice number already exists. Please enter a unique invoice number.";
                    //lblMessage.CssClass = "error-message";
                    txtInvoiceNo.Text = string.Empty;
                    txtInvoiceNo.Focus();
                }
                else
                {
                    lblMessage.Text = string.Empty;
                }

            }
        }
    }
    protected void ClearFields()
    {
        // Clear input fields after successful submission
        txtSalesmanPassword.Text = "";
        txtSupplierName.Text = "";
        txtInvoiceNo.Text = "";
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        ddlPaymentMode.SelectedIndex = 0;

        // Clear totals and discounts fields
        txtPartyDiscount.Text = "";
        txtSchDiscount.Text = "";
        txtTotalDiscount.Text = "";
        txtGrossAmt.Text = "";
        txtTotalGST.Text = "";
        txtNetAmt.Text = "";

        // Clear Product Details fields
        ClearProductFields();

        // Clear DESCRIPTION field (txtNarration)
        txtNarration.Text = "";
    }
    protected void ClearProductFields()
    {
        foreach (GridViewRow row in gvProductDetails.Rows)
        {
            TextBox txtProductName = (TextBox)row.FindControl("txtProductName");
            TextBox txtQty = (TextBox)row.FindControl("txtQty");
            TextBox txtBatchNo = (TextBox)row.FindControl("txtBatchNo");
            TextBox txtExpiry = (TextBox)row.FindControl("txtExpiry");
            TextBox txtMRP = (TextBox)row.FindControl("txtMRP");
            TextBox txtRate = (TextBox)row.FindControl("txtRate");
            TextBox txtFree = (TextBox)row.FindControl("txtFree");
            TextBox txtSchDisc = (TextBox)row.FindControl("txtSchDisc");
            TextBox txtGST = (TextBox)row.FindControl("txtGST");
            TextBox txtAmount = (TextBox)row.FindControl("txtAmount");

            if (txtProductName != null) txtProductName.Text = "";
            if (txtQty != null) txtQty.Text = "";
            if (txtBatchNo != null) txtBatchNo.Text = "";
            if (txtExpiry != null) txtExpiry.Text = "";
            if (txtMRP != null) txtMRP.Text = "";
            if (txtRate != null) txtRate.Text = "";
            if (txtFree != null) txtFree.Text = "";
            if (txtSchDisc != null) txtSchDisc.Text = "";
            if (txtGST != null) txtGST.Text = "";
            if (txtAmount != null) txtAmount.Text = "";
        }
    }
}