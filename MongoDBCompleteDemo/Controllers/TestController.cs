using Microsoft.AspNetCore.Mvc;
using MongoDBCompleteDemo.Data;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly MongoDbContext _context;

    public TestController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Successfully connected to MongoDB!");
    }
}