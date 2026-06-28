using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDBCompleteDemo.Models;
using MongoDBCompleteDemo.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        await _authRepository.AssignRoleAsync(userId, role);
        return Ok();
    }

    [HttpGet("check-role")]
    [Authorize]
    public async Task<IActionResult> CheckRole(string role)
    {
        var hasRole = await _authRepository.HasRoleAsync("your-user-id-here", role);
        return Ok(hasRole);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // In a real project, validate the user against the database.
        // For this demo, we are using a test account.
        if (loginDto.Username == "admin" && loginDto.Password == "A123456a")
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, loginDto.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyAtLeast32CharsLong"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        return Unauthorized("Invalid username or password.");
    }
}