using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeviceManagment.Shared.Responses
{
    public class OfficeResponse
    {
        public string OfficeId { get; set; } = string.Empty;
        [Required, StringLength(40, MinimumLength = 4),
         DataType(DataType.Text),
         Display(Name = "Office Name"),
         RegularExpression(@"^[a-zA-Z''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(40, MinimumLength = 4),
         DataType(DataType.Text),
         Display(Name = "Office Location"),
         RegularExpression(@"^[a-zA-Z''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Location { get; set; } = string.Empty;
    }
}
