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

public partial class Admin_Receipt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                btnSave.Visible = false;
                btnEdit.Visible = true;
                BindReceiptData(Convert.ToInt32(Request.QueryString["id"]));
            }
            else
            {
                btnSave.Visible = true;
                btnEdit.Visible = false;
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }
    }
    [WebMethod]
    public static List<string> GetPayerNames(string prefixText, int count)
    {
        List<string> payerNames = new List<string>();
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = @"
        SELECT DISTINCT p.PATIENT_NAME
        FROM [dbo].[tbl_Patients] p
        WHERE p.PATIENT_NAME LIKE @SearchText + '%'";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar).Value = prefixText.Trim();
                conn.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        payerNames.Add(sdr["PATIENT_NAME"].ToString());
                    }
                }
            }
        }

        return payerNames;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // Validation for Amount
            decimal amount;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid amount.');", true);
                return;
            }

            // Validation for Payment Type
            if (!rbCash.Checked && !rbCredit.Checked)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a payment type.');", true);
                return;
            }

            string payer = txtPayer.Text.Trim();
            int patientId = GetPatientID(payer);

            if (patientId == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Payer not found.');", true);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO [dbo].[tbl_Receipt] (DATE, AMOUNT, PAYTYPE, PAYER, DESCRIPTION, PATIENT_ID) VALUES (@DATE, @AMOUNT, @PAYTYPE, @PAYER, @DESCRIPTION, @PATIENT_ID)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DATE", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null));
                        cmd.Parameters.AddWithValue("@AMOUNT", amount);
                        cmd.Parameters.AddWithValue("@PAYTYPE", rbCash.Checked ? "Cash" : "Credit");
                        cmd.Parameters.AddWithValue("@PAYER", payer);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@PATIENT_ID", patientId);

                        cmd.ExecuteNonQuery();
                    }
                    ClearFields();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Receipt saved successfully.');", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
                }
            }
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        Response.Redirect("RptReceipt.aspx");
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // Validation for Amount
            decimal amount;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid amount.');", true);
                return;
            }

            // Validation for Payment Type
            if (!rbCash.Checked && !rbCredit.Checked)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a payment type.');", true);
                return;
            }

            int receiptId = Convert.ToInt32(Request.QueryString["id"]);
            string payer = txtPayer.Text.Trim();
            int patientId = GetPatientID(payer);

            if (patientId == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Payer not found.');", true);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE [dbo].[tbl_Receipt] SET DATE = @DATE, AMOUNT = @AMOUNT, PAYTYPE = @PAYTYPE, PAYER = @PAYER, DESCRIPTION = @DESCRIPTION, PATIENT_ID = @PATIENT_ID WHERE RECEIPT_ID = @RECEIPT_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RECEIPT_ID", receiptId);
                        cmd.Parameters.AddWithValue("@DATE", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null));
                        cmd.Parameters.AddWithValue("@AMOUNT", amount);
                        cmd.Parameters.AddWithValue("@PAYTYPE", rbCash.Checked ? "Cash" : "Credit");
                        cmd.Parameters.AddWithValue("@PAYER", payer);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@PATIENT_ID", patientId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Receipt updated successfully.');", true);
                            Response.Redirect("RptReceipt.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to update receipt.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
                }
            }
        }
    }
    private int GetPatientID(string payer)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT PATIENT_ID FROM [dbo].[tbl_Patients] WHERE PATIENT_NAME = @PAYER";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PAYER", payer);

                try
                {
                    conn.Open();
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
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving patient ID: " + ex.Message);
                }
            }
        }
    }
    private void ClearFields()
    {
        txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtPayer.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtDescription.Text = string.Empty;
        rbCash.Checked = false;
        rbCredit.Checked = false;
    }
    private void BindReceiptData(int receiptId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [dbo].[tbl_Receipt] WHERE RECEIPT_ID = @RECEIPT_ID";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RECEIPT_ID", receiptId);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtDate.Text = Convert.ToDateTime(reader["DATE"]).ToString("dd-MM-yyyy");
                            txtAmount.Text = reader["AMOUNT"].ToString();
                            txtPayer.Text = reader["PAYER"].ToString();
                            txtDescription.Text = reader["DESCRIPTION"].ToString();
                            if (reader["PAYTYPE"].ToString() == "Cash")
                            {
                                rbCash.Checked = true;
                            }
                            else
                            {
                                rbCredit.Checked = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving receipt details: " + ex.Message);
                }
            }
        }
    }
    protected void txtPayer_TextChanged(object sender, EventArgs e)
    {
        string payerName = txtPayer.Text.Trim();
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        bool isValid = false;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "SELECT COUNT(*) FROM [dbo].[tbl_Patients] WHERE PATIENT_NAME = @PayerName";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PayerName", SqlDbType.NVarChar).Value = payerName;
                conn.Open();

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    isValid = true;
                }
            }
        }

        if (isValid)
        {
            
        }
        else
        {
            txtPayer.Text = string.Empty;

            string alertMessage = "Payer name not found. Please enter a valid payer name.";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{alertMessage}');", true);
        }
    }
}