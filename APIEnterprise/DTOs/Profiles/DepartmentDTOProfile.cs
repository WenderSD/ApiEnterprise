using APIEnterprise.Models;
using AutoMapper;

namespace APIEnterprise.DTOs.Profiles
{
    public class DepartmentDTOProfile : Profile
    {
        public DepartmentDTOProfile()
        {
            CreateMap<DepartmentModel, DepartmentDTO>().ReverseMap();
            CreateMap<EmployeeModel, EmployeeDTO>().ReverseMap();
        }
    }
}
