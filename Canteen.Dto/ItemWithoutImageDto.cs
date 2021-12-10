namespace Canteen.Dto;

public class ItemWithoutImageDto
{
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public bool Active { get; set; }
}