using System.Globalization;
using System.Text.Json;
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
public class OrdersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly CanteenContext _context;

    public OrdersController(IMapper mapper, CanteenContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null) return NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync()
    {
        var orderDtos = await _context.Orders.ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();

        return Ok(orderDtos);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> PostOrderAsync([FromBody] JsonElement json)
    {
        var items = json.GetProperty("items").Deserialize<List<int>>();

        if (items == null || !items.Any()) return BadRequest();

        var employeeIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "id")!;

        if (!int.TryParse(employeeIdClaim.Value, out var employeeId)) return BadRequest();

        if (await _context.Employees.FindAsync(employeeId) == null) return null!;

        var order = new Order
        {
            EmployeeId = employeeId,
            Year = (short) DateTime.Now.Year,
            Number = (short) ISOWeek.GetWeekOfYear(DateTime.Today)
        };

        await _context.Orders.AddAsync(order).AsTask();
        await _context.SaveChangesAsync();

        var itemsByCount = items.GroupBy(itemId => itemId).ToDictionary(grouping => grouping.Key, grouping => grouping.Count());
        var orderItems = new List<OrderItem>();

        foreach (var (itemId, count) in itemsByCount)
            orderItems.Add(new OrderItem
            {
                Quantity = count,
                ItemId = itemId,
                OrderId = order.OrderId
            });

        await _context.OrderItems.AddRangeAsync(orderItems);
        await _context.SaveChangesAsync();

        return _mapper.Map<Order, OrderDto>(order);
    }
}