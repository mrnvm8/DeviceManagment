using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeviceManagment.Shared.Responses
{
    public class DeviceResponse
    {
        public string DeviceId { get; set; } = string.Empty;
        public string DepartmentId { get; set; } = string.Empty;
        public string TypeId { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public string IMEINo { get; set; } = string.Empty;
        public decimal PurchasedPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchasedDate { get; set; } = DateTime.Today;
        public Condition Condition { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
