namespace APIEnterprise.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        T Create(T model, CancellationToken cancellationToken = default);
        T Update(T model);
        Task<T> DeleteAsync(Guid id);
    }
}
