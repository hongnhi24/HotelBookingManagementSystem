using HotelAPI.Data;
using HotelAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // API: TÌM PHÒNG TRỐNG
    // =========================
    [HttpGet("available")]
    public IActionResult GetAvailable(DateTime checkIn, DateTime checkOut, int soKhach = 1, int soPhong=1)
    {
        try
        {
            var rooms = _context.Room
                .Include(r => r.RoomType)
                .Where(r =>
                    r.RoomType.MaxGuests >= soKhach && //  lọc theo số khách
                    !_context.BookingDetail.Any(bd =>
                        bd.RoomID == r.RoomID &&
                        _context.Booking.Any(b =>
                            b.BookingID == bd.BookingID &&
                            checkIn < b.CheckOutDate &&
                            checkOut > b.CheckInDate
                        )
                    )
                )
                .ToList();
            if (rooms.Count < soPhong)
            {
                return Ok(new List<Room>()); // trả về rỗng nếu không đủ phòng
            }
            return Ok(rooms);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    // =========================
    // API: ĐẶT PHÒNG
    // =========================
    [HttpPost("create")]
    public IActionResult CreateBooking([FromBody] CreateBookingDTO dto)
    {
        if (dto == null)
            return BadRequest("Dữ liệu không hợp lệ");

        // 1. Tạo Booking
        //  lấy giá phòng từ RoomType
        var room = _context.Room
            .Include(r => r.RoomType)
            .FirstOrDefault(r => r.RoomID == dto.RoomID);

        if (room == null)
            return BadRequest("Phòng không tồn tại");
        var totalDays = (decimal)(dto.CheckOutDate - dto.CheckInDate).Days;
       
        //  tính tiền
        var totalPrice = room.RoomType.Price * totalDays;

        //  tính cọc 50%
        var deposit = totalPrice * 0.5m;

        //  tạo booking
        var booking = new Booking
        {
            CustomerID = dto.MaKH,
            BookingDate = DateTime.Now,
            CheckInDate = dto.CheckInDate.Date.AddHours(14),
            CheckOutDate = dto.CheckOutDate.Date.AddHours(12),
            Status = "Chờ duyệt",
            NguonDat = "Online",
            Deposit = deposit,
            LoaiHinhThue = "Đêm"
        };


        _context.Booking.Add(booking);
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }

        // 2. Tạo BookingDetail
        var bookingDetail = new BookingDetail
        {
            BookingID = booking.BookingID,
            RoomID = dto.RoomID,
            SoNguoi = dto.SoNguoi,
            Price = totalPrice 
        };

        _context.BookingDetail.Add(bookingDetail);
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }

        return Ok(new
        {
            bookingID = booking.BookingID,
            deposit = booking.Deposit,
            totalPrice = totalPrice
        });

    }
    [HttpGet("history/search")]
    public IActionResult SearchHistory(
    string cccd,
    string sdt)
    {
        if (
            string.IsNullOrWhiteSpace(cccd) ||
            string.IsNullOrWhiteSpace(sdt)
        )
        {
            return BadRequest("Vui lòng nhập đầy đủ thông tin");
        }

        // Tìm khách
        var customer = _context.Customer
            .FirstOrDefault(c =>
                c.CCCD == cccd &&
                c.SDT == sdt);

        if (customer == null)
        {
            return NotFound("Thông tin khách hàng không đúng");
        }

        // Tìm booking đúng khách
        var booking = _context.Booking
            .Where(b =>b.CustomerID == customer.MaKH)
            .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
            .OrderByDescending(b => b.BookingDate)
            .ToList();

        if (booking.Count == 0)
        {
            return NotFound("Khách hàng chưa có lịch sử đặt phòng");
        }

        return Ok(booking);
    }
}