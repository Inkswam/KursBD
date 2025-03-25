using HotelManagementAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Context;

public sealed class HotelManagementContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<UniqueRoom> UniqueRooms { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Blacklist> Blacklists { get; set; }

    public HotelManagementContext()
    {
        Database.EnsureCreated();
    }

    public HotelManagementContext(DbContextOptions<HotelManagementContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasOne(u => u.Administrator)
                .WithOne(a => a.User)
                .HasForeignKey<Administrator>(a => a.Email)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(u => u.Receptionist)
                .WithOne(a => a.User)
                .HasForeignKey<Receptionist>(a => a.Email)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity
                .HasOne(u => u.Guest)
                .WithOne(a => a.User)
                .HasForeignKey<Guest>(a => a.Email)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestEmail)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomNumber)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity
                .HasOne(r => r.Payment)
                .WithOne(p => p.Reservation)
                .HasForeignKey<Payment>(p => p.ReservationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity
                .HasOne(r => r.UniqueRoom)
                .WithMany(ur => ur.Rooms)
                .HasForeignKey(r => r.Type)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}