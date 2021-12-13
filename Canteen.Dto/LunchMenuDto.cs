using Canteen.DataAccess;

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
    
    public LunchCancellationDto? MondayLunchCancellation { get; set; }
    public LunchCancellationDto? FridayLunchCancellation { get; set; }
    public LunchCancellationDto? ThursdayLunchCancellation { get; set; }
    public LunchCancellationDto? TuesdayLunchCancellation { get; set; }
    public LunchCancellationDto? WednesdayLunchCancellation { get; set; }
    
}