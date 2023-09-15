using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared.Responses
{
    public class EmployeeResponse
    {
        public string EmplyeeId { get; set; } = string.Empty;
        public string PersonId { get; set; } = string.Empty;
        public string DepartmentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string OfficeName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Work Email is required"),
         EmailAddress,
         StringLength(40),
         Display(Name = "Work Email")]
        public string Email { get; set; } = string.Empty;
    }
}
