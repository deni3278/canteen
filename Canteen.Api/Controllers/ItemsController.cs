using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    [HttpGet, Route("{active:bool?}")]
    public async Task<IActionResult> GetItemsAsync(bool active, [FromQuery] bool withImage = true)
    {
        IQueryable<Item> items = _context.Items.Include(item => item.Category);

        if (active)
            items = items.Where(item => item.Active);

        IEnumerable itemResult;

        if (!withImage)
            itemResult = await items.ProjectTo<ItemWithoutImageDto>(_mapper.ConfigurationProvider).ToListAsync();
        else
            itemResult = await items.ProjectTo<ItemDto>(_mapper.ConfigurationProvider).ToListAsync();

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
        var item = await _context.Items.ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .FirstAsync(dto => dto.ItemId == id);
        return Ok(item);
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

    [HttpGet, Route("{id}/image")]
    public async Task<ActionResult<byte[]>> GetItemImageAsync(int id)
    {
        var image = await _context.Items.Where(item => item.ItemId == id).Select(item => item.Image).FirstAsync();

        if (image == null)
            return NotFound();

        return File(image, "image/png");
    }

    [HttpPost, Route("images/get")]
    public async Task<ActionResult<List<byte[]>>> GetItemImagesAsync([FromBody] IEnumerable<int> ids)
    {
        var images = await _context.Items.Where(item => ids.Contains(item.ItemId)).Select(item => item.Image)
            .ToListAsync();

        if (images == null)
            return NotFound();

        return Ok(images);
    }
}