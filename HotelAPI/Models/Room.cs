using HotelAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Room")]
public class Room
{
    [Key]
    public int RoomID { get; set; }

    public string? RoomName { get; set; }

    public int RoomTypeID { get; set; }

    public string? Status { get; set; }

    public int Floor { get; set; }
    public RoomType? RoomType { get; set; }
    [JsonIgnore]
    public ICollection<BookingDetail> BookingDetails { get; set; }
}