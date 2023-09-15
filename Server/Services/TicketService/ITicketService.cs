namespace DeviceManagment.Server.Services.TicketService
{
    public interface ITicketService : IDisposable
    {
        Task<ServiceResponse<List<TicketResponse>>> GetTickets();
        Task<ServiceResponse<TicketResponse>> GetTicketById(string ticketId);
        Task<ServiceResponse<bool>> UpdateTicket(TicketResponse ticket, string ticketId);
        Task<ServiceResponse<bool>> AddTicket(TicketResponse ticket);
        Task<ServiceResponse<bool>> DeleteTicket(string ticketId);
        Task<ServiceResponse<bool>> TicketArchived(TicketResponse ticket);
        Task<ServiceResponse<bool>> TicketAcknowledge(TicketResponse ticket);
    }
}
