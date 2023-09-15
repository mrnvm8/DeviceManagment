using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DeviceManagment.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashids _hashids;

        public AuthService(DataContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IHashids hashids)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _hashids = hashids;
        }
        public async Task<ServiceResponse<bool>> ChangePassword(string newPassword, int id)
        {
            //get the user
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true,
                Message = "Password has been changed." };
        }

        public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
           .FindFirstValue(ClaimTypes.NameIdentifier));

        public int GetEmployeeId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.Name));

        public string GetRole() => _httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.Role);

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            //Get the user that have this email address in the
            //table Employee
            var employee = await _context.Employees.
                FirstOrDefaultAsync(x => x.WorkEmail.ToLower()
                .Equals(email.ToLower()));

            //if email does not exist return
            if (employee is null)
            {
                response.Success = false;
                response.Message = "User email does not exist";
                return response;
            }

            //checking if the user exist in the table User
            //using employeeId from the employee
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.EmployeeId
                .Equals(employee.EmployeeId));

            if (user is null)
            {
                response.Success = false;
                response.Message = "User not Found";

            }
            else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                // Creating the json web Token
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> Register(User user, string password)
        {
            var response = new ServiceResponse<string>();
            //check if the user exist in the user
            //table before registering them
            if (await UserExist(user.EmployeeId))
            {
                response.Success = false;
                response.Message = "User already exists.";
            }
            else
            {
                //method to create the hash password
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users?.Add(user);
                await _context.SaveChangesAsync();
                response.Data = _hashids.Encode(user.UserId);
                response.Message = "Registration successful";
            }
            return response;
        }

        private string CreateToken(User user)
        {
            //this will create and store token and clamis

            List<Claim> clams = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.EmployeeId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            //needing the key that is stored in the json file
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            //creating new signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //token
            var token = new JwtSecurityToken(
                claims: clams,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            //json web token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        //creating a passwordhash when registering a user
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //crytogrphy algorithm
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                //creating password hash
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Verifying PasswordHash when login
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);

            }
        }
        //Checking if user exist and return a bool true or false
        public async Task<bool> UserExist(int employeeId)
        {
            //Checking if user exist
            var user = await _context.Users
                .AnyAsync(e => e.EmployeeId
                .Equals(employeeId));

            if (user)
            {
                return true;
            }
            return false;
        }
    }
}
