using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class eBookRating
    {
        public int Id { get; set; }
        public float Rating { get; set; }
        public string Review { get; set; }
        [ForeignKey(nameof(eBookRating))]
        public int userID { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(eBook))]
        public int eBookId { get; set; }
        public eBook eBook { get; set; }
    }
}
