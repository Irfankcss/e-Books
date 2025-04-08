namespace eBooksBackend.Data.Controllers.Payments
{
    public class PaymentDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }
    }
}
