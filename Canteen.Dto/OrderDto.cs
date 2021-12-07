namespace Canteen.Dto;

public class OrderDto
{
    public int OrderId { get; set; }
    public int EmployeeId { get; set; }
    public short Number { get; set; }
    public short Year { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; }
}