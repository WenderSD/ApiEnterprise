using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIEnterprise.Models
{
    public class EmployeeModel
    {
        public EmployeeModel()
        {
        }

        [Key]
        [Required]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public DateOnly BirthDate { get; set; }
        [StringLength(50)]
        public string? CorporateEmail { get; set; }
        [StringLength(50)]
        public string? Function { get; set; }
        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Salary { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
        [JsonIgnore]
        public DepartmentModel? Department { get; set; }
    }
}
