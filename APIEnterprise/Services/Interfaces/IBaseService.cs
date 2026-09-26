using APIEnterprise.Pagination;

namespace APIEnterprise.Services.Interfaces;

public interface IBaseService<T> where T : class
{
    Task<IEnumerable<T>> GetPaged(PaginationParams pagParams);
};