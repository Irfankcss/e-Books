namespace eBooksBackend.Data.Models
{
    public class eBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author {  get; set; }
        public double price { get; set; }
        public string Description { get; set; }
        public int Year {  get; set; }
    }
}
