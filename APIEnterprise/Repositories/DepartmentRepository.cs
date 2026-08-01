using APIEnterprise.Context;
using APIEnterprise.Models;
using APIEnterprise.Pagination;
using APIEnterprise.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APIEnterprise.Repositories
{
    public class DepartmentRepository : Repository<DepartmentModel>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext appDbContext) : base(appDbContext)
        {            
        }
        public async Task<EmployeeModel> GetManagerAsync(Guid departmentId)
        {
            var dept = _appDbContext.Departments.Include(d => d.Manager).FirstOrDefault(d => d.Id == departmentId);

            if (dept == null)
                throw new ArgumentNullException("Departamento não encontrado");
            var manager = dept.Manager;

            return manager;
        }

        public async Task<IEnumerable<EmployeeModel>> GetByDeptAsync(Guid deptId)
        {
            var dept = await _appDbContext.Departments.AsNoTracking().Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == deptId);

            var employeesDept = dept.Employees.ToList();

            if (employeesDept == null)
                throw new ArgumentNullException("Entidade não encontrada");

            return employeesDept;
        }

        public async Task<IPagedList<DepartmentModel>> GetAllDepartmentsAsync(DepartmentParams pagParams)
        {
            var depts = await GetAllAsync();
            return await depts.ToPagedListAsync(pagParams.PageNumber, pagParams.PageSize);
        }

        public async Task<IPagedList<DepartmentModel>> GetDepartmentsNameFilterAsync(DepartmentNameFilter deptFilter)
        {
            var depts = await GetAllAsync();

            if(!string.IsNullOrEmpty(deptFilter.Name))
                depts = depts.Where(d => d.Name.Contains(deptFilter.Name, StringComparison.OrdinalIgnoreCase));

            return await depts.ToPagedListAsync(deptFilter.PageNumber, deptFilter.PageSize);
        }
    }
}
