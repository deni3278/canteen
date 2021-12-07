using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<LunchMenu, LunchMenuDto>();
        CreateMap<LunchCancellation, LunchCancellationDto>();
        
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dto => dto.Items,opt => opt.MapFrom(employee => employee.Items.Select(item => item.ItemId)))
            .ReverseMap();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryItemsDto>().ReverseMap();
        CreateMap<Item, ItemDto>().ReverseMap();
    }
}