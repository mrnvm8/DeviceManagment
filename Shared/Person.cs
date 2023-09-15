using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagment.Shared
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string FirstName { get; set; } = string.Empty;
        [Column(TypeName = "varchar(20)")]
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public Person()
        {
            Employees = new HashSet<Employee>();
        }
    }
}
