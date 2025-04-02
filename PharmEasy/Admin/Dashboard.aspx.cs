using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dashboard : System.Web.UI.Page
{
    private TaskDataAccess taskDataAccess = new TaskDataAccess();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRecentSales();
            BindTasks();
            litDailySales.Text = GetDailySales().ToString("C"); // Format as currency
            litMonthlySales.Text = GetMonthlySales().ToString("C");
            litTotalBalance.Text = GetTotalBalance().ToString("C");
            litTotalRevenue.Text = GetTotalRevenue().ToString("C");
        }
    }
    private void BindRecentSales()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"
            SELECT TOP 5
                s.DATE AS [DATE],
                s.SALES_ID AS [BILL_NO],
                p.PATIENT_NAME AS [PATIENT_NAME],
                s.PAYTYPE AS [PAYMENT_MODE],
                s.TTL_AMT AS [TOTAL_AMOUNT],
                COALESCE(SUM(t.CR) - SUM(t.DR), 0) AS [BALANCE]
            FROM 
                dbo.tbl_SalesMaster s
            JOIN 
                dbo.tbl_Patients p ON s.PATIENT_ID = p.PATIENT_ID
            LEFT JOIN 
                dbo.tbl_TransactionDetail t ON s.PATIENT_ID = t.PATIENT_ID
            GROUP BY 
                s.DATE, s.SALES_ID, p.PATIENT_NAME, s.PAYTYPE, s.TTL_AMT
            ORDER BY 
                s.DATE DESC;
        ";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvRecentSales.DataSource = dt;
            gvRecentSales.DataBind();
        }
    }
    protected void btnDetail_Command(object sender, CommandEventArgs e)
    {
        string billNo = e.CommandArgument.ToString();
        // Redirect or perform action based on the bill number
        Response.Redirect("SalesMaster.aspx?billNo=" + billNo);
    }
    public class TaskDataAccess
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        public DataTable GetTasks()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TaskID, TaskDescription, IsCompleted FROM tbl_Tasks ORDER BY CreatedDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void AddTask(string taskDescription)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbl_Tasks (TaskDescription, IsCompleted) VALUES (@TaskDescription, 0)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TaskDescription", taskDescription);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateTask(int taskId, bool isCompleted)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE tbl_Tasks SET IsCompleted = @IsCompleted WHERE TaskID = @TaskID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TaskID", taskId);
                cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteTask(int taskId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM tbl_Tasks WHERE TaskID = @TaskID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TaskID", taskId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
    private void BindTasks()
    {
        DataTable dtTasks = taskDataAccess.GetTasks();
        rptTasks.DataSource = dtTasks;
        rptTasks.DataBind();
    }
    protected void chkTask_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkTask = (CheckBox)sender;
        RepeaterItem item = (RepeaterItem)chkTask.NamingContainer;

        // Get the TaskID from the button's CommandArgument
        int taskId = Convert.ToInt32(((Button)item.FindControl("btnDelete")).CommandArgument);

        // Determine if the task is completed based on the checkbox's checked state
        bool isCompleted = chkTask.Checked;

        // Update the task status in the database
        taskDataAccess.UpdateTask(taskId, isCompleted);

        // Optionally, you can re-bind the task list to reflect the changes immediately
        BindTasks();
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        int taskId = Convert.ToInt32(e.CommandArgument);
        taskDataAccess.DeleteTask(taskId);
        BindTasks();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string taskDescription = txtTask.Text.Trim();
        if (!string.IsNullOrEmpty(taskDescription))
        {
            taskDataAccess.AddTask(taskDescription);
            txtTask.Text = string.Empty;
            BindTasks();
        }
    }
    public decimal GetDailySales()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        string query = "SELECT ISNULL(SUM(GROSS_AMT), 0) FROM dbo.tbl_SalesMaster WHERE DATE = CAST(GETDATE() AS DATE)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            object result = command.ExecuteScalar();
            return Convert.ToDecimal(result);
        }
    }
    public decimal GetMonthlySales()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        string query = "SELECT ISNULL(SUM(GROSS_AMT), 0) FROM dbo.tbl_SalesMaster WHERE DATE >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1) AND DATE < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            object result = command.ExecuteScalar();
            return Convert.ToDecimal(result);
        }
    }
    public float GetTotalBalance()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;
        // Updated query to get the sum of BALANCE for the current day
        string query = "SELECT SUM(BALANCE) FROM dbo.tbl_TransactionDetail WHERE DATE >= CAST(GETDATE() AS DATE)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            // ExecuteScalar to get the sum
            object result = command.ExecuteScalar();
            // Convert the result to float and return
            return result != DBNull.Value ? Convert.ToSingle(result) : 0;
        }
    }
    private decimal GetTotalRevenue()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        // SQL query to calculate total revenue
        string query = @"
        SELECT 
            ISNULL((SELECT SUM(TTL_AMT) FROM dbo.tbl_SalesMaster), 0) +
            ISNULL((SELECT SUM(BALANCE) FROM dbo.tbl_TransactionDetail), 0) -
            ISNULL((SELECT SUM(NET_AMT) FROM dbo.tbl_PurchaseMaster), 0) AS TotalRevenue";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            object result = command.ExecuteScalar();
            return Convert.ToDecimal(result);
        }
    }


}