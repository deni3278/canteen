using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;

    public EmployeesController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        IEnumerable<Employee> employees = await _context.Employees.ToListAsync();
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        if (employeeDtos == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(employeeDtos);
    }

    [HttpGet("token")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeFromJwtAsync()
    {
        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId))
            return BadRequest();

        var employee = await _context.Employees
            .Include(employee => employee.Items)
            .FirstOrDefaultAsync(employee1 => employee1.EmployeeId == employeeId);

        if (employee == null)
            return NotFound();

        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return Ok(employeeDto);
    }
}