using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Item, ItemDto>().ReverseMap();
    }
}