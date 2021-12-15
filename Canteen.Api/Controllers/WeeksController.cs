using AutoMapper;
using AutoMapper.QueryableExtensions;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class WeeksController : ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;

    public WeeksController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeeks()
    {
        return Ok(await _context.Weeks.ToListAsync());
    }

    [HttpGet("{year}/{week}")]
    public async Task<IActionResult> GetWeek(int year, int week)
    {
        var weekFromDb = await _context.Weeks
            .ProjectTo<WeekDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(w => w.Year == year && w.Number == week);

        if (weekFromDb == null)
        {
            return NotFound();
        }

        return Ok(weekFromDb);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWeek([FromBody] WeekDto week)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var weekToCreate = _mapper.Map<Week>(week);


        var currentWeekNumber = await _context.Weeks.MaxAsync(w => w.Number);
        var currentWeek = await _context.Weeks
            .Where(w => w.Year == DateTime.Now.Year)
            .FirstOrDefaultAsync(w => w.Number == currentWeekNumber);

        if (!currentWeek.Finalized)
        {
            return BadRequest("Cannot create new week before current week is finalized");
        }

        _context.Weeks.Add(weekToCreate);
        await _context.SaveChangesAsync();
        return Ok(weekToCreate);
    }

    [HttpPost, Route("finalize")]
    public async Task<IActionResult> finalizeWeek([FromBody] WeekDto week)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var weekFromDb = await _context.Weeks.FirstOrDefaultAsync(w => week.Year == w.Year && w.Number == week.Number);
        if (weekFromDb == null)
        {
            return NotFound();
        }

        weekFromDb.Finalized = true;

        await _context.SaveChangesAsync();


        return Ok(weekFromDb);
    }
}