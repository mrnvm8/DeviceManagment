using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagment.Shared
{
    public class Device
    {
        public int DeviceId { get; set; }
        public int DepartmentId { get; set; }
        public int DeviceTypeId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string? DeviceSerialNo { get; set; } 
        public string? DeviceIMEINo { get; set; }
        public Condition Condition { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasedPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime PurchasedDate { get; set; }
        public virtual DeviceType? DeviceType { get; set; }
        public virtual Department? Department { get; set; }
        
        public virtual ICollection<DeviceLoans> DevicesLoans { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public Device()
        {
            DevicesLoans = new HashSet<DeviceLoans>();
            Tickets = new HashSet<Ticket>();
        }
    }
}
