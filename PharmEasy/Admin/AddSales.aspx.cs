using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddSales : System.Web.UI.Page
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
                // Editing an existing sales record
                btnAdd.Visible = false;
                btnEdit.Visible = true;

                // Retrieve the sales ID from the query string
                int salesId;
                if (int.TryParse(Request.QueryString["id"], out salesId))
                {
                    BindProductDetails(salesId);
                    BindSalesData(salesId);
                }
                else
                {
                    // Handle invalid salesId here (e.g., show an error message or redirect)
                    lblMessage.Text = "Invalid sales record.";
                    lblMessage.CssClass = "error-message";
                }
            }
            else
            {
                // Adding a new sales record
                btnAdd.Visible = true;
                btnEdit.Visible = false;

                // Set the default date
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Initialize the GridView with an empty row
                DataTable dt = CreateDataTable();
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                gvProductDetails.DataSource = dt;
                gvProductDetails.DataBind();

                // Generate the bill number
                GenerateBillNumber();
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

            AddNewRowToGrid();
        }
        catch (Exception ex)
        {
            ShowAlert($"Error adding product row: {ex.Message}");
        }
    }
    protected void AddNewRowToGrid()
    {
        DataTable dt = CreateDataTable();

        foreach (GridViewRow row in gvProductDetails.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                DataRow dr = dt.NewRow();
                dr["ProductName"] = ((TextBox)row.FindControl("txtProductName")).Text;
                dr["Unit"] = ((TextBox)row.FindControl("txtUnit")).Text;
                dr["BatchNo"] = ((TextBox)row.FindControl("txtBatchNo")).Text;
                dr["Expiry"] = ((TextBox)row.FindControl("txtExpiry")).Text; // Ensure this field exists and is correctly referenced
                dr["MRP"] = decimal.Parse(((TextBox)row.FindControl("txtMRP")).Text); // Ensure data types match
                dr["Qty"] = int.Parse(((TextBox)row.FindControl("txtQty")).Text); // Ensure data types match
                dr["GST"] = decimal.Parse(((TextBox)row.FindControl("txtGST")).Text); // Ensure data types match
                dr["Amount"] = decimal.Parse(((TextBox)row.FindControl("txtAmount")).Text); // Ensure data types match
                dt.Rows.Add(dr);
            }
        }

        // Add a new empty row
        DataRow newRow = dt.NewRow();
        dt.Rows.Add(newRow);

        // Bind the updated DataTable to GridView
        gvProductDetails.DataSource = dt;
        gvProductDetails.DataBind();
    }

    private DataTable CreateDataTable()
    {
        DataTable dt = new DataTable();

        // Define columns
        dt.Columns.Add("ProductName", typeof(string));
        dt.Columns.Add("Unit", typeof(string));
        dt.Columns.Add("BatchNo", typeof(string));
        dt.Columns.Add("Expiry", typeof(string)); // Ensure this column is included
        dt.Columns.Add("MRP", typeof(decimal));
        dt.Columns.Add("Qty", typeof(int));
        dt.Columns.Add("GST", typeof(decimal));
        dt.Columns.Add("Amount", typeof(decimal));

        return dt;
    }

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        double amt = 0;
        try
        {
            foreach (GridViewRow grdrow in gvProductDetails.Rows)
            {
                // Get controls from the current row
                TextBox txtMRP = (TextBox)grdrow.FindControl("txtMRP");
                TextBox txtQty = (TextBox)grdrow.FindControl("txtQty");
                TextBox txtAmount = (TextBox)grdrow.FindControl("txtAmount");

                // Ensure that MRP and Qty are not empty before calculation
                if (!string.IsNullOrEmpty(txtMRP.Text) && !string.IsNullOrEmpty(txtQty.Text))
                {
                    // Perform the calculation for Amount
                    amt = Convert.ToDouble(txtMRP.Text) * Convert.ToDouble(txtQty.Text);
                    txtAmount.Text = amt.ToString("F2"); // Format to 2 decimal places
                }
                else
                {
                    // Show alert if MRP or Qty is empty
                    string script = "alert('Enter MRP & Quantity for the product.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
                    lblMessage.Text = "Enter MRP & Quantity for the product.";
                    lblMessage.CssClass = "error-message";
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during processing
            lblMessage.Text = $"Error calculating amount: {ex.Message}";
            lblMessage.CssClass = "error-message";
        }
        CalculateTotals();
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

                // Query to retrieve product details from tbl_ProdMaster and tbl_ProdDetail
                string query = @"
            SELECT 
                PM.UNIT, 
                PD.BATCH_NO, 
                PD.EXPIRY, 
                PD.MRP, 
                TM.PERCENTAGE AS GST
            FROM 
                [dbo].[tbl_ProdMaster] PM
            INNER JOIN 
                [dbo].[tbl_ProdDetail] PD ON PM.PROD_ID = PD.PROD_ID
            INNER JOIN 
                [dbo].[tbl_TaxMaster] TM ON PM.TAX_ID = TM.TAX_ID
            WHERE 
                PM.PROD_NM = @PRD_NAME";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PRD_NAME", txtProductName.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Retrieve and populate product details
                        string unit = reader["UNIT"].ToString();
                        string batchNo = reader["BATCH_NO"].ToString();
                        DateTime expiry = Convert.ToDateTime(reader["EXPIRY"]);
                        double mrp = Convert.ToDouble(reader["MRP"]);
                        decimal gstPercentage = Convert.ToDecimal(reader["GST"]);

                        // Assuming you have TextBoxes in the GridView to display these values
                        TextBox txtUnit = row.FindControl("txtUnit") as TextBox;
                        TextBox txtBatchNo = row.FindControl("txtBatchNo") as TextBox;
                        TextBox txtExpiry = row.FindControl("txtExpiry") as TextBox;
                        TextBox txtMRP = row.FindControl("txtMRP") as TextBox;
                        TextBox txtGst = row.FindControl("txtGst") as TextBox;

                        if (txtUnit != null) txtUnit.Text = unit;
                        if (txtBatchNo != null) txtBatchNo.Text = batchNo;
                        if (txtExpiry != null) txtExpiry.Text = expiry.ToString("MM/yyyy"); // Displaying only month/year
                        if (txtMRP != null) txtMRP.Text = mrp.ToString("F2"); // Formatting to 2 decimal places
                        if (txtGst != null) txtGst.Text = gstPercentage.ToString("F2"); // Formatting to 2 decimal places
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
    protected void txtSalesmanPassword_TextChanged(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        string password = txtSalesmanPassword.Text.Trim();

        if (!string.IsNullOrEmpty(password))
        {
            string query = "SELECT UID, NAME FROM [dbo].[tbl_Users] WHERE [PASSWORD] = @Password";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", password);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["UID"]);
                                string salesmanName = reader["NAME"].ToString();

                                // Store the User ID in a hidden field or a ViewState
                                ViewState["UserId"] = userId;

                                txtSalesmanName.Text = salesmanName;
                            }
                            else
                            {
                                txtSalesmanName.Text = string.Empty;
                                ShowAlert("Salesman not found. Please check the password.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error retrieving Salesman Name: {ex.Message}");
            }
        }
        else
        {
            txtSalesmanName.Text = string.Empty;
        }
    }
    private void CalculateTotals()
    {
        decimal grossAmount = 0;
        decimal totalGST = 0;
        decimal discountPercent = 0;
        decimal totalDiscount = 0;

        foreach (GridViewRow row in gvProductDetails.Rows)
        {
            // Retrieve values from GridView controls
            TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
            TextBox txtGST = (TextBox)row.FindControl("txtGST");

            if (decimal.TryParse(txtAmount.Text.Trim(), out decimal amount))
            {
                grossAmount += amount;
            }

            if (decimal.TryParse(txtGST.Text.Trim(), out decimal gstPercentage))
            {
                // Assuming GST is a percentage and not a decimal
                totalGST += (grossAmount * gstPercentage / 100);
            }
        }

        // Calculate total discount based on user input
        if (decimal.TryParse(txtDiscountPercent.Text.Trim(), out discountPercent))
        {
            totalDiscount = grossAmount * discountPercent / 100;
        }
        else if (decimal.TryParse(txtTotalDiscount.Text.Trim(), out totalDiscount))
        {
            // User entered discount directly
        }

        // Update the fields with calculated values
        txtGrossAmount.Text = grossAmount.ToString("F2");
        txtTotalGST.Text = totalGST.ToString("F2");
        txtTotalDiscount.Text = totalDiscount.ToString("F2");
        txtTotalAmount.Text = (grossAmount - totalDiscount).ToString("F2");
    }
    protected void txtDiscountPercent_TextChanged(object sender, EventArgs e)
    {
        CalculateTotals();
    }
    protected void txtTotalDiscount_TextChanged(object sender, EventArgs e)
    {
        CalculateTotals();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateInput())
                return;

            // Retrieve sales master input values
            DateTime salesDate = DateTime.Parse(txtDate.Text.Trim());
            decimal grossAmount = decimal.Parse(txtGrossAmount.Text.Trim());
            decimal discount = decimal.Parse(txtTotalDiscount.Text.Trim());
            decimal totalGST = decimal.Parse(txtTotalGST.Text.Trim());
            decimal totalAmount = decimal.Parse(txtTotalAmount.Text.Trim());
            string paymentMode = ddlPaymentMode.SelectedValue;
            string description = txtNarration.Text.Trim();

            // Retrieve patient details
            string patientName = txtPatientName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string mobile = txtPhoneNo.Text.Trim();
            string doctorName = txtDrName.Text.Trim();
            string city = txtCity.Text.Trim();

            // Insert into tbl_Patients if patient does not exist
            int patientId = GetPatientId(patientName, mobile);
            if (patientId == -1)
            {
                // Insert new patient
                string insertPatientQuery = @"
            INSERT INTO [dbo].[tbl_Patients] ([PATIENT_NAME], [ADDRESS], [MOBILE], [DR_NAME], [CITY])
            VALUES (@PatientName, @Address, @Mobile, @DoctorName, @City);
            SELECT SCOPE_IDENTITY();";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertPatientQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PatientName", patientName);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                        cmd.Parameters.AddWithValue("@City", city);
                        patientId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }

            // Insert into Sales Master table
            string insertSalesMasterQuery = @"
        INSERT INTO [dbo].[tbl_SalesMaster] ([DATE], [GROSS_AMT], [DISC_%], [TTL_GST], [TTL_AMT], [PAYTYPE], [DESCRIPTION], [UID], [PATIENT_ID], [DISC_AMT])
        VALUES (@Date, @GrossAmount, @Discount, @TotalGST, @TotalAmount, @PaymentMode, @Description, @UserId, @PatientId, @Discount);
        SELECT SCOPE_IDENTITY();";

            int salesId;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(insertSalesMasterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", salesDate);
                    cmd.Parameters.AddWithValue("@GrossAmount", grossAmount);
                    cmd.Parameters.AddWithValue("@Discount", discount);
                    cmd.Parameters.AddWithValue("@TotalGST", totalGST);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(Session["userid"]));

                    salesId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Loop through each row in the GridView and insert into Sales Detail table
                foreach (GridViewRow row in gvProductDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string productName = ((TextBox)row.FindControl("txtProductName")).Text.Trim();
                        int qty = int.Parse(((TextBox)row.FindControl("txtQty")).Text.Trim());
                        decimal amount = decimal.Parse(((TextBox)row.FindControl("txtAmount")).Text.Trim());

                        int productId = GetProductId(productName);
                        if (productId == -1)
                        {
                            // Show error if product does not exist
                            string errorMessage = "Product does not exist in the database.";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", $"alert('{errorMessage}');", true);
                            return;
                        }

                        // Insert into Sales Detail table
                        string insertSalesDetailQuery = @"
                    INSERT INTO [dbo].[tbl_SalesDetail] ([SALES_ID], [PROD_ID], [QTY], [AMOUNT])
                    VALUES (@SalesId, @ProductId, @Qty, @Amount)";

                        using (SqlCommand detailCmd = new SqlCommand(insertSalesDetailQuery, conn))
                        {
                            detailCmd.Parameters.AddWithValue("@SalesId", salesId);
                            detailCmd.Parameters.AddWithValue("@ProductId", productId);
                            detailCmd.Parameters.AddWithValue("@Qty", qty);
                            detailCmd.Parameters.AddWithValue("@Amount", amount);
                            detailCmd.ExecuteNonQuery();
                        }
                    }
                }

                // Update the Bill Number in the TextBox
                txtBillNo.Text = salesId.ToString();

                // Show success message
                string successMessage = "Sales added successfully!";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", $"alert('{successMessage}');", true);
            }

            ClearFields();
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error: {ex.Message}";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", $"alert('{errorMessage}');", true);
        }
    }
    // Method to get or create patient
    private int GetPatientId(string patientName, string mobile)
    {
        int patientId = -1;

        string query = @"
        SELECT [PATIENT_ID] FROM [dbo].[tbl_Patients] 
        WHERE [PATIENT_NAME] = @PatientName AND [MOBILE] = @Mobile";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientName", patientName);
                cmd.Parameters.AddWithValue("@Mobile", mobile);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    patientId = Convert.ToInt32(result);
                }
            }
        }

        return patientId;
    }
    // Method to get product ID by name
    private int GetProductId(string productName)
    {
        int productId = -1;

        string query = @"
        SELECT [PROD_ID] FROM [dbo].[tbl_ProdMaster] 
        WHERE [PROD_NM] = @ProductName";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProductName", productName);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    productId = Convert.ToInt32(result);
                }
            }
        }

        return productId;
    }
    private bool ValidateInput()
    {
        DateTime date;
        decimal value;

        // Validate Date
        if (!DateTime.TryParse(txtDate.Text.Trim(), out date))
        {
            ShowAlert("Invalid date format.");
            return false;
        }

        // Validate Decimal values
        if (!decimal.TryParse(txtGrossAmount.Text.Trim(), out value) ||
            !decimal.TryParse(txtTotalDiscount.Text.Trim(), out value) ||
            !decimal.TryParse(txtTotalGST.Text.Trim(), out value) ||
            !decimal.TryParse(txtTotalAmount.Text.Trim(), out value))
        {
            ShowAlert("Invalid decimal value.");
            return false;
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(txtPatientName.Text.Trim()) ||
            string.IsNullOrWhiteSpace(txtPhoneNo.Text.Trim()) ||
            string.IsNullOrWhiteSpace(txtCity.Text.Trim()))
        {
            ShowAlert("Patient name, phone number, and city are required.");
            return false;
        }



        return true;
    }
    private void ClearFields()
    {
        // Reset form fields
        txtSalesmanPassword.Text = string.Empty;
        txtSalesmanName.Text = string.Empty;
        txtBillNo.Text = string.Empty;
        txtDiscountPercent.Text = string.Empty;
        txtTotalDiscount.Text = string.Empty;
        txtDate.Text = string.Empty;
        txtGrossAmount.Text = string.Empty;
        txtTotalDiscount.Text = string.Empty;
        txtTotalGST.Text = string.Empty;
        txtTotalAmount.Text = string.Empty;
        ddlPaymentMode.SelectedIndex = 0;
        txtNarration.Text = string.Empty;
        txtPatientName.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtDrName.Text = string.Empty;
        txtCity.Text = string.Empty;


        // Clear GridView rows if needed
        gvProductDetails.DataSource = null;
        gvProductDetails.DataBind();

        // Clear any messages displayed
        lblMessage.Text = string.Empty;
        lblMessage.CssClass = string.Empty;
    }
    private void GenerateBillNumber()
    {
        int salesId = 0;

        string query = "SELECT ISNULL(MAX([SALES_ID]), 0) FROM [dbo].[tbl_SalesMaster]";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    salesId = Convert.ToInt32(result);
                }
            }
        }

        // Display the generated Bill Number in the TextBox
        txtBillNo.Text = salesId.ToString();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateInput())
                return;

            // Retrieve sales master input values
            DateTime salesDate = DateTime.Parse(txtDate.Text.Trim());
            decimal grossAmount = decimal.Parse(txtGrossAmount.Text.Trim());
            decimal discount = decimal.Parse(txtTotalDiscount.Text.Trim());
            decimal totalGST = decimal.Parse(txtTotalGST.Text.Trim());
            decimal totalAmount = decimal.Parse(txtTotalAmount.Text.Trim());
            string paymentMode = ddlPaymentMode.SelectedValue;
            string description = txtNarration.Text.Trim();

            // Retrieve patient details
            string patientName = txtPatientName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string mobile = txtPhoneNo.Text.Trim();
            string doctorName = txtDrName.Text.Trim();
            string city = txtCity.Text.Trim();

            int salesId = GetSalesIdByBillNo(txtBillNo.Text.Trim());
            if (salesId == -1)
            {
                ShowAlert("Sales record not found.");
                return;
            }

            int patientId = GetPatientId(patientName, mobile);
            if (patientId == -1)
            {
                ShowAlert("Patient not found.");
                return;
            }

            string updateSalesMasterQuery = @"
            UPDATE [dbo].[tbl_SalesMaster]
            SET [DATE] = @Date, [GROSS_AMT] = @GrossAmount, [DISC_%] = @Discount, [TTL_GST] = @TotalGST, 
                [TTL_AMT] = @TotalAmount, [PAYTYPE] = @PaymentMode, [DESCRIPTION] = @Description, 
                [PATIENT_ID] = @PatientId, [DISC_AMT] = @Discount
            WHERE [SALES_ID] = @SalesId";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateSalesMasterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SalesId", salesId);
                    cmd.Parameters.AddWithValue("@Date", salesDate);
                    cmd.Parameters.AddWithValue("@GrossAmount", grossAmount);
                    cmd.Parameters.AddWithValue("@Discount", discount);
                    cmd.Parameters.AddWithValue("@TotalGST", totalGST);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.ExecuteNonQuery();
                }

                foreach (GridViewRow row in gvProductDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string productName = ((TextBox)row.FindControl("txtProductName")).Text.Trim();
                        int qty = int.Parse(((TextBox)row.FindControl("txtQty")).Text.Trim());
                        decimal amount = decimal.Parse(((TextBox)row.FindControl("txtAmount")).Text.Trim());

                        int productId = GetProductId(productName);
                        if (productId == -1)
                        {
                            ShowAlert("Product does not exist in the database.");
                            return;
                        }

                        if (SalesDetailExists(salesId, productId))
                        {
                            string updateSalesDetailQuery = @"
                            UPDATE [dbo].[tbl_SalesDetail]
                            SET [QTY] = @Qty, [AMOUNT] = @Amount
                            WHERE [SALES_ID] = @SalesId AND [PROD_ID] = @ProductId";

                            using (SqlCommand detailCmd = new SqlCommand(updateSalesDetailQuery, conn))
                            {
                                detailCmd.Parameters.AddWithValue("@SalesId", salesId);
                                detailCmd.Parameters.AddWithValue("@ProductId", productId);
                                detailCmd.Parameters.AddWithValue("@Qty", qty);
                                detailCmd.Parameters.AddWithValue("@Amount", amount);
                                detailCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string insertSalesDetailQuery = @"
                            INSERT INTO [dbo].[tbl_SalesDetail] ([SALES_ID], [PROD_ID], [QTY], [AMOUNT])
                            VALUES (@SalesId, @ProductId, @Qty, @Amount)";

                            using (SqlCommand detailCmd = new SqlCommand(insertSalesDetailQuery, conn))
                            {
                                detailCmd.Parameters.AddWithValue("@SalesId", salesId);
                                detailCmd.Parameters.AddWithValue("@ProductId", productId);
                                detailCmd.Parameters.AddWithValue("@Qty", qty);
                                detailCmd.Parameters.AddWithValue("@Amount", amount);
                                detailCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                ShowAlert("Sales updated successfully.");
            }

            ClearFields();
        }
        catch (Exception ex)
        {
            ShowAlert("Error: " + ex.Message);
        }
    }
    private int GetSalesIdByBillNo(string billNo)
    {
        try
        {
            string query = "SELECT SALES_ID FROM [dbo].[tbl_SalesMaster] WHERE BILL_NO = @BillNo";
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BillNo", billNo);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result); // Sales ID found
                    }
                    else
                    {
                        return -1; // Sales ID not found
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Error retrieving Sales ID: {ex.Message}");
            return -1;
        }
    }
    private bool SalesDetailExists(int salesId, int productId)
    {
        try
        {
            string query = @"
        SELECT COUNT(1) 
        FROM [dbo].[tbl_SalesDetail] 
        WHERE SALES_ID = @SalesId AND PROD_ID = @ProductId";

            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SalesId", salesId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Error checking sales detail: {ex.Message}");
            return false;
        }
    }

    private void BindSalesData(int salesId)
    {
        string query = @"SELECT s.SALES_ID, s.SALESMAN_NAME, s.BILL_NO, s.PAYMENT_MODE, 
                            p.PATIENT_NAME, p.ADDRESS, p.MOBILE, p.DR_NAME, p.CITY
                     FROM tbl_SalesMaster s
                     LEFT JOIN tbl_Patients p ON s.PATIENT_ID = p.PATIENT_ID
                     WHERE s.SALES_ID = @SalesID";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SalesID", salesId);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtSalesmanName.Text = reader["SALESMAN_NAME"].ToString();
                        txtBillNo.Text = reader["BILL_NO"].ToString();
                        ddlPaymentMode.SelectedValue = reader["PAYMENT_MODE"].ToString();
                        txtPatientName.Text = reader["PATIENT_NAME"].ToString();
                        txtAddress.Text = reader["ADDRESS"].ToString();
                        txtPhoneNo.Text = reader["MOBILE"].ToString();
                        txtDrName.Text = reader["DR_NAME"].ToString();
                        txtCity.Text = reader["CITY"].ToString();
                    }
                }
            }
        }
    }
    private void BindProductDetails(int salesId)
    {
        // Define the SQL query to retrieve product details
        string query = @"
        SELECT 
            pm.PROD_NM AS ProductName, 
            pd.BATCH_NO AS BatchNo, 
            sd.QTY, 
            sd.AMOUNT AS MRP, 
            pm.UNIT AS Unit, 
            ISNULL(tm.PERCENTAGE, 0) AS GST, 
            (sd.AMOUNT * sd.QTY) AS BaseAmount, 
            ((sd.AMOUNT * sd.QTY) * ISNULL(tm.PERCENTAGE, 0) / 100) AS GSTAmount,
            ((sd.AMOUNT * sd.QTY) + ((sd.AMOUNT * sd.QTY) * ISNULL(tm.PERCENTAGE, 0) / 100)) AS TotalAmount
        FROM 
            tbl_SalesDetail sd
        INNER JOIN 
            tbl_ProdDetail pd ON sd.PROD_ID = pd.PROD_ID
        INNER JOIN 
            tbl_ProdMaster pm ON sd.PROD_ID = pm.PROD_ID
        LEFT JOIN 
            tbl_TaxMaster tm ON pm.TAX_ID = tm.TAX_ID
        WHERE 
            sd.SALES_ID = @SalesID";

        // Establish the database connection and retrieve data
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SalesID", salesId);
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvProductDetails.DataSource = dt;
                    gvProductDetails.DataBind();
                }
            }
        }
    }


}