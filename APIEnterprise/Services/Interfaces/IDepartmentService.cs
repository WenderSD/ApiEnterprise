using APIEnterprise.DTOs;
using APIEnterprise.Models;

namespace APIEnterprise.Services.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentDTO> GetDepartmentByIdAsync(Guid id);
}