using System.Security.Cryptography;
using System.Text.Json;
using Bogus;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;

namespace HotelManagementAPI.Services;

public class SeederService
{
    private readonly HotelManagementContext _context;
    private readonly CredentialsService _credentials;

    public SeederService(HotelManagementContext context, CredentialsService credentials)
    {
        _context = context;
        _credentials = credentials;
    }

    public async Task SeedAsync(CancellationToken ct)
    {

        var random = new Random();
        (string passwordHash, string passwordSalt) =
            _credentials.HashPassword(Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8)));

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Email, f => f.Internet.Email(uniqueSuffix: Guid.NewGuid().ToString()))
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.PhoneNumber, f => "+38" + f.Phone.PhoneNumber("0#########"))
            .RuleFor(u => u.BirthDate, f => DateOnly.FromDateTime(f.Date.Past(30, DateTime.Today.AddYears(-18))))
            .RuleFor(u => u.PasswordSalt, f => passwordSalt)
            .RuleFor(u => u.PasswordHash, f => passwordHash)
            .RuleFor(u => u.UserRole, f => EUserRole.Guest);

        var usersGuests = userFaker.Generate(3000);
        var usersReceptionists = userFaker.Generate(4).Select(u =>
        {
            u.UserRole = EUserRole.Receptionist;
            return u;
        }).ToList();
        var usersAdministrators = userFaker.Generate(2);

        var guests = usersGuests
            .Select(u => new Guest
            {
                Email = u.Email,
                History = new Faker().Lorem.Paragraph(),
                User = u
            }).ToList();

        var receptionists = usersReceptionists
            .Select(u => new Receptionist
            {
                Email = u.Email,
                User = u
            }).ToList();

        var administrators = usersAdministrators
            .Select(u => new Administrator
            {
                Email = u.Email,
                User = u
            }).ToList();

        var uniqueRooms = new List<UniqueRoom>
        {
            new()
            {
                RoomType = ERoomType.Single, Capacity = 1, Price = 100000, ImageUrl = "Uploads/Rooms/Single.jpg"
            },
            new()
            {
                RoomType = ERoomType.Inclusive, Capacity = 2, Price = 130000, ImageUrl = "Uploads/Rooms/Inclusive.jpg"
            },
            new()
            {
                RoomType = ERoomType.Double, Capacity = 2, Price = 250000, ImageUrl = "Uploads/Rooms/Double.jpg"
            },
            new()
            {
                RoomType = ERoomType.Deluxe, Capacity = 3, Price = 400000, ImageUrl = "Uploads/Rooms/Deluxe.jpg"
            },
            new()
            {
                RoomType = ERoomType.Apartment, Capacity = 5, Price = 800000, ImageUrl = "Uploads/Rooms/Apartment.jpg"
            }
        };

        var rooms = new List<Room>();
        for (int i = 1; i <= 60; i++)
        {
            var roomType = uniqueRooms[random.Next(uniqueRooms.Count)].RoomType;
            rooms.Add(new Room
            {
                Number = i,
                Floor = random.Next(1, 5),
                Type = roomType,
                Status = ERoomStatus.Available,
                UniqueRoom = uniqueRooms.First(r => r.RoomType == roomType)
            });
        }

        var services = new Faker<Service>()
            .RuleFor(s => s.Id, _ => Guid.NewGuid())
            .RuleFor(s => s.Name, f => f.Commerce.ProductName())
            .RuleFor(s => s.Description, f => f.Commerce.ProductDescription())
            .RuleFor(s => s.Price, f => f.Random.Int(5000, 30000))
            .Generate(5);

        var reservations = new List<Reservation>();
        var payments = new List<Payment>();
        foreach (var guest in guests)
        {
            var room = rooms[random.Next(rooms.Count)];
            var checkIn = DateOnly.FromDateTime(DateTime.Today.AddDays(random.Next(-300, 300)));
            var checkOut = checkIn.AddDays(random.Next(1, 7));
            var resId = Guid.NewGuid();

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                ReservationId = resId,
                Date = checkOut,
                Amount = room.UniqueRoom.Price * (long)
                    (checkOut.ToDateTime(new TimeOnly(0,0 )) - 
                     checkIn.ToDateTime(new TimeOnly(0, 0)))
                    .TotalDays + services.Take(2).Sum(s => s.Price),
                PaymentMethod = EPaymentMethod.CreditCard
            };

            var reservation = new Reservation
            {
                Id = resId,
                GuestEmail = guest.Email,
                RoomNumber = room.Number,
                RoomType = room.Type,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                Status = EReservationStatus.Confirmed,
                Services = JsonSerializer.Serialize(services.Take(2).ToList()),
                Guest = guest,
                Room = room,
                Payment = payment
            };
            
            payment.Reservation = reservation;

            payments.Add(payment);
            reservations.Add(reservation);
        }

        _context.Users.AddRange(usersGuests);
        await _context.SaveChangesAsync(ct);
        _context.Guests.AddRange(guests);
        _context.Receptionists.AddRange(receptionists);
        _context.Administrators.AddRange(administrators);
        await _context.SaveChangesAsync(ct);
        _context.UniqueRooms.AddRange(uniqueRooms);
        await _context.SaveChangesAsync(ct);
        _context.Rooms.AddRange(rooms);
        _context.Services.AddRange(services);
        await _context.SaveChangesAsync(ct);
        _context.Reservations.AddRange(reservations);
        _context.Payments.AddRange(payments);
        await _context.SaveChangesAsync(ct);

        Console.WriteLine("🎉 Database seeded!");
    }
}