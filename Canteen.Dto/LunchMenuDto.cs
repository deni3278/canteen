namespace Canteen.Dto;

public class LunchMenuDto
{
    public int LunchMenuId { get; set; }
    public short Number { get; set; }
    public short Year { get; set; }
    public int? MondayItemId { get; set; }
    public int? TuesdayItemId { get; set; }
    public int? WednesdayItemId { get; set; }
    public int? ThursdayItemId { get; set; }
    public int? FridayItemId { get; set; }
    public ICollection<LunchCancellationDto> LunchCancellations { get; set; }
}