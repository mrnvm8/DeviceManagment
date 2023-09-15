using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeServices _officeServices;

        public OfficeController(IOfficeServices officeServices)
        {
            _officeServices = officeServices;
        }

        [HttpGet, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(List<OfficeResponse>))]
        public async Task<ActionResult<ServiceResponse<List<OfficeResponse>>>> GetOfficeList()
        {
            var result = await _officeServices.GetOfficeList();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(OfficeResponse))] //Ok
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<OfficeResponse>>> GetOfficeById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _officeServices.GetOfficeById(id);

            if (result.Data is null)
            {
                return NotFound($"Office Id: {id} not found.");
            }

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddOffice([FromBody] OfficeResponse office)
        {
            if (office is null)
            {
                return BadRequest(); // 400 Bad request
            }

            var result = await _officeServices.AddOffice(office);
            return Ok(result);
        }

        [HttpPut(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateOffice(string id, OfficeResponse office)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _) || office.OfficeId != id)
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var exist = await _officeServices.GetOfficeById(id);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _officeServices.UpdateOffice(id, office);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveOffice(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _officeServices.GetOfficeById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _officeServices.RemoveOffice(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"office Id: {id} was not found.");
            }
        }
    }
}
