using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(eBook))]
        public int eBookId { get; set; }
        public eBook eBook { get; set; }
    }
}
