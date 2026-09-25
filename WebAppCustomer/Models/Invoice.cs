namespace WebAppCustomer.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }

        public int BookingID { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
