using DeviceManagment.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(List<DepartmentResponse>))]
        public async Task<ActionResult<ServiceResponse<List<DepartmentResponse>>>> GetDepartmentList()
        {
            var result = await _departmentService.GetDepartmentList();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(DepartmentResponse))] //Ok
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<DepartmentResponse>>> GetDepartmentById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _departmentService.GetDepartmentById(id);

            if (result.Data is null)
            {
                return NotFound($"Department Id: {id} not found.");
            }

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddDepartment([FromBody] DepartmentResponse department)
        {
            if(department is null)
                {
                return BadRequest(); // 400 Bad request
            }

            var result = await _departmentService.AddDepartment(department);
            return Ok(result);
        }

        [HttpPut(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateEmployee(string id, DepartmentResponse department)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _) || department.DepartId != id)
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var exist = await _departmentService.GetDepartmentById(id);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _departmentService.UpdateDepartment(id, department);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveDepartment(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _departmentService.GetDepartmentById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _departmentService.RemoveDepartment(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"departmentn Id: {id} was not found.");
            }
        }
    }
}
