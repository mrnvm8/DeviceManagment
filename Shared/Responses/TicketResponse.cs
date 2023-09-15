using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared.Responses
{
    public class TicketResponse
    {
        public string TicketId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string DeviceTypeId { get; set; } = string.Empty;
        [Required]
        public string? TicketTitle { get; set; }
        [Required]
        public string? TicketIssue { get; set; }
        [DataType(DataType.Date)]
        public DateTime TicketDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ArangedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime FixedDate { get; set; }
        public string? TicketSolution { get; set; } 
        public string? TicketUpdate { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceType { get; set; }
        public string? Department { get; set; }
        public string?TicketYear { get; set; } 
        public bool IssueSolved { get; set; } = true;
        public bool Updated { get; set; } = true;
    }
}
