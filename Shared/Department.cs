using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagment.Shared
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public int OfficeId { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }

        public Office? Offices { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public Department()
        {
            Employees = new HashSet<Employee>();
            Devices = new HashSet<Device>();
        }
    }
}
