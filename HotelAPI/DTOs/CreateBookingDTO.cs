namespace HotelAPI.DTOs
{
    public class CreateBookingDTO
    {
        public int RoomID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int SoNguoi { get; set; }
       
        public string MaKH { get; set; }
    }
}
//DTO nhận dữ liệu từ frontend