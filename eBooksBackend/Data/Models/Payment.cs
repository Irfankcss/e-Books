using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } 
        public DateTime PaymentDate { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
