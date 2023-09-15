using Microsoft.AspNetCore.Mvc;

namespace DeviceManagment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHashids _hashids;

        public AuthController(IAuthService authService, IHashids hashids)
        {
            _authService = authService;
            _hashids = hashids;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(UserRegister request)
        {
            //we need to grab the email from the object since
            //we need to used it to check if the user exist
            var response = await _authService.Register(
                new User
                {
                    EmployeeId = _hashids.Decode(request.EmployeeId)[0]
                }, request.Password);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            //we need to grab the email from the object since
            //we need to used it to check if the user exist
            var response = await _authService.Login(request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
