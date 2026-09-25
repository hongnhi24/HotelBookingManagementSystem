using HotelAPI.Data;
using HotelAPI.DTOs;
using HotelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/customer/create
    [HttpPost("create")]
    public IActionResult CreateCustomer([FromBody] CreateCustomerDTO dto)
    {
        if (dto == null)
            return BadRequest("Dữ liệu không hợp lệ");
       //yêu cầu nhập đủ thông tin
        if (
        string.IsNullOrWhiteSpace(dto.TenKH) ||
        string.IsNullOrWhiteSpace(dto.SDT) ||
        string.IsNullOrWhiteSpace(dto.DiaChi) ||
        string.IsNullOrWhiteSpace(dto.CCCD) ||
        string.IsNullOrWhiteSpace(dto.GioiTinh) ||
        string.IsNullOrWhiteSpace(dto.Email)||
        string.IsNullOrWhiteSpace(dto.QuocTich)
        )
        {
            return BadRequest("Vui lòng nhập đầy đủ thông tin!");
        }
        // CCCD phải đúng 12 số
        if (dto.CCCD.Length != 12 || !dto.CCCD.All(char.IsDigit))
        {
            return BadRequest("CCCD phải gồm đúng 12 số!");
        }
        // SĐT phải gồm đúng 10 số
        if (dto.SDT.Length != 10 || !dto.SDT.All(char.IsDigit))
        {
            return BadRequest("Số điện thoại phải gồm đúng 10 số!");
        }
        //check email
        try
        {
            var mail = new MailAddress(dto.Email);
        }
        catch
        {
            return BadRequest("Email không hợp lệ!");
        }
        // Tìm khách theo CCCD
        var customerByCCCD = _context.Customer
            .FirstOrDefault(c => c.CCCD == dto.CCCD);

        // Nếu CCCD đã tồn tại
        if (customerByCCCD != null)
        {
            // Nếu đúng cùng khách → dùng lại MaKH
            if (
                customerByCCCD.TenKH == dto.TenKH &&
                customerByCCCD.SDT == dto.SDT
            )
            {
                return Ok(new
                {
                    maKH = customerByCCCD.MaKH,
                    tenKH = customerByCCCD.TenKH
                });
            }

            // Khác thông tin nhưng trùng CCCD
            return BadRequest("CCCD này đã được đăng ký!");
        }

        // Kiểm tra SĐT trùng với người khác
        var sdtExists = _context.Customer
            .Any(c => c.SDT == dto.SDT);

        if (sdtExists)
            return BadRequest("Số điện thoại này đã được đăng ký!");
        //kiểm tra tính hợp lệ của email
       
        // Tự sinh MaKH dạng WKH001, WKH002, ...
        var lastCustomer = _context.Customer
            .OrderByDescending(c => c.MaKH)
            .FirstOrDefault();

        string newMaKH = "WKH001"; 

        if (lastCustomer != null)
        {
            var lastNum = int.Parse(lastCustomer.MaKH.Substring(3)); // lấy số ký tự
            newMaKH = "WKH" + (lastNum + 1).ToString("D3");
        }

        var customer = new Customer
        {
            MaKH = newMaKH,
            TenKH = dto.TenKH,
            SDT = dto.SDT,
            DiaChi = dto.DiaChi,
            CCCD = dto.CCCD,
            Email = dto.Email,
            GioiTinh = dto.GioiTinh,
            QuocTich = dto.QuocTich
        };

        _context.Customer.Add(customer);
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }

        return Ok(new { maKH = customer.MaKH, tenKH = customer.TenKH });
    }
}