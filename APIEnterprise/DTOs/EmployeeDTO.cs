using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIEnterprise.DTOs
{
    public class EmployeeDTO
    {
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
        public decimal Salary { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
