using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Canteen.Api.Controllers;
using Canteen.DataAccess;
using Canteen.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Canteen.Api.Tests;

public class Tests
{
    private CanteenContext _context { get; set; }
    private IMapper _mapper { get; set; }

    private ItemsController _ItemsController { get; set; }

    public Tests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<CanteenProfile>(); });
        _mapper = config.CreateMapper();
    }

    [SetUp]
    public void Setup()
    {
        _context = new CanteenContext(new DbContextOptionsBuilder<CanteenContext>().UseSqlServer(
                "Server=tcp:ralle.database.windows.net,1433;Initial Catalog=Canteen;Persist Security Info=False;User ID=rasmus234;Password=rasmuS123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .Options);
        _ItemsController = new ItemsController(_context, _mapper);
    }

    [Test]
    public async Task PostItemReturnsItem()
    {
        var item = new Item()
        {
            Active = false,
            CategoryId = 1,
            Name = "TestItem",
            Price = 100,
            Image = Array.Empty<byte>(),
        };

        var itemDto = _mapper.Map<ItemDto>(item);

        var result = await _ItemsController.CreateItemAsync(itemDto);

        Assert.IsInstanceOf<CreatedAtRouteResult>(result);
        
        var createdAtRouteResult = (CreatedAtRouteResult)result;

        var itemCreated = createdAtRouteResult.Value as Item;


        Assert.IsInstanceOf<Item>(itemCreated);
        Assert.AreEqual(item.Name, itemCreated.Name);
    }

    [Test]
    public async Task GetItemReturnsItem()
    {

        var itemId = 1;
        var result = await _ItemsController.GetItemByIdAsync(itemId);

        Assert.IsInstanceOf<OkObjectResult>(result);

        var okObjectResult = (OkObjectResult)result;

        var itemReturned = okObjectResult.Value as ItemDto;

        Assert.IsInstanceOf<ItemDto>(itemReturned);
        Assert.AreEqual(itemId, itemReturned.ItemId);
    }
    
    [Test]
    public async Task GetActiveItemsReturnsActiveItems()
    {
        var result = await _ItemsController.GetItemsAsync(true);

        Assert.IsInstanceOf<OkObjectResult>(result);

        var okObjectResult = (OkObjectResult)result;

        var itemsReturned = okObjectResult.Value as IEnumerable<ItemDto>;

        Assert.IsInstanceOf<IEnumerable<ItemDto>>(itemsReturned);
        
        Assert.True(itemsReturned.All(dto => dto.Active == true));
    }
    
    [Test]
    public async Task DeleteItemReturnsOk()
    {
        var response = await _ItemsController.CreateItemAsync(new ItemDto()
        {
            Active = true,
            CategoryId = 1,
            Image = Array.Empty<byte>(),
            Name = "TestItem",
            Price = 100,
        });
        
        var createdAtRouteResult = (CreatedAtRouteResult)response;
        
        
        var item = createdAtRouteResult.Value as Item;
        
        var itemId = new ItemIdDto(){ItemId = item.ItemId};
        
        Assert.NotNull(itemId);
        
        var result = await _ItemsController.DeleteItemAsync(itemId);

        Assert.IsInstanceOf<OkResult>(result);
    }
    
    [Test]
    public async Task GetItemImageReturnsImage()
    {
        var response = await _ItemsController.CreateItemAsync(new ItemDto()
        {
            Active = true,
            CategoryId = 1,
            Image = Array.Empty<byte>(),
            Name = "TestItem",
            Price = 100,
        });
        
        var createdAtRouteResult = (CreatedAtRouteResult)response;

        var item = createdAtRouteResult.Value as Item;

        var result = await _ItemsController.GetItemImageAsync(item.ItemId);
        
        Assert.IsInstanceOf<FileContentResult>(result.Result);
    }
    
    [TearDown]
    public void TearDown()
    {
    }
}