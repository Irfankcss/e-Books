using eBooksBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data
{
    public class eBookStoreDbContext : DbContext
    {
        public eBookStoreDbContext(DbContextOptions<eBookStoreDbContext> options) : base(options) { }
        public DbSet<eBook> eBook {  get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<eBookCategory> eBookCategories { get; set; }

        public DbSet<OrderItem> orderItems { get; set; }
    }
}
