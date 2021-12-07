using System.Text.Json;
using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Controllers;


[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ItemController: ControllerBase
{
    private readonly CanteenContext _context;
    private readonly IMapper _mapper;
    
    public ItemController(CanteenContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<List<ItemDto>>> GetItems()
    {
        var items = _context.Items.Include(item => item.Category);
        var itemDto = _mapper.Map<List<ItemDto>>(items);
        return Ok(itemDto);
    }

    [HttpGet("{categoryId:int}")]
    public async Task<ActionResult<List<ItemDto>>> GetItemsByCategoryId(int categoryId)
    {
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        
        return NoContent();
    }
    
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem([FromBody] JsonElement json)
    {

        return Ok();
    }

}