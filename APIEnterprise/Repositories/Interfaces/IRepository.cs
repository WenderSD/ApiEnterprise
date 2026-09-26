using APIEnterprise.Pagination;
using X.PagedList;

namespace APIEnterprise.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IPagedList<T>> GetPagedAsync(PaginationParams pagParams);
        Task<T> GetByIdAsync(Guid id);
        T Create(T model, CancellationToken cancellationToken = default);
        T Update(T model);
        Task<T> DeleteAsync(Guid id);
    }
}
