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
public class ItemsController : ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;

    public ItemsController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemsAsync()
    {
        IEnumerable<Item> items = await _context.Items
            .Include(item => item.Category)
            .Where(item => item.Active == true)
            .ToListAsync();
        var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);
        
        if (itemDtos == null)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        return Ok(itemDtos);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetItemsByCategoryIdAsync(int categoryId)
    {
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        
        return Ok(_mapper.Map<ItemDto>(item));
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteItemAsync(ItemIdDto itemIdDto)
    {
        if (itemIdDto.ItemId == null)
            return BadRequest();

        var item = await _context.Items.FindAsync(itemIdDto.ItemId);

        if (item == null)
            return BadRequest();

        item.Active = false;
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateItemAsync(ItemDto itemDto)
    {
        if (itemDto == null)
            return BadRequest();

        var item = _mapper.Map<Item>(itemDto);
        
        if (item == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        item.Category = null;
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}