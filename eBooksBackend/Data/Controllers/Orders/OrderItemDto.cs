namespace eBooksBackend.Data.Controllers.Orders
{
    public class OrderItemDTO
    {
        public int eBookId { get; set; }
        public string eBookTitle { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
