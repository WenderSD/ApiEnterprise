using APIEnterprise.DTOs;
using APIEnterprise.Models;
using APIEnterprise.Pagination;
using APIEnterprise.Repositories.Interfaces;
using APIEnterprise.Services.Interfaces;
using Mapster;

namespace APIEnterprise.Services;

public class DepartmentService : BaseService<DepartmentModel>, IDepartmentService
{
    private readonly IUnitOfWork<DepartmentModel> _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;

    public DepartmentService(IUnitOfWork<DepartmentModel> unitOfWork,
        ICacheService cacheService,
        ILogger<DepartmentService> logger) : base(unitOfWork.Repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async Task<DepartmentDTO> GetDepartmentByIdAsync(Guid id)
    {
        var dept = await _cacheService.GetOrCreateAsync(
            $"department:{id}",
            async token => await GetFactory(id), 
            TimeSpan.FromMinutes(10)
        );

        var departmentDto = dept.Adapt<DepartmentDTO>();
        
        return departmentDto;
    }

    private async Task<DepartmentModel> GetFactory(Guid id)
    {
        _logger.LogInformation("Consultando o banco...");

        var dbDept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);

        return dbDept;
    }
}