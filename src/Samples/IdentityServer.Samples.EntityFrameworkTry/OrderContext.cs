using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Samples.EntityFrameworkTry
{
    public class OrderContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=EFTry;User Id=sa;Password=Password1");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailID);
                entity.HasIndex(e => e.OrderID);
                entity.Property(e => e.OrderDetailID).HasColumnName("OrderDetailID");
                
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderID);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.Version).IsConcurrencyToken();
            });
            
            
            
            
            modelBuilder.Entity<Order>()
                .Property(b => b.OrderDate)
                .HasDefaultValueSql("CONVERT(date, GETDATE())");
            
            
        }
    }
}