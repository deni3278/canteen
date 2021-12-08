using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dto => dto.Items, opt => opt.MapFrom(model => model.Items.Select(item => item.ItemId)));
        CreateMap<EmployeeLunch, EmployeeLunchDto>().ReverseMap();
        CreateMap<Item, ItemDto>().ReverseMap();
        CreateMap<LunchCancellation, LunchCancellationDto>().ReverseMap();
        CreateMap<LunchMenu, LunchMenuDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}