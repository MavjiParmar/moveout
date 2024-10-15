using BoxnMove.Database.DbModels;
using BoxnMove.Models.Models;
using BoxnMove.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxnMove.Database
{
    public class BoxnMoveDBContext : DbContext
    {
        private readonly ILogger<BoxnMoveDBContext> logger;
        public BoxnMoveDBContext() : base()
        {
        }
        public BoxnMoveDBContext(DbContextOptions<BoxnMoveDBContext> options, ILogger<BoxnMoveDBContext> _logger) : base(options)
        {
            logger = _logger;
        }

        public DbSet<OTPStore> OTPStores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CouponCode> CouponCodes { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }

        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users);

            modelBuilder.Entity<User>()
                  .HasIndex(u => u.UserName)
                  .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.MobileNumber)
            .IsUnique();


        }
    }
}
