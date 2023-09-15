using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagment.Shared
{
    public class Office
    {
        public int OfficeId { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string OfficeName { get; set; } = string.Empty;
        [Column(TypeName = "varchar(20)")]
        public string Location { get; set; } = string.Empty;
        public virtual ICollection<Department> Departments { get; set; }
        public Office()
        {
            Departments = new HashSet<Department>();
        }
    }
}
