using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeviceManagment.Shared.Responses
{
    public class PersonResponse
    {
        public string PersonId { get; set; } = string.Empty;
        [Required, StringLength(20, MinimumLength = 4),
        DataType(DataType.Text),
        Column(TypeName = "varchar(20)"),
        Display(Name = "First Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Column(TypeName = "varchar(20)"),
        Display(Name = "Last Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,20}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Surname { get; set; } = string.Empty;
        public Gender Gender { get; set; }
    }
}
