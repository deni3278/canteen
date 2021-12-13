namespace Canteen.Dto;

public class CategoryItemsDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<ItemDto> Items { get; set; } = null!;
}