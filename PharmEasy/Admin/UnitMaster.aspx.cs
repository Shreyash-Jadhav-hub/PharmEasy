using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UnitMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind_Data();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_Data();
    }
    public void Bind_Data()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UNIT_LIST", conn); // Assuming UNIT_LIST is your stored procedure for UnitMaster
                cmd.CommandType = CommandType.StoredProcedure;

                // Customize parameters based on your search criteria
                if (!string.IsNullOrEmpty(txtSearchUnitName.Text.Trim()) && ddlSearchIsActive.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 2); // Statement for searching by unit name
                    cmd.Parameters.AddWithValue("@UNIT_NM", txtSearchUnitName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ACTIVE", DBNull.Value);
                }
                else if (string.IsNullOrEmpty(txtSearchUnitName.Text.Trim()) && ddlSearchIsActive.SelectedValue != "2")
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 3); // Statement for filtering by active status
                    cmd.Parameters.AddWithValue("@UNIT_NM", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ACTIVE", ddlSearchIsActive.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATEMENT", 1); // Default statement to fetch all units
                    cmd.Parameters.AddWithValue("@UNIT_NM", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ACTIVE", DBNull.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    GridViewUnits.DataSource = dt;
                    GridViewUnits.DataBind();
                }
                else
                {
                    GridViewUnits.DataSource = null;
                    GridViewUnits.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            string script = $"alert('Error: {ex.Message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "SearchError", script, true);
        }
    }

}