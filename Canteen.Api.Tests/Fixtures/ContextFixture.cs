using System;
using Canteen.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Canteen.Api.Tests.Fixtures;

public class ContextFixture : IDisposable
{
    public CanteenContext Context { get; }
    
    public ContextFixture()
    {
        Context = new CanteenContext(new DbContextOptionsBuilder<CanteenContext>().UseInMemoryDatabase("Canteen").Options);
        
        Seed();
    }

    private void Seed()
    {
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        var employee = new Employee
        {
            FirstName = "John",
            LastName = "Doe",
            Password = "Test"
        };

        Context.Employees.Add(employee);
        Context.SaveChanges();
    }
    
    public void Dispose()
    {
        Context.Dispose();
    }
}