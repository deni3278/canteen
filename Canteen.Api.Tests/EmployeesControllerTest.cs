using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Canteen.Api.Controllers;
using Canteen.Api.Tests.Fixtures;
using Canteen.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Canteen.Api.Tests;

public class EmployeesControllerTest : IClassFixture<ContextFixture>
{
    private readonly ContextFixture _contextFixture;
    private readonly EmployeesController _controller;

    public EmployeesControllerTest(ContextFixture contextFixture)
    {
        _contextFixture = contextFixture;

        var employees = _contextFixture.Context.Employees.AsEnumerable();
        var mapper = new Mock<IMapper>();
        
        mapper.Setup(x => x.Map<IEnumerable<EmployeeDto>>(employees)).Returns(new List<EmployeeDto>
        {
            new()
            {
                EmployeeId = employees.First().EmployeeId,
                FirstName = employees.First().FirstName,
                LastName = employees.First().LastName,
                Password = employees.First().Password
            }
        }.AsEnumerable());
        
        _controller = new EmployeesController(contextFixture.Context, mapper.Object);
    }

    [Fact]
    public async Task GetEmployees_Exists_ReturnDto()
    {
        var employees = _contextFixture.Context.Employees.AsEnumerable();
        var employeeDtos = (await _controller.GetEmployees() as OkObjectResult).Value as IEnumerable<EmployeeDto>;
        
        Assert.Equal(employees.First().FirstName, employeeDtos.First().FirstName);
    }
}