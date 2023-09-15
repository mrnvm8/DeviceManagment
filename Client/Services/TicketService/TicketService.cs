using DeviceManagment.Shared;
using System;

namespace DeviceManagment.Client.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _http;
        public List<TicketResponse> Tickets { get ; set; } =new List<TicketResponse>();

        public TicketService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> AcknowlegeTicket(TicketResponse ticket)
        {
            var result = await _http.PutAsJsonAsync($"api/ticket/acknowledge", ticket);
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<bool>> AddTicket(TicketResponse ticket)
        {
            var result = await _http.PostAsJsonAsync("api/ticket", ticket);
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<bool>> ArchiveTicket(TicketResponse ticket)
        {
            var result = await _http.PutAsJsonAsync($"api/ticket/archive", ticket);
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<TicketResponse>> GetTicketById(string ticketId)
        {
            var response = await _http.GetAsync($"api/ticket/{ticketId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<TicketResponse>() { Message = $"The Id : {ticketId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<TicketResponse>>();
            }
        }

        public async Task GetTickets()
        {
            var result = await _http.GetAsync("api/ticket");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<TicketResponse>>>();
                Tickets = response.Data;
            }
            else
            {
                Tickets = new List<TicketResponse>();
            }
        }

        public async Task<ServiceResponse<bool>> RemoveTicket(string ticketId)
        {
            var result = await _http.DeleteAsync($"api/ticket/{ticketId}");
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<bool>> UpdateTicket(TicketResponse ticket)
        {
            var result = await _http.PutAsJsonAsync($"api/ticket/{ticket.TicketId}", ticket);
            return await ReturnMessage(result);
        }

        private static async Task<ServiceResponse<bool>> ReturnMessage(HttpResponseMessage result)
        {
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
