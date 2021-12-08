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
public class LunchMenusController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly CanteenContext _context;

    public LunchMenusController(IMapper mapper, CanteenContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet, Route("current")]
    public async Task<ActionResult<LunchMenuDto>> GetCurrentMenuAsync()
    {
        var currentYear = (short) DateTime.Now.Year;
        var currentWeek = (short) ISOWeek.GetWeekOfYear(DateTime.Now);

        var lunchMenu = await _context.LunchMenus
            .Include(menu => menu.LunchCancellations)
            .FirstAsync(menu => menu.Number == currentWeek && menu.Year == currentYear);
        var lunchMenuDto = _mapper.Map<LunchMenuDto>(lunchMenu);

        if (lunchMenuDto == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(lunchMenuDto);
    }
    
    [HttpPost, Route("employeeLunch")]
    public async Task<ActionResult<EmployeeLunchDto>> PostEmployeeLunchAsync(EmployeeLunchDto employeeLunchDto)
    {
        var employeeLunch = _mapper.Map<EmployeeLunch>(employeeLunchDto);
        
        var existingEmployeeLunch = await _context.EmployeeLunches.FindAsync(employeeLunch.LunchMenuId,employeeLunch.EmployeeId);

        if (existingEmployeeLunch == null)
        {
           await _context.EmployeeLunches.AddAsync(employeeLunch); 
        }
        else
        {
            _context.Entry(existingEmployeeLunch).CurrentValues.SetValues(employeeLunch);
        }
        
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<EmployeeLunchDto>(employeeLunch));
    }
    
    [HttpGet,Route("employeeLunch/{employeeId}/{lunchMenuId}")]
    public async Task<ActionResult<EmployeeLunchDto>> GetEmployeeLunchAsync(int lunchMenuId, int employeeId)
    {
        var employeeLunch = await _context.EmployeeLunches.FindAsync(lunchMenuId, employeeId);

        if (employeeLunch == null)
            return NotFound();

        return Ok(_mapper.Map<EmployeeLunchDto>(employeeLunch));
    }
    
}