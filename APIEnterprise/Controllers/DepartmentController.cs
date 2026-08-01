using System.Text.Json;
using APIEnterprise.DTOs;
using APIEnterprise.Filters;
using APIEnterprise.Models;
using APIEnterprise.Pagination;
using APIEnterprise.Repositories.Interfaces;
using APIEnterprise.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace APIEnterprise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IUnitOfWork unitOfWork,  IDepartmentService departmentService)
        {
            _unitOfWork = unitOfWork;
            _departmentService = departmentService;
        }
        [HttpGet(Name = "departments")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                var departmentsDto = departments.Adapt<IEnumerable<DepartmentDTO>>();

                return Ok(departmentsDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("pagination" ,Name = "departments-pagination")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartmentsPagination([FromQuery] DepartmentParams pagParams)
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetAllDepartmentsAsync(pagParams);
                var departmentsDto = departments.Adapt<IEnumerable<DepartmentDTO>>();
                
                return Ok(departmentsDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("pagination/filter/name", Name = "departments-name filter")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<List<DepartmentDTO>>> GetDepartmentsNameFilter([FromQuery] DepartmentNameFilter deptFilter)
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetDepartmentsNameFilterAsync(deptFilter);
                IEnumerable<DepartmentDTO> departmentsDto = GetDepartmentsDtoWithMetaData(departments);

                return Ok(departmentsDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<DepartmentDTO>> GetById(Guid id)
        {
            try
            {
                var departmentDto = await _departmentService.GetDepartmentByIdAsync(id);

                return Ok(departmentDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("{id:Guid}/manager")]
        public async Task<ActionResult<EmployeeDTO>> GetManager(Guid id)
        {
            try
            {
                var manager = await _unitOfWork.DepartmentRepository.GetManagerAsync(id);

                var managerDto = manager.Adapt<EmployeeDTO>();

                return Ok(managerDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }


        [HttpGet("{deptId:Guid}/employees")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployeesByDept(Guid deptId)
        {
            try
            {
                var employeesDept = await _unitOfWork.DepartmentRepository.GetByDeptAsync(deptId);

                var employeesDeptDto = employeesDept.Adapt<IEnumerable<EmployeeDTO>>();

                return Ok(employeesDeptDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<DepartmentDTO>> CreateDepartment(DepartmentDTO departmentDtoRequest)
        {
            try
            {
                var departmentRequest = departmentDtoRequest.Adapt<DepartmentModel>();

                var dept = _unitOfWork.DepartmentRepository.Create(departmentRequest);
                await _unitOfWork.CommitAsync();

                var deptDto = dept.Adapt<DepartmentDTO>();

                return new CreatedAtRouteResult("getdepartments", new { id = deptDto.Id }, deptDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<DepartmentDTO>> UpdateDepartment(Guid id, DepartmentDTO departmentDtoRequest)
        {
            try
            {
                if (departmentDtoRequest.Id != id)
                    throw new ArgumentNullException("Entidade não encontrada");

                var departmentRequest = departmentDtoRequest.Adapt<DepartmentModel>();

                var dept = _unitOfWork.DepartmentRepository.Update(departmentRequest);
                await _unitOfWork.CommitAsync();

                var deptDto = dept.Adapt<DepartmentDTO>();

                return Ok(deptDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<DepartmentDTO>> DeleteDepartment(Guid id)
        {
            try
            {
                var dept = await _unitOfWork.DepartmentRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();

                var deptDto = dept.Adapt<DepartmentDTO>();  

                return Ok(dept);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
        }

        private IEnumerable<DepartmentDTO> GetDepartmentsDtoWithMetaData(IPagedList<DepartmentModel> departments)
        {
            var metadata = new
            {
                departments.Count,
                departments.PageSize,
                departments.PageCount,
                departments.TotalItemCount,
                departments.HasNextPage,
                departments.HasPreviousPage,
            };
            
            var departmentsDto = departments.Adapt<IEnumerable<DepartmentDTO>>();
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return departmentsDto;
        }
    }
}
