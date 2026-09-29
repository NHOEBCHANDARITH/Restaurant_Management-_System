using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Models;

namespace Restaurant_Management__System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DiningTable> DiningTables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        protected override void OnModelCreating(
           ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Supplier>().ToTable("Suppliers");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");
            modelBuilder.Entity<Inventory>().ToTable("Inventories");
            modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<DiningTable>().ToTable("DiningTables");
            modelBuilder.Entity<Reservation>().ToTable("Reservations");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
            modelBuilder.Entity<Payment>().ToTable("Payments");
        }
    }
}
