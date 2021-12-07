using AutoMapper;
using Canteen.DataAccess;
using Canteen.Dto;

namespace Canteen.Api;

public class CanteenProfile : Profile
{
    public CanteenProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategoryItemsDto>();
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dto => dto.Items, opt => opt.MapFrom(model => model.Items.Select(item => item.ItemId)));
        CreateMap<Item, ItemDto>().ReverseMap();
        CreateMap<LunchCancellation, LunchCancellationDto>();
        CreateMap<LunchMenu, LunchMenuDto>();
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
    }
}