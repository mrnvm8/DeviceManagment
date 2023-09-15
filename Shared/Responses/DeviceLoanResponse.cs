using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared.Responses
{
    public class DeviceLoanResponse
    {
        public string LoanId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }
        public string ReturnToUserId { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public string  EmployeeName { get; set; } = string.Empty;
    }
}
