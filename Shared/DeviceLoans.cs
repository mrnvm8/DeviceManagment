using DeviceManagment.Shared.Auth;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared
{
    public class DeviceLoans
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public int EmployeeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }
        public int? ReturnToUserId { get; set; }
        public bool IsApproved { get; set; }
        public virtual User? User { get; set; }
        public virtual Device? Device { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
