using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAppCustomer.Models;

public class RoomController : Controller
{
    private readonly HttpClient _httpClient;

    public RoomController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7256/");
    }

    public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int soKhach=1, int soPhong=1)
    {

        // Lưu lại giá trị để hiện lên form
        ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd");
        ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd");
        ViewBag.SoPhong = soPhong;
        var rooms = new List<Room>();

        if (checkIn != null && checkOut != null)
        {
           
            var response = await _httpClient.GetAsync(
                $"api/booking/available?checkIn={checkIn:yyyy-MM-ddTHH:mm:ss}&checkOut={checkOut:yyyy-MM-ddTHH:mm:ss}&soKhach={soKhach}&soPhong={soPhong}");
            var json = await response.Content.ReadAsStringAsync();

            //DEBUG: xem API trả gì
            Console.WriteLine(json);

            if (response.IsSuccessStatusCode)
            {
                rooms = JsonSerializer.Deserialize<List<Room>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                });
            }
            else
            {
                return Content("API lỗi: " + json);
            }
        }
        ViewBag.SoKhach = soKhach;
        ViewBag.SoPhong = soPhong;
        return View(rooms);
    }
}