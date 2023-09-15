using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(List<PersonResponse>))]
        public async Task<ActionResult<ServiceResponse<List<PersonResponse>>>> GetPeopleList()
        {
            var result = await _personService.GetPeopleList();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(PersonResponse))] //Ok
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<PersonResponse>>> GetPersonById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _personService.GetPersonById(id);
            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddPerson([FromBody] PersonResponse person)
        {
            if (person is null)
            {
                return BadRequest(); // 400 Bad request
            }

            var result = await _personService.AddPerson(person);
            return Ok(result);
        }

        [HttpPut(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdatePerson(string id, PersonResponse person)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _) || person.PersonId != id)
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var exist = await _personService.GetPersonById(id);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _personService.UpdatePerson(id, person);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemovePerson(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _personService.GetPersonById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _personService.RemovePerson(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"person Id: {id} was not found.");
            }
        }
    }
}
