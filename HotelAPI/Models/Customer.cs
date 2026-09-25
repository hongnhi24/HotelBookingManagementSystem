using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAPI.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public string? MaKH { get; set; }

        [Required]
        public string? TenKH { get; set; }

        public string? SDT { get; set; }

        public string? DiaChi { get; set; }

        public string? CCCD { get; set; }

        public string? GioiTinh { get; set; }

        public string? QuocTich { get; set; }
        public string? Email { get; set; }
    }
}