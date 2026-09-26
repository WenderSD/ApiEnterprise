namespace APIEnterprise.Repositories.Interfaces
{
    public interface IUnitOfWork<T> where T : class
    {
        IRepository<T> Repository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        Task CommitAsync();
        Task DisposeAsync();
    }
}
