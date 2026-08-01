using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIEnterprise.Models
{
    public class DepartmentModel
    {
        public DepartmentModel()
        {
            Employees = new Collection<EmployeeModel>();
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public Guid? ManagerId { get; set; }
        [JsonIgnore]
        public ICollection<EmployeeModel>? Employees { get; set; }
        [JsonIgnore]
        public EmployeeModel? Manager { get; set; }
    }
}
