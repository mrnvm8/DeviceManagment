using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeviceManagment.Shared.Responses
{
    public class DepartmentResponse
    {
        public string DepartId { get; set; } = string.Empty;
        public string OfficeId { get; set; } = string.Empty;
        [Required, StringLength(40, MinimumLength = 4),
        DataType(DataType.Text),
        Display(Name = "Department Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,20}$",
        ErrorMessage = "Characters are not allowed.")]
        public string DepartmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OfficeName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
