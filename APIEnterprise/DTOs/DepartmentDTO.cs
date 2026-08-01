using System.ComponentModel.DataAnnotations;

namespace APIEnterprise.DTOs
{
    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
