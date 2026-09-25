using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppCustomer.Models
{
    public class BookingDetail
    {
        [Key]
        public int BookingDetailID { get; set; }

        public int BookingID { get; set; }

        public int RoomID { get; set; }

        public decimal? Price { get; set; }

        public int? SoNguoi { get; set; }

        public decimal? Discount { get; set; }

        [ForeignKey("BookingID")]
        public Booking Booking { get; set; }

        [ForeignKey("RoomID")]
        public Room  Room { get; set; }
    }
}
