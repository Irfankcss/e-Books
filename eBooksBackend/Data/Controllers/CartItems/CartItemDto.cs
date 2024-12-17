namespace eBooksBackend.Data.Controllers.CartItems
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int eBookId { get; set; }
        public string eBookTitle { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
