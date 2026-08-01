using System.ComponentModel.DataAnnotations;

namespace APIEnterprise.DTOs
{
    public class EmployeeDTOUpdate
    {
        [StringLength(50)]
        public string? Function { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
