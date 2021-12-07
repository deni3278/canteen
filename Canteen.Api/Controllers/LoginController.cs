using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Canteen.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Canteen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CanteenContext _context;

    public LoginController(IConfiguration configuration, CanteenContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] JsonElement json)
    {
        if (!json.TryGetProperty("password", out var passwordProperty))
            return BadRequest();

        var password = passwordProperty.GetString();

        if (password == null)
            return BadRequest();

        var employee = await _context.Employees.Where(employee => employee.Password.Equals(password))
            .FirstOrDefaultAsync();

        if (employee == null)
            return Unauthorized();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Claims = new Dictionary<string, object> {["id"] = employee.EmployeeId}
        });
        var jwt = new {token = tokenHandler.WriteToken(securityToken)};

        return Ok(jwt);
    }
}