using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eBooksBackend.Data.Models
{
    public class eBookUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EbookId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public string Format { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("EbookId")]
        public eBook Ebook { get; set; }
    }
}
