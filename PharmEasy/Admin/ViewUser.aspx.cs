using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ViewUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string username = Session["Username"] as string;
            if (username != null)
            {
                GetUserDetails(username);
            }
            else
            {
                lblMessage.Text = "User not logged in.";
            }
        }
    }
    private void GetUserDetails(string username)
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-RHE90U9\\SQLEXPRESS;Initial Catalog=PharmaDB;Integrated Security=True;TrustServerCertificate=True");
        con.Open();
        string query = "SELECT /*UID,*/ USERNAME, PASSWORD, ROLE, NAME, DATE, MOBILE, ADDRESS FROM tbl_Users WHERE USERNAME = @Username";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@Username", username);
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            //lblUID.Text = reader["UID"].ToString();
            lblUsername.Text = reader["USERNAME"].ToString();
            lblPassword.Text = reader["PASSWORD"].ToString();
            lblRole.Text = reader["ROLE"].ToString();
            lblName.Text = reader["NAME"].ToString();
            lblDate.Text = reader["DATE"].ToString();
            lblMobile.Text = reader["MOBILE"].ToString();
            lblAddress.Text = reader["ADDRESS"].ToString();

        }
        else
        {
            lblMessage.Text = "User details not found.";
        }

        reader.Close();
        con.Close();
    }
}