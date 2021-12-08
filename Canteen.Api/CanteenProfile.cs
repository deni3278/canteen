using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<EmployeeLunch, EmployeeLunchDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<LunchMenu, LunchMenuDto>().ReverseMap();
        CreateMap<LunchCancellation, LunchCancellationDto>().ReverseMap();
        
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dto => dto.Items, opt => opt.MapFrom(model => model.Items.Select(item => item.ItemId)));
        CreateMap<Item, ItemDto>().ReverseMap();
    }
}