using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Data
{
    public class HotelContext : DbContext
    {
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrderedService> OrderedServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Cleaning> Cleanings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Database=HotelManagement;Username=postgres;Password=25250825");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Определение связей между таблицами для соответствия pgAdmin схеме
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Guest)
                .WithMany(g => g.Bookings)
                .HasForeignKey(b => b.GuestId);
                
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomNumber);
                
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId);
                
            modelBuilder.Entity<Cleaning>()
                .HasOne(c => c.Room)
                .WithMany(r => r.Cleanings)
                .HasForeignKey(c => c.RoomNumber);
                
            modelBuilder.Entity<Cleaning>()
                .HasOne(c => c.Staff)
                .WithMany(s => s.Cleanings)
                .HasForeignKey(c => c.StaffId);
                
            modelBuilder.Entity<OrderedService>()
                .HasOne(o => o.Guest)
                .WithMany(g => g.OrderedServices)
                .HasForeignKey(o => o.GuestId);
                
            modelBuilder.Entity<OrderedService>()
                .HasOne(o => o.Service)
                .WithMany(s => s.OrderedServices)
                .HasForeignKey(o => o.ServiceId);
        }
    }
}