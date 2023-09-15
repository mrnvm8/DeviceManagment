namespace DeviceManagment.Client.Services.TicketService
{
    public interface ITicketService
    {
        List<TicketResponse> Tickets { get; set; }
        Task GetTickets();
        Task<ServiceResponse<TicketResponse>> GetTicketById(string ticketId);
        Task<ServiceResponse<bool>> AddTicket(TicketResponse ticket);
        Task<ServiceResponse<bool>> UpdateTicket(TicketResponse ticket);
        Task<ServiceResponse<bool>> RemoveTicket(string ticketId);
        Task<ServiceResponse<bool>> AcknowlegeTicket(TicketResponse ticket);
        Task<ServiceResponse<bool>> ArchiveTicket(TicketResponse ticket);
    }
}
