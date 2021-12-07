using System.Collections;
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
    public async Task<IActionResult> GetCategories(bool includeItems)
    {
        var categoriesSet = _context.Categories;
        IEnumerable<Category> categories = includeItems
            ? await categoriesSet.Include(category => category.Items).ToListAsync()
            : await categoriesSet.ToListAsync();

        IEnumerable dtos;

        if (includeItems)
        {
            dtos = _mapper.Map<IEnumerable<CategoryItemsDto>>(categories);
        }
        else
        {
            dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        if (dtos == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(dtos);
    }
}