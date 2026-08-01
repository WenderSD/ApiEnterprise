namespace APIEnterprise.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        Task CommitAsync();
        Task DisposeAsync();
    }
}
