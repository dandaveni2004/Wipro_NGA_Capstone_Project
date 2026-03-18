namespace CustomerSupportAPI.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }

        public string CustomerName { get; set; }

        public string Issue { get; set; }

        public string Status { get; set; }

        public int AgentId { get; set; }

        public int CategoryId { get; set; }
    }
}