using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; } 
        public Cart Cart { get; set; }
        [ForeignKey(nameof(eBook))]
        public int eBookId { get; set; } 
        public eBook eBook { get; set; }
    }
}
