using System.ComponentModel.DataAnnotations;
using APIEnterprise.Context;
using APIEnterprise.Models;
using APIEnterprise.Pagination;
using APIEnterprise.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APIEnterprise.Repositories
{
    public class EmployeeRepository : Repository<EmployeeModel>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<IPagedList<EmployeeModel>> GetAllEmployeesAsync(EmployeesParams pagParams)
        {
            var employees = await GetAllAsync();
            return await employees.ToPagedListAsync(pagParams.PageNumber, pagParams.PageSize);
        }

        public async Task<IPagedList<EmployeeModel>> GetEmployeesBirthDateFilterAsync(EmployeesBirthDateFilter employeesFilter)
        {
            var employees = await GetAllAsync();

            if(employeesFilter.BirthDate.HasValue && !string.IsNullOrEmpty(employeesFilter.Filter))
            {
                if (employeesFilter.Filter.Equals("maior", StringComparison.OrdinalIgnoreCase))
                    employees = employees.Where(e => e.BirthDate > employeesFilter.BirthDate).OrderBy(e => e.BirthDate);
                else if (employeesFilter.Filter.Equals("menor", StringComparison.OrdinalIgnoreCase))
                    employees = employees.Where(e => e.BirthDate < employeesFilter.BirthDate).OrderBy(e => e.BirthDate);
                else if (employeesFilter.Filter.Equals("igual", StringComparison.OrdinalIgnoreCase))
                    employees = employees.Where(e => e.BirthDate == employeesFilter.BirthDate).OrderBy(e => e.BirthDate);
                else
                    throw new ValidationException("Filtro inválido");
            }

            var employeeaFiltered = await employees.ToPagedListAsync(employeesFilter.PageNumber, employeesFilter.PageSize);

            return employeeaFiltered;
        }
    }
}
