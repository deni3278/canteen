using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Item, ItemDto>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)).ReverseMap();
    }
}