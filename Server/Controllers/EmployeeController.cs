using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(List<EmployeeResponse>))]
        public async Task<ActionResult<ServiceResponse<List<EmployeeResponse>>>> GetEmployeeList()
        {
            var result = await _employeeService.GetEmployees();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(EmployeeResponse))] //Ok
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<EmployeeResponse>>> GetEmployeeById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _employeeService.GetEmployeeById(id);

            if (result.Data is null)
            {
                return NotFound($"Employee Id: {id} not found.");
            }

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddEmployee([FromBody] EmployeeResponse employee)
        {
            if (employee is null)
            {
                return BadRequest(); // 400 Bad request
            }

            var result = await _employeeService.AddEmployee(employee);
            return Ok(result);

        }

        [HttpPut(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateEmployee(string id, EmployeeResponse employee)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _) || employee.EmplyeeId != id)
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var exist = await _employeeService.GetEmployeeById(id);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _employeeService.UpdateEmployee(id, employee);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveEmployee(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _employeeService.GetEmployeeById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _employeeService.RemoveEmployee(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"Employee Id: {id} was not found.");
            }
        }
    }
}
