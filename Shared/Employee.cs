using DeviceManagment.Shared.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagment.Shared
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public int PersonId { get; set; }
        public int DepartmentId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? WorkEmail { get; set; }
        public bool IsEmployeeActive { get; set; } = true;
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<DeviceLoans> DevicesLoans { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Department? Department { get; set; }
        public Employee()
        {
            Users = new HashSet<User>();
            DevicesLoans = new HashSet<DeviceLoans>();
        }

    }
}
