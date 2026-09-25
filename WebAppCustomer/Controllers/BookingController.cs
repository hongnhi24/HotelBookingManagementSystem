using Microsoft.AspNetCore.Mvc;

using System.Text;
using System.Text.Json;
using WebAppCustomer.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;


public class BookingController : Controller
{
    private readonly HttpClient _httpClient;

    public BookingController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7256/");
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(
        string cartJson, string HoTen, string CCCD,
        string SoDienThoai, string Email, string DiaChi, string QuocTich, string GioiTinh)
    {
        var cart = JsonSerializer.Deserialize<CartData>(cartJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (cart == null || cart.Rooms == null || cart.Rooms.Count == 0)
            return Content("Không có phòng nào trong giỏ");

        // BƯỚC 1: Lưu khách hàng
        var customerJson = JsonSerializer.Serialize(new { TenKH = HoTen, SDT = SoDienThoai, DiaChi, CCCD, Email, GioiTinh, QuocTich });
        var customerResponse = await _httpClient.PostAsync("api/customer/create",
            new StringContent(customerJson, Encoding.UTF8, "application/json"));

        if (!customerResponse.IsSuccessStatusCode)
            return Content("Lỗi lưu khách hàng: " + await customerResponse.Content.ReadAsStringAsync());

        var maKH = JsonSerializer.Deserialize<JsonElement>(await customerResponse.Content.ReadAsStringAsync())
                                 .GetProperty("maKH").GetString();

        // BƯỚC 2: Đặt từng phòng
        if (string.IsNullOrEmpty(cart.CheckIn) || string.IsNullOrEmpty(cart.CheckOut))
            return Content("Vui lòng chọn ngày nhận và trả phòng");
        var checkIn = DateTime.Parse(cart.CheckIn);
        var checkOut = DateTime.Parse(cart.CheckOut);
        var bookingIDs = new List<int>();
        decimal totalPrice = 0, totalDeposit = 0; 

        foreach (var room in cart.Rooms)
        {
            var bookingJson = JsonSerializer.Serialize(new
            {
                RoomID = room.RoomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                SoNguoi = room.SoNguoi,
                MaKH = maKH
            });

            var bookingResponse = await _httpClient.PostAsync("api/booking/create",
                new StringContent(bookingJson, Encoding.UTF8, "application/json"));

            if (!bookingResponse.IsSuccessStatusCode)
                return Content($"Lỗi đặt phòng {room.RoomName}: " + await bookingResponse.Content.ReadAsStringAsync());

            var result = JsonSerializer.Deserialize<JsonElement>(await bookingResponse.Content.ReadAsStringAsync());
            bookingIDs.Add(result.TryGetProperty("bookingID", out var bid) ? bid.GetInt32() : 0);
            totalPrice += result.TryGetProperty("totalPrice", out var tp) ? tp.GetDecimal() : 0;
            totalDeposit += result.TryGetProperty("deposit", out var dep) ? dep.GetDecimal() : 0;
        }

        // BƯỚC 3: Trang thành công
        ViewBag.HoTen = HoTen;
        ViewBag.SoDienThoai = SoDienThoai;
        ViewBag.MaKH = maKH;
        ViewBag.Email = Email;
        ViewBag.BookingIDs = string.Join(", ", bookingIDs);
        ViewBag.SoPhong = cart.Rooms.Count;
        ViewBag.TenPhong = string.Join(", ", cart.Rooms.Select(r => r.RoomName));
        return View("Success", new Booking
        {
            BookingDate = DateTime.Now,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            TotalPrice = totalPrice,
            Deposit = totalDeposit
        });
    }
    // Trang nhập tìm kiếm
    [HttpGet]
    public IActionResult SearchHistory()
    {
        return View();
    }

    // Xử lý tìm kiếm
    [HttpPost]
    public async Task<IActionResult> SearchHistory(
        string cccd,
        string sdt)
    {
        var response = await _httpClient.GetAsync(
            $"api/booking/history/search?" +
            $"cccd={cccd}&sdt={sdt}");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<List<Booking>>(json, options);

        // Chuyển sang trang History
        return View("History", data);
    }
    public IActionResult Success() => View(); 
}


public class CartData
{
    public List<CartRoom> Rooms { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }
    public int Nights { get; set; }
}

public class CartRoom
{
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public string RoomType { get; set; }
    public decimal Price { get; set; }
    public int SoNguoi { get; set; }
}