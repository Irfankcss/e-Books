using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey(nameof(eBook))]
        public int eBookId { get; set; }
        public eBook eBook { get; set; }

        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
