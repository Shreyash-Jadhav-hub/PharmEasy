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

public partial class Admin_Payment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                btnSave.Visible = false;
                btnEdit.Visible = true;
                BindPaymentData(Convert.ToInt32(Request.QueryString["id"]));
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
    public static List<string> GetReceiverNames(string prefixText, int count)
    {
        List<string> receiverNames = new List<string>();
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
                        receiverNames.Add(sdr["PATIENT_NAME"].ToString());
                    }
                }
            }
        }

        return receiverNames;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Server-side validation for Amount and Payment Type
        if (Page.IsValid)
        {
            decimal amount;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid amount.');", true);
                return;
            }

            if (!rbCash.Checked && !rbCredit.Checked)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a payment type.');", true);
                return;
            }

            string receiver = txtReceiver.Text.Trim();
            int patientId = GetPatientID(receiver);

            if (patientId == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Receiver not found.');", true);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO [dbo].[tbl_Payment] (DATE, AMOUNT, PAYTYPE, RECEIVER, DESCRIPTION, PATIENT_ID) VALUES (@DATE, @AMOUNT, @PAYTYPE, @RECEIVER, @DESCRIPTION, @PATIENT_ID)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DATE", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null));
                        cmd.Parameters.AddWithValue("@AMOUNT", amount); // Ensure the parsed amount is used
                        cmd.Parameters.AddWithValue("@PAYTYPE", rbCash.Checked ? "Cash" : "Credit");
                        cmd.Parameters.AddWithValue("@RECEIVER", receiver);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@PATIENT_ID", patientId != -1 ? (object)patientId : DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }

                    ClearFields();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Payment saved successfully.');", true);
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
        Response.Redirect("RptPayment.aspx");
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

            int paymentId = Convert.ToInt32(Request.QueryString["id"]);
            string receiver = txtReceiver.Text.Trim();
            int patientId = GetPatientID(receiver);

            if (patientId == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Receiver not found.');", true);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE [dbo].[tbl_Payment] SET DATE = @DATE, AMOUNT = @AMOUNT, PAYTYPE = @PAYTYPE, RECEIVER = @RECEIVER, DESCRIPTION = @DESCRIPTION, PATIENT_ID = @PATIENT_ID WHERE PAYMENT_ID = @PAYMENT_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PAYMENT_ID", paymentId);
                        cmd.Parameters.AddWithValue("@DATE", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null));
                        cmd.Parameters.AddWithValue("@AMOUNT", amount); // Use validated amount
                        cmd.Parameters.AddWithValue("@PAYTYPE", rbCash.Checked ? "Cash" : "Credit");
                        cmd.Parameters.AddWithValue("@RECEIVER", receiver);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@PATIENT_ID", patientId != -1 ? (object)patientId : DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Payment updated successfully.');", true);
                            Response.Redirect("RptPayment.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to update payment.');", true);
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
    private int GetPatientID(string receiver)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT PATIENT_ID FROM [dbo].[tbl_Patients] WHERE PATIENT_NAME = @RECEIVER";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RECEIVER", receiver);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving patient ID: " + ex.Message);
                }
            }
        }
    }  
    private void BindPaymentData(int paymentId)
{
    string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string query = "SELECT * FROM [dbo].[tbl_Payment] WHERE PAYMENT_ID = @PAYMENT_ID";
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@PAYMENT_ID", paymentId);

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtDate.Text = Convert.ToDateTime(reader["DATE"]).ToString("dd-MM-yyyy");
                        txtAmount.Text = reader["AMOUNT"].ToString();
                        txtReceiver.Text = reader["RECEIVER"].ToString();
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
                throw new Exception("Error retrieving payment details: " + ex.Message);
            }
        }
    }
}
    protected void txtReceiver_TextChanged(object sender, EventArgs e)
    {
        string receiverName = txtReceiver.Text.Trim();
        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        bool isValid = false;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "SELECT COUNT(*) FROM [dbo].[tbl_Patients] WHERE PATIENT_NAME = @ReceiverName";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@ReceiverName", SqlDbType.NVarChar).Value = receiverName;
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
            txtReceiver.Text = string.Empty;

            string alertMessage = "Receiver name not found. Please enter a valid receiver name.";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{alertMessage}');", true);
        }
    }
    private void ClearFields()
    {
        txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtReceiver.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtDescription.Text = string.Empty;
        rbCash.Checked = false;
        rbCredit.Checked = false;
    }
}