using Microsoft.Data.SqlClient;
using CustomerSupportAPI.Models;

namespace CustomerSupportAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        string connectionString =
        "Server=GANESH;Database=CustomerSupportDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public void CreateTicket(Ticket ticket)
        {
            using SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string customerQuery =
            "SELECT CustomerId FROM Customers WHERE Name=@name";

            SqlCommand customerCmd =
            new SqlCommand(customerQuery, conn);

            customerCmd.Parameters.AddWithValue("@name", ticket.CustomerName);

            object result = customerCmd.ExecuteScalar();

            if (result == null)
            {
                throw new Exception("Customer not found");
            }

            int customerId = Convert.ToInt32(result);

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

        public List<object> GetTickets()
        {
            List<object> tickets = new List<object>();

            using SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string query =
            @"SELECT 
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
                    customerName = reader["CustomerName"].ToString(),
                    category = reader["CategoryName"].ToString(),
                    agent = reader["AgentName"].ToString(),
                    issue = reader["Issue"].ToString(),
                    status = reader["Status"].ToString()
                });
            }

            return tickets;
        }
    }
}