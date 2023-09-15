using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared.Auth
{
    public class UserRegister
    {
        //public int EmployeeId { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
