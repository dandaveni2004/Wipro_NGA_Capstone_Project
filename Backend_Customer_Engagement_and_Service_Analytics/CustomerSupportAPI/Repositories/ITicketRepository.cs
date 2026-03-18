using CustomerSupportAPI.Models;

namespace CustomerSupportAPI.Repositories
{
    public interface ITicketRepository
    {
        void CreateTicket(Ticket ticket);

        List<object> GetTickets();
    }
}