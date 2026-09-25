using Microsoft.EntityFrameworkCore;
using HotelAPI;
using HotelAPI.Models;

namespace HotelAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Room> Room { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingDetail> BookingDetail { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
        public DbSet<Customer> Customer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapping table name nếu cần
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<BookingDetail>().ToTable("BookingDetail");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<RoomType>().ToTable("RoomType");

            modelBuilder.Entity<RoomType>().HasData(
       new RoomType { RoomTypeID = 1, TypeName = "Phòng đơn", Price = 400000, Description = "" },
       new RoomType { RoomTypeID = 2, TypeName = "Phòng đôi", Price = 600000, Description = "" },
       new RoomType { RoomTypeID = 4, TypeName = "Phòng gia đình", Price = 1000000, Description = "" },
       new RoomType { RoomTypeID = 3, TypeName = "Phòng VIP", Price = 1300000, Description = "" }
   );
        }
    }
}