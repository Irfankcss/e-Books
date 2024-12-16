using System.ComponentModel.DataAnnotations.Schema;

namespace eBooksBackend.Data.Models
{
    public class eBookCategory
    {
        public int Id { get; set; }
        [ForeignKey(nameof(eBook))]
        public int eBooksId {  get; set; }
        public eBook eBook { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
