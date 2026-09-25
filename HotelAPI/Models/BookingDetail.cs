using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("BookingDetail")]
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
    [JsonIgnore]
    public Booking? Booking { get; set; }

    [ForeignKey("RoomID")]
    public Room? Room { get; set; }
}