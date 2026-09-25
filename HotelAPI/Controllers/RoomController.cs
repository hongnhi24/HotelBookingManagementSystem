using HotelAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly AppDbContext _context;

    public RoomController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/room/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = _context.Room
            .Include(r => r.RoomType)
            .FirstOrDefault(r => r.RoomID == id);

        if (room == null)
            return NotFound("Phòng không tồn tại");

        return Ok(room);
    }
}