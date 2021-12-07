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
}