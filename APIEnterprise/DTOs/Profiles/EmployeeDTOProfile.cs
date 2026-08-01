using APIEnterprise.Models;
using AutoMapper;

namespace APIEnterprise.DTOs.Profiles
{
    public class EmployeeDTOProfile : Profile
    {
        public EmployeeDTOProfile()
        {
            CreateMap<EmployeeModel, EmployeeDTO>().ReverseMap();
            CreateMap<DepartmentModel, DepartmentDTO>().ReverseMap();
        }
    }
}
