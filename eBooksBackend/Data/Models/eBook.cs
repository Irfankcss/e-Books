namespace eBooksBackend.Data.Models
{
    public class eBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Cover { get; set; }
        public string Path { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
