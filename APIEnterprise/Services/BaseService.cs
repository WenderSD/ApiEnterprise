using APIEnterprise.Pagination;
using APIEnterprise.Repositories.Interfaces;
using APIEnterprise.Services.Interfaces;

namespace APIEnterprise.Services;

public class BaseService<T> : IBaseService<T> where T :class
{
     private readonly IRepository<T> _repository;
     private readonly IUnitOfWork<T> _unitOfWork;
    public BaseService( IRepository<T> repository, IUnitOfWork<T> unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<T>> GetPaged(PaginationParams pagParams)
    {
        var models = await _unitOfWork.Repository.GetPagedAsync(pagParams);

        return models;
    }
}