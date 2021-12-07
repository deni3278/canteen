namespace Canteen.Dto;

public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public int[] Items { get; set; } = null!;
}