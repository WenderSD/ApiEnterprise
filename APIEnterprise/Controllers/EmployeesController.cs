using System.Text.Json;
using APIEnterprise.DTOs;
using APIEnterprise.Models;
using APIEnterprise.Pagination;
using APIEnterprise.Repositories.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;


namespace APIEnterprise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork<EmployeeModel> _UnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public EmployeesController(IUnitOfWork<EmployeeModel> unitOfWork, IMapper mapper, ILogger<EmployeesController> logger)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #region GET

        [HttpGet( Name = "getemployees")]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployeesNoPagination()
        {
            try
            {
                _logger.LogInformation("================= GET api/Employees =================");
                var employees = await _UnitOfWork.EmployeeRepository.GetAllAsync();

                var employeesDto = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

                return Ok(employeesDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployees([FromQuery] EmployeesParams pagParams)
        {
            try
            {
                _logger.LogInformation("================= GET api/Employees =================");
                var employees = await _UnitOfWork.EmployeeRepository.GetAllEmployeesAsync(pagParams);

                IEnumerable<EmployeeDTO> employeesDto = GetEmployeesWithMetadaData(employees);

                return Ok(employeesDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("filter/birthdate")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployeesBirthDateFilter([FromQuery] EmployeesBirthDateFilter employeesFilter)
        {
            try
            {
                _logger.LogInformation("================= GET api/Employees =================");
                var employees = await _UnitOfWork.EmployeeRepository.GetEmployeesBirthDateFilterAsync(employeesFilter);

                IEnumerable<EmployeeDTO> employeesDto = GetEmployeesWithMetadaData(employees);

                return Ok(employeesDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("{id:Guid}", Name = "getById")]
        public async Task<ActionResult<EmployeeDTO>> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation($"================= GET api/Employees/id = {id} =================");
                var employee = _UnitOfWork.EmployeeRepository.GetByIdAsync(id);

                var employeeDto = _mapper.Map<EmployeeDTO>(employee);

                return Ok(employeeDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        #endregion

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployees(EmployeeDTO employeeDtoRequest, CancellationToken cancellationToken)
        {
            try
            {
                var employeeRequest = _mapper.Map<EmployeeModel>(employeeDtoRequest);

                var employee = _UnitOfWork.EmployeeRepository.Create(employeeRequest, cancellationToken);
                await _UnitOfWork.CommitAsync();

                var employeeDto = _mapper.Map<EmployeeDTO>(employee);

                return new CreatedAtRouteResult("getemployees", new { id = employeeDto.Id }, employeeDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<EmployeeDTO>> UpdateEmployee(Guid id, EmployeeDTO employeeDtoRequest)
        {
            try
            {
                if (employeeDtoRequest.Id != id)
                    throw new ArgumentNullException("Entidade não encontrada");

                var employeeRequest = _mapper.Map<EmployeeModel>(employeeDtoRequest);

                var employee = _UnitOfWork.EmployeeRepository.Update(employeeRequest);
                await _UnitOfWork.CommitAsync();

                var employeeDto = _mapper.Map<EmployeeDTO>(employee);

                return Ok(employeeDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPatch("{id:Guid}/update-function")]
        public async Task<ActionResult<EmployeeDTO>> UpdateFunction(Guid id, EmployeeDTOUpdate employeeDtoRequest)
        {
            try
            {
                if (employeeDtoRequest is null)
                    throw new ArgumentNullException("Objeto nulo");

                var employeeUpdate = await _UnitOfWork.EmployeeRepository.GetByIdAsync(id);

                if(employeeDtoRequest.Function != null)
                    employeeUpdate.Function = employeeDtoRequest.Function;

                if (employeeDtoRequest.Salary != null)
                    employeeUpdate.Salary = employeeDtoRequest.Salary;

                if (employeeDtoRequest.DepartmentId != null)
                    employeeUpdate.DepartmentId = employeeDtoRequest.DepartmentId;

                var employee = _UnitOfWork.EmployeeRepository.Update(employeeUpdate);
                await _UnitOfWork.CommitAsync();

                var employeeDto = _mapper.Map<EmployeeDTO>(employee);

                return Ok(employeeDto);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<EmployeeDTO>> DeleteEmployee(Guid id)
        {
            try
            {
                var employee = await _UnitOfWork.EmployeeRepository.DeleteAsync(id);
                await _UnitOfWork.CommitAsync();

                var employeeDto = _mapper.Map<EmployeeDTO>(employee);

                return Ok(employeeDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
        }

        #region Métodos Auxiliares
        private IEnumerable<EmployeeDTO> GetEmployeesWithMetadaData(IPagedList<EmployeeModel> employees)
        {
            var metadata = new
            {
                employees.Count,
                employees.PageSize,
                employees.PageCount,
                employees.TotalItemCount,
                employees.HasNextPage,
                employees.HasPreviousPage,
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            return employeesDto;
            #endregion
        }
    }
}
