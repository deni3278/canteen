namespace Canteen.Dto;

public class LunchCancellationDto
{
    public int LunchCancellationId { get; set; }
    public int LunchMenuId { get; set; }
    public string? Message { get; set; }
}