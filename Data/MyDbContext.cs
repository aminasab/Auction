using Microsoft.EntityFrameworkCore;
using OnlineAuction.Models;

namespace OnlineAuction.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Mandarin> Mandarins { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mandarin>().ToTable("Mandarins");
        }
    }
}
