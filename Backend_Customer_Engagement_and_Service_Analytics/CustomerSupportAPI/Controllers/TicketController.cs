using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using CustomerSupportAPI.Models;
namespace CustomerSupportAPI.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        private string connectionString =
        "Server=GANESH;Database=CustomerSupportDB;Trusted_Connection=True;TrustServerCertificate=True;";
        [HttpPost]
        public IActionResult CreateTicket([FromBody] Ticket ticket)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string customerQuery =
                "SELECT CustomerId FROM Customers WHERE Name=@name";
                SqlCommand customerCmd =
                new SqlCommand(customerQuery, conn);
                customerCmd.Parameters.AddWithValue("@name", ticket.CustomerName);
                object result = customerCmd.ExecuteScalar();
                if (result == null)
                {
                    return BadRequest("Customer not found");
                }
                int customerId = Convert.ToInt32(result);
                string checkTicketQuery =
                "SELECT COUNT(*) FROM Tickets WHERE Issue=@issue AND CustomerId=@customerId";
                SqlCommand checkCmd =
                new SqlCommand(checkTicketQuery, conn);
                checkCmd.Parameters.AddWithValue("@issue", ticket.Issue);
                checkCmd.Parameters.AddWithValue("@customerId", customerId);
                int ticketCount = (int)checkCmd.ExecuteScalar();
                if (ticketCount > 0)
                {
                    return BadRequest("Duplicate ticket detected");
                }
                string query =
                @"INSERT INTO Tickets(CustomerId,AgentId,CategoryId,Issue,Status)
                VALUES (@customerId,@agentId,@categoryId,@issue,@status)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@agentId", ticket.AgentId);
                cmd.Parameters.AddWithValue("@categoryId", ticket.CategoryId);
                cmd.Parameters.AddWithValue("@issue", ticket.Issue);
                cmd.Parameters.AddWithValue("@status", ticket.Status);
                cmd.ExecuteNonQuery();
            }
            return Ok("Ticket Created Successfully");
        }
        [HttpGet]
        public IActionResult GetTickets()
        {
            List<object> tickets = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query =
                @"SELECT 
                T.TicketId,
                C.Name AS CustomerName,
                TC.CategoryName,
                A.AgentName,
                T.Issue,
                T.Status
                FROM Tickets T
                JOIN Customers C ON T.CustomerId = C.CustomerId
                JOIN Agents A ON T.AgentId = A.AgentId
                JOIN TicketCategories TC ON T.CategoryId = TC.CategoryId";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tickets.Add(new
                    {
                        ticketId = reader["TicketId"],
                        customerName = reader["CustomerName"].ToString(),
                        category = reader["CategoryName"].ToString(),
                        agent = reader["AgentName"].ToString(),
                        issue = reader["Issue"].ToString(),
                        status = reader["Status"].ToString()
                    });
                }
            }
            return Ok(tickets);
        }
        [HttpPut("{ticketId}")]
        public IActionResult UpdateTicketStatus(int ticketId, [FromBody] string newStatus)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string currentStatusQuery =
                "SELECT Status FROM Tickets WHERE TicketId=@id";
                SqlCommand statusCmd =
                new SqlCommand(currentStatusQuery, conn);
                statusCmd.Parameters.AddWithValue("@id", ticketId);
                string currentStatus = statusCmd.ExecuteScalar()?.ToString();
                if (currentStatus == "Closed" && newStatus == "Open")
                {
                    return BadRequest("Closed ticket cannot be reopened");
                }
                string query =
                "UPDATE Tickets SET Status=@status WHERE TicketId=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", ticketId);
                cmd.ExecuteNonQuery();
            }
            return Ok("Ticket status updated");
        }
    }
}