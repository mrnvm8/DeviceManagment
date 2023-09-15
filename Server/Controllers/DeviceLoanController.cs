using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceLoanController : ControllerBase
    {
        private readonly IDeviceLoanService _deviceLoan;

        public DeviceLoanController(IDeviceLoanService deviceLoanService)
        {
            _deviceLoan = deviceLoanService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<ServiceResponse<List<DeviceLoans>>>> GetDeviceLoanList()
        {
            var result = await _deviceLoan.GetDeviceLoansList();
            return Ok(result);
        }

        [HttpGet("history/{id}"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<DeviceLoans>>>> GetHistory(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }
            var result = await _deviceLoan.HistoryOfDevice(id);
            return Ok(result);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ServiceResponse<DeviceLoans>>> GetDeviceLoanById(string id)
        {
            if (string.IsNullOrEmpty(id) || int.TryParse(id, out _))
            {
                return BadRequest($"Error Id {id} , It's  doesn't exist.");
            }
            var result = await _deviceLoan.GetDeviceLoanById(id);
            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> AssignDevice(DeviceLoanResponse deviceLoan)
        {
            if (deviceLoan is null)
            {
                return BadRequest(deviceLoan);
            }

            var result = await _deviceLoan.AssignDevice(deviceLoan);
            return Ok(result);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> UnassignDevice(string id, DeviceLoanResponse deviceLoans)
        {
            if (string.IsNullOrEmpty(deviceLoans.LoanId) || int.TryParse(deviceLoans.LoanId, out _))
            {
                return BadRequest($"Error Id {deviceLoans.LoanId} , It's  doesn't exist.");
            }

            var result = await _deviceLoan.UnassignDevice(id, deviceLoans);
            return Ok(result);
        }
    }
}
