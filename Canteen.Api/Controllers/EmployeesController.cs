using System.Globalization;
using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;

[AllowAnonymous] // TODO: Change to [Authorize] after testing
[ApiController]
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
    public async Task<IActionResult> GetEmployeesAsync()
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

    [HttpGet("cake/{id:int}")]
    public async Task<IActionResult> GetEmployeeCakeAsync(int id)
    {
        var employeeCake = await _context.EmployeeCakes.Where(empCake => empCake.EmployeeId == id && 
                                                                         empCake.Year == ISOWeek.GetYear(DateTime.Now) && 
                                                                         empCake.Number == ISOWeek.GetWeekOfYear(DateTime.Now))
                                                       .FirstAsync();
        var employeeCakeDto = _mapper.Map<EmployeeCakeDto>(employeeCake);

        return Ok(employeeCakeDto);
    }

    [HttpPost,Route("favourites")]
    public async Task<IActionResult> addFavouriteItem(int itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId))
            return BadRequest();

        var employee = await _context.Employees.FindAsync(employeeId);

        if (employee == null)
            return NotFound();

        employee.Items.Add(item);

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpDelete,Route("favourites")]
    public async Task<IActionResult> RemoveFavouriteItemAsync(int itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId))
            return BadRequest();

        var employee = await _context.Employees
            .Include(employee => employee.Items)
            .FirstAsync(employee => employee.EmployeeId == employeeId);

        if (employee == null)
            return NotFound();

        
        employee.Items.Remove(item);
        

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost,Route("cakes")]
    public async Task<IActionResult> addEmployeeCakeAsync(EmployeeCakeDto employeeCakeDto)
    {

        var employeeCake = _mapper.Map<EmployeeCake>(employeeCakeDto);
        await _context.EmployeeCakes.AddAsync(employeeCake);

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    
}