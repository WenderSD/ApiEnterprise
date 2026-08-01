using APIEnterprise.DTOs;
using APIEnterprise.Models;
using APIEnterprise.Repositories.Interfaces;
using APIEnterprise.Services.Interfaces;
using Mapster;

namespace APIEnterprise.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;

    public DepartmentService(IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<DepartmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async Task<DepartmentDTO> GetDepartmentByIdAsync(Guid id)
    {
        var dept = await _cacheService.GetOrCreateAsync<DepartmentModel>(
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

        await Task.Delay(3000, CancellationToken.None);

        var dbDept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);

        return dbDept;
    }
}