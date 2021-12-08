namespace Canteen.Dto;

public class EmployeeLunchDto
{
    public int EmployeeId { get; set; }
    public int LunchMenuId { get; set; }
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
}