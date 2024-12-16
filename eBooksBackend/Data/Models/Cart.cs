using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; } 
        public User User { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
