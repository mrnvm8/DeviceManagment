using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<TicketResponse>>>> GetTickets()
        {
            var result = await _ticketService.GetTickets();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize]
        [ProducesResponseType(200, Type = typeof(TicketResponse))] //Ok
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<TicketResponse>>> GetTicketById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _ticketService.GetTicketById(id);

            if (result.Data is null)
            {
                return NotFound($"Ticket Id: {id} not found.");
            }

            return Ok(result);
        }

        [HttpPost, Authorize]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddTicket([FromBody] TicketResponse ticket)
        {
            if (ticket is null)
            {
                return BadRequest(); // 400 Bad request
            }

            var result = await _ticketService.AddTicket(ticket);
            return Ok(result);
        }

        [HttpPut(template: "{id}"), Authorize]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateTicket(string id, TicketResponse ticket)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _) || ticket.TicketId != id)
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var exist = await _ticketService.GetTicketById(id);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _ticketService.UpdateTicket(ticket, id);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveTicket(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _ticketService.GetTicketById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _ticketService.DeleteTicket(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"Ticket Id: {id} was not found.");
            }
        }

        [HttpPut("acknowledge"), Authorize]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> AcknowledgeTicket(TicketResponse ticket)
        {

            var exist = await _ticketService.GetTicketById(ticket.TicketId);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _ticketService.TicketAcknowledge(ticket);

            return Ok(result);
        }

        [HttpPut("archive"), Authorize]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> ArchiveTicket(TicketResponse ticket)
        {
            var exist = await _ticketService.GetTicketById(ticket.TicketId);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _ticketService.TicketArchived(ticket);

            return Ok(result);
        }

    }
}
