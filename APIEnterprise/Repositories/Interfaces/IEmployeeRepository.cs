using APIEnterprise.Models;
using APIEnterprise.Pagination;
using X.PagedList;

namespace APIEnterprise.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<EmployeeModel>
    {
        Task<IPagedList<EmployeeModel>> GetAllEmployeesAsync(EmployeesParams paginationParams);
        Task<IPagedList<EmployeeModel>> GetEmployeesBirthDateFilterAsync(EmployeesBirthDateFilter employeesFilter);
    }
}
