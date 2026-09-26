using APIEnterprise.Context;
using APIEnterprise.Repositories.Interfaces;

namespace APIEnterprise.Repositories
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly IRepository<T>? _repository;
        private readonly IEmployeeRepository? _employeeRepository;
        private readonly IDepartmentRepository? _departmentRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository { get { return _repository ?? new Repository<T>(_context); } }
        public IEmployeeRepository EmployeeRepository { get { return _employeeRepository ?? new EmployeeRepository(_context); } }
        public IDepartmentRepository DepartmentRepository { get { return _departmentRepository ?? new DepartmentRepository(_context); } }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
