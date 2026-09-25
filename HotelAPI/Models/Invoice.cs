using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Invoice")]
public class Invoice
{
    [Key]
    public int InvoiceID { get; set; }

    public int? BookingID { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    [ForeignKey("BookingID")]
    public Booking Booking { get; set; }
}