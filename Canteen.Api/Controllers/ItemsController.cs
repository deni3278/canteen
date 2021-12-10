using System.Collections;
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

    [HttpGet,Route("{active:bool?}")]
    public async Task<IActionResult> GetItemsAsync(bool active,[FromQuery] bool withImage = true)
    {
        
        var items = _context.Items
            .Include(item => item.Category);


        IEnumerable itemResult;
        
        if (active)
            itemResult = items.Where(item => item.Active);
        else
        {
            itemResult = items;
        }

        if (!withImage)
            itemResult = await items.Select(item => new ItemWithoutImageDto
            {
                Active = item.Active,
                Category = _mapper.Map<CategoryDto>(item.Category),
                Name = item.Name,
                Price = item.Price,
                CategoryId = item.CategoryId,
                ItemId = item.ItemId
            }).ToListAsync();

        else
        {
            itemResult = await items.ToListAsync();
            itemResult = _mapper.Map<IEnumerable<ItemDto>>(itemResult);
        }
        
        
        if (itemResult == null)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        return Ok(itemResult);
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
    
    [HttpGet,Route("{id}/image")]
    public async Task<ActionResult<byte[]>> GetItemImageAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
            return NotFound();

        return File(item.Image, "image/png");
    }
    
    [HttpPost,Route("images/get")]
    public async Task<ActionResult<List<byte[]>>> GetItemImagesAsync([FromBody] IEnumerable<int> ids)
    {
        var items = await _context.Items.Where(item => ids.Contains(item.ItemId)).ToListAsync();

        if (items == null)
            return NotFound();

        var images = items.Select(item => item.Image).ToList();
        
        return Ok(images);
    }
  
}