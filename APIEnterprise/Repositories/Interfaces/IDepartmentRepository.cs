using APIEnterprise.Models;
using APIEnterprise.Pagination;
using X.PagedList;

namespace APIEnterprise.Repositories.Interfaces
{
    public interface IDepartmentRepository : IRepository<DepartmentModel>
    {
        Task<EmployeeModel> GetManagerAsync(Guid departmentId);
        Task<IEnumerable<EmployeeModel>> GetByDeptAsync(Guid deptId);
        Task<IPagedList<DepartmentModel>> GetDepartmentsNameFilterAsync(DepartmentNameFilter deptFilter);
    }
}
