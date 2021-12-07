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
public class CategoriesController : ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        IEnumerable<Category> categories = await _context.Categories.ToListAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        
        if (categoryDtos == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(categoryDtos);
    }
}