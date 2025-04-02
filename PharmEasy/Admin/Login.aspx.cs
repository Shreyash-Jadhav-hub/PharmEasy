using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Abandon();
            Session.Clear();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-RHE90U9\\SQLEXPRESS;Initial Catalog=PharmaDB;Integrated Security=True;TrustServerCertificate=True"))
        {
            try
            {
                con.Close();
                con.Open();
                string loginQuery = "SELECT COUNT(*) FROM [dbo].[tbl_Users] WHERE USERNAME = @Username AND PASSWORD = @Password";
                using (SqlCommand command = new SqlCommand(loginQuery, con))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        Session["Username"] = username;

                        SqlCommand cmd = new SqlCommand("SELECT [UID] FROM [dbo].[tbl_Users] WHERE [USERNAME]=@Username", con);
                        cmd.Parameters.AddWithValue("@Username", username);
                        SqlDataAdapter da=new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if(dt.Rows.Count > 0)
                        {
                            Session["userid"] = dt.Rows[0]["UID"].ToString();
                        }

                        Response.Redirect("Dashboard.aspx");
                    }
                    else
                    {
                        string script = "alert('Invalid username or password. Please try again.');";
                        ClientScript.RegisterStartupScript(this.GetType(), "LoginAlert", script, true);
                    }
                }
            }
            catch (Exception ex)
            {
          
                string script = $"alert('An error occurred: {ex.Message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "LoginError", script, true);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
