
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Booking")]
public class Booking
{
    [Key]
    public int BookingID { get; set; }

    public string? CustomerID { get; set; }

    public DateTime? BookingDate { get; set; }

    public DateTime? CheckInDate { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public string? Status { get; set; }

    public int? EmployeeID { get; set; }
 
    public decimal? Deposit { get; set; }

    public string? NguonDat { get; set; }

    public string? Note { get; set; }
    public string? LoaiHinhThue { get; set; }
    public string? PaymentStatus { get; set; }

    public ICollection<BookingDetail>? BookingDetails { get; set; }
   
}