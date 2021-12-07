namespace Canteen.Dto;

public class ItemDto
{
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public byte[] Image { get; set; } = null!;
    public CategoryDto Category { get; set; } = null!;
}