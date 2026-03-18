using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using CustomerSupportAPI.Models;
namespace CustomerSupportAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private string connectionString =
        "Server=GANESH;Database=CustomerSupportDB;Trusted_Connection=True;TrustServerCertificate=True;";
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery =
                "SELECT COUNT(*) FROM Customers WHERE Email=@email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@email", customer.Email);
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    return BadRequest("Customer already exists");
                }
                string query =
                "INSERT INTO Customers(Name,Email) VALUES(@name,@email)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", customer.Name);
                cmd.Parameters.AddWithValue("@email", customer.Email);
                cmd.ExecuteNonQuery();
            }
            return Ok("Customer Added Successfully");
        }
        [HttpGet]
        public IActionResult GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Name,Email FROM Customers";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
            return Ok(customers);
        }
    }
}