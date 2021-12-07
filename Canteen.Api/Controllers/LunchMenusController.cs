using System.Globalization;
using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
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
    public async Task<ActionResult<LunchMenuDto>> GetCurrentMenu()
    {
        
        short currentYear = (short)DateTime.Now.Year;
        short currentWeek = (short) ISOWeek.GetWeekOfYear(DateTime.Now);
        var lunchMenu = await _context.LunchMenus
            .Include(menu => menu.LunchCancellations)
            .FirstOrDefaultAsync(menu => menu.Number == currentWeek && menu.Year == currentYear);
        
        var lunchMenuDto = _mapper.Map<LunchMenuDto>(lunchMenu);

        return Ok(lunchMenuDto);
    }
}

