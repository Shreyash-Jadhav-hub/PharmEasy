using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddCust : System.Web.UI.Page
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
               
                btnAddCustomer.Visible = false; 
                btnEdit.Visible = true;         
                int customerId = Convert.ToInt32(Request.QueryString["id"]);
                BindCustomerData(customerId);   
            }
            else
            {
                
                btnAddCustomer.Visible = true;   
                btnEdit.Visible = false;         
              
            }
        }
    }
    protected void btnAddCustomer_Click(object sender, EventArgs e)
    {
        // Retrieve input values from the form
        string patientName = txtPatientName.Text.Trim();
        string address = txtAddress.Text.Trim();
        string mobile = txtMobile.Text.Trim();
        string doctorName = txtDoctorName.Text.Trim();
        string city = txtCity.Text.Trim();

        // Validate input
        if (string.IsNullOrEmpty(patientName) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(mobile) ||
            string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(city))
        {
            // Display error message if any field is empty
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('All fields are required.');", true);
            return;
        }

        // Validate mobile number (10 digits)
        var mobilePattern = new Regex(@"^[0-9]{10}$");
        if (!mobilePattern.IsMatch(mobile))
        {
            // Display error message if mobile number is invalid
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid mobile number. Please enter a 10-digit mobile number.');", true);
            return;
        }

        // Code to add customer to the database
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO [dbo].[tbl_Patients] (PATIENT_NAME, ADDRESS, MOBILE, DR_NAME, CITY) VALUES (@PatientName, @Address, @Mobile, @DoctorName, @City)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientName", patientName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                    cmd.Parameters.AddWithValue("@City", city);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Customer added successfully.');", true);
            ClearFormFields();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
        }
    }
    private void ClearFormFields()
    {
        txtPatientName.Text = "";
        txtAddress.Text = "";
        txtMobile.Text = "";
        txtDoctorName.Text = "";
        txtCity.Text = "";
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {

        string patientName = txtPatientName.Text.Trim();
        string address = txtAddress.Text.Trim();
        string mobile = txtMobile.Text.Trim();
        string doctorName = txtDoctorName.Text.Trim();
        string city = txtCity.Text.Trim();


        if (Request.QueryString["id"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid patient ID.');", true);
            return;
        }
        int patientId = Convert.ToInt32(Request.QueryString["id"]);

      
        if (string.IsNullOrEmpty(patientName) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(mobile) ||
            string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(city))
        {
            
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('All fields are required.');", true);
            return;
        }

       
        var mobilePattern = new Regex(@"^[0-9]{10}$");
        if (!mobilePattern.IsMatch(mobile))
        {
            // Display error message if mobile number is invalid
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid mobile number. Please enter a 10-digit mobile number.');", true);
            return;
        }

     
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE [dbo].[tbl_Patients] SET PATIENT_NAME = @PatientName, ADDRESS = @Address, MOBILE = @Mobile, DR_NAME = @DoctorName, CITY = @City WHERE PATIENT_ID = @PatientID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientName", patientName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@PatientID", patientId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Customer details updated successfully.');", true);
            ClearFormFields();
            Response.Redirect("CustDetail.aspx");
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
        }
    }
    private void BindCustomerData(int patientId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        string query = "SELECT PATIENT_NAME, ADDRESS, MOBILE, DR_NAME, CITY FROM [dbo].[tbl_Patients] WHERE PATIENT_ID = @PatientID";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtPatientName.Text = reader["PATIENT_NAME"].ToString();
                            txtAddress.Text = reader["ADDRESS"].ToString();
                            txtMobile.Text = reader["MOBILE"].ToString();
                            txtDoctorName.Text = reader["DR_NAME"].ToString();
                            txtCity.Text = reader["CITY"].ToString();
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Patient not found.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
                }
            }
        }
    }
}