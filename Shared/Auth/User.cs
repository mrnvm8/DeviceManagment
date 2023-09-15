using System.ComponentModel.DataAnnotations;

namespace DeviceManagment.Shared.Auth
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "employee";
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<DeviceLoans> DevicesLoans { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public User()
        {
            DevicesLoans = new HashSet<DeviceLoans>();
            Tickets = new HashSet<Ticket>();
        }
    }
}
