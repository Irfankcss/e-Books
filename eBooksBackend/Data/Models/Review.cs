using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; } 
        public User User { get; set; }
        [ForeignKey(nameof(eBook))]
        public int eBookId { get; set; }
        public eBook eBook { get; set; }
    }
}
