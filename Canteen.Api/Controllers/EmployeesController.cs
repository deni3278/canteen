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
}