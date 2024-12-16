using eBooksBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data
{
    public class eBookStoreDbContext : DbContext
    {
        public eBookStoreDbContext(DbContextOptions<eBookStoreDbContext> options) : base(options) { }
        public DbSet<eBook> eBook {  get; set; }
    }
}
