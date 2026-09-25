using HotelAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAPI.Models
{
    [Table("RoomType")]
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        public string? TypeName { get; set; }
        public decimal Price {  get; set; }
        public string? Description { get; set; }
        public int MaxGuests { get; set; }
        public decimal? FirstHourPrice { get; set; }  
        public decimal? NextHourPrice { get; set; }
    }
}
