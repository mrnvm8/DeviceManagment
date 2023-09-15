using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IDeviceTypeService _deviceTypeService;

        public DeviceTypeController(IDeviceTypeService deviceTypeService)
        {
            _deviceTypeService = deviceTypeService;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(List<DeviceTypeResponse>))]
        public async Task<ActionResult<ServiceResponse<List<DeviceTypeResponse>>>> GetDeviceTypeList()
        {
            var result = await _deviceTypeService.GetDeviceTypeList();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize]
        [ProducesResponseType(200, Type = typeof(DeviceTypeResponse))] //Ok
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<DeviceTypeResponse>>> GetDeviceTypById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _deviceTypeService.GetDeviceTypeById(id);
            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddDeviceType([FromBody] DeviceTypeResponse deviceType)
        {
            if (deviceType is null)
            {
                return BadRequest(); // 400 Bad request
            }

            var result = await _deviceTypeService.AddDeviceType(deviceType);
            if (result.Success.Equals(false))
            {
                return BadRequest($"{result.Message}");
            }
            return Ok(result);
        }

        [HttpPut(template: "{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateDeviceType(string id, DeviceTypeResponse deviceType)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _) || deviceType.Id != id)
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var exist = await _deviceTypeService.GetDeviceTypeById(id);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _deviceTypeService.UpdateDeviceType(id, deviceType);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveDeviceType(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _deviceTypeService.GetDeviceTypeById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _deviceTypeService.RemoveDeviceType(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"type Id: {id} was not found.");
            }
        }
    }
}
