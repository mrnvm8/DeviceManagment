using DeviceManagment.Shared.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("bytype/{typeId}"), Authorize]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<DeviceResponse>>>> GetDeviceListByType(string typeId)
        {
            var result = await _deviceService.GetDeviceListByType(typeId);
            return Ok(result);
        }

        [HttpGet("Items")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<DataItem>>>> GetChartData()
        {
            var result = await _deviceService.BoughtDeviceByYears();
            return Ok(result);
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(List<DeviceResponse>))]
        public async Task<ActionResult<ServiceResponse<List<DeviceResponse>>>> GetDeviceList()
        {
            var result = await _deviceService.GetDevicesList();
            return Ok(result);
        }

        [HttpGet(template: "{id}"), Authorize]
        [ProducesResponseType(200, Type = typeof(DeviceResponse))] //Ok
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<ActionResult<ServiceResponse<DeviceTypeResponse>>> GetDeviceById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            var result = await _deviceService.GetDeviceById(id);
            return Ok(result);
        }

        [HttpPost, Authorize]
        [ProducesResponseType(200, Type = typeof(bool))] //Ok
        [ProducesResponseType(400)] //Bad Request
        public async Task<ActionResult<ServiceResponse<bool>>> AddDevice([FromBody] DeviceResponse device)
        {
            if (device is null)
            {
                return BadRequest(); // 400 Bad request
            }

            var result = await _deviceService.AddDevice(device);
            if (result.Success.Equals(false))
            {
                return BadRequest($"{result.Message}");
            }
            return Ok(result);
        }

        [HttpPut, Authorize]
        [ProducesResponseType(200)] //Ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] //Resource not found
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateDevice(DeviceResponse device)
        {
            if (string.IsNullOrEmpty(device.DeviceId) || int.TryParse(device.DeviceId, out _))
            {
                return BadRequest($"Error Id {device.DeviceId} , It's  doesn't exist.");
            }

            var exist = await _deviceService.GetDeviceById(device.DeviceId);
            if (exist.Data == null)
            {
                return NotFound();
            }
            var result = await _deviceService.UpdateDevice(device);

            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveDevice(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }

            //checking if person exist
            var exist = await _deviceService.GetDeviceById(id);

            if (exist.Data is null)
            {
                return NotFound(); // 404 Resource not found
            }

            var result = await _deviceService.RemoveDevice(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"Device Id: {id} was not found.");
            }
        }
    }
}
