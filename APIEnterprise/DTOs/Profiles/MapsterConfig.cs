using APIEnterprise.Models;
using Mapster;

namespace APIEnterprise.DTOs.Profiles
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EmployeeModel, EmployeeDTO>();
            config.NewConfig<DepartmentModel, DepartmentDTO>();
        }
    }
}
