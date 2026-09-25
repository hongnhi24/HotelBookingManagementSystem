
namespace WebAppCustomer.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        public string RoomName { get; set; }

        public int RoomTypeID { get; set; }

        public string Status { get; set; }

        public RoomType RoomType { get; set; }
    }
}
