using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddEmp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void btnAddEmployee_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;
        string role = ddlRole.SelectedValue;
        string name = txtName.Text;
        string mobile = txtMobile.Text;
        string address = txtAddress.Text;
        DateTime date = DateTime.Now;

        // Validate inputs
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role) ||
            string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(address))
        {
            string script = "alert('All fields are required.');";
            ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
            return;
        }

        string connString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "INSERT INTO tbl_Users (USERNAME, PASSWORD, ROLE, NAME, DATE, MOBILE, ADDRESS) " +
                           "VALUES (@Username, @Password, @Role, @Name, @Date, @Mobile, @Address)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.Parameters.AddWithValue("@Address", address);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    string script = "alert('Employee Or Admin added successfully.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
                    ClearFields();
                    Response.Redirect("EmpDetail.aspx");
                }
                catch (Exception ex)
                {
                    string script = $"alert('An error occurred: {ex.Message}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertScript", script, true);
                }
            }
        }
    }
    private void ClearFields()
    {
        txtUsername.Text = string.Empty;
        txtPassword.Text = string.Empty;
        ddlRole.SelectedIndex = 0;
        txtName.Text = string.Empty;
        txtMobile.Text = string.Empty;
        txtAddress.Text = string.Empty;
    }
}
