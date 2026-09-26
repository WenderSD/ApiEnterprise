using APIEnterprise.DTOs;
using APIEnterprise.Models;
using APIEnterprise.Pagination;

namespace APIEnterprise.Services.Interfaces;

public interface IDepartmentService : IBaseService<DepartmentModel>
{
    Task<DepartmentDTO> GetDepartmentByIdAsync(Guid id);
}