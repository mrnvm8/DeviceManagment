using DeviceManagment.Shared.Auth;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public int DeviceTypeId { get; set; }
        public string TicketTitle { get; set; } = string.Empty;
        public string TicketIssue { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime TicketDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime ArangedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime FixedDate { get; set; } = DateTime.Now;
        public string? TicketSolution { get; set; }
        public string? TicketUpdate { get; set; }
        public string TicketYear { get; set; } = string.Empty;
        public bool IssueSolved { get; set; } = false;
        public bool Updated { get; set; } = false;
        public virtual User? Users { get; set; }
        public virtual Device? Devices { get; set; }
        public virtual DeviceType? Types { get; set; }
    }
}
