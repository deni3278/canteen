using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<Category, CategoryItemsDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dto => dto.Items, opt
                => opt.MapFrom(model => model.Items.Select(item => item.ItemId))).ReverseMap();
        CreateMap<EmployeeLunch, EmployeeLunchDto>().ReverseMap();
        CreateMap<EmployeeCake, EmployeeCakeDto>().ReverseMap();
        CreateMap<Item, ItemDto>().ReverseMap();
        CreateMap<LunchCancellation, LunchCancellationDto>().ReverseMap();
        CreateMap<LunchMenu, LunchMenuDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<EmployeeCake, EmployeeCakeDto>().ReverseMap();
    }
}