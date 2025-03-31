using System.Resources;
using System.Text.Json;
using AutoMapper;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;
using HotelManagementAPI.Entities.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Services;

public class ManagementService
{
    private readonly HotelManagementContext _context;
    private readonly IMapper _mapper;

    public ManagementService(HotelManagementContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Dictionary<int, List<DateOnly>>> GetRoomBookingsAsync()
    {
        var reservations = await _context.Reservations
            .Where(r => 
                r.Status == EReservationStatus.Confirmed ||
                r.Status == EReservationStatus.Reserved)
            .ToListAsync();

        var roomBookings = new Dictionary<int, List<DateOnly>>();

        foreach (var reservation in reservations)
        {
            if (!roomBookings.ContainsKey(reservation.RoomNumber))
            {
                roomBookings[reservation.RoomNumber] = new List<DateOnly>();
            }

            var bookedDates = GetDatesBetween(reservation.CheckInDate, reservation.CheckOutDate);
            roomBookings[reservation.RoomNumber].AddRange(bookedDates);
        }

        return roomBookings;
    }

    private IEnumerable<DateOnly> GetDatesBetween(DateOnly startDate, DateOnly endDate)
    {
        var dates = new List<DateOnly>();
        var currentDate = startDate;

        while (currentDate < endDate)
        {
            dates.Add(currentDate);
            currentDate = currentDate.AddDays(1);
        }

        return dates;
    }

    public IEnumerable<DateTime> GetFullyBookedDates(Dictionary<int, List<DateOnly>> roomBookings)
    {
        var allDates = roomBookings.SelectMany(rb => rb.Value).Distinct();

        var fullyBookedDates = new List<DateTime>();

        foreach (var date in allDates)
        {
            bool isFullyBooked = roomBookings.All(rb => rb.Value.Contains(date));
            
            //if (isFullyBooked)
                fullyBookedDates.Add(date.ToDateTime(new TimeOnly(0, 0, 0, 0)));
        }

        return fullyBookedDates;
    }

    public async Task<UserReservationsWrapper> GetUserReservationsByDateAsync(DateOnly date, CancellationToken ct)
    {
        var reservations = await _context.Reservations.Where(r =>
            r.CheckInDate <= date && r.CheckOutDate > date &&
            (r.Status == EReservationStatus.Confirmed || r.Status == EReservationStatus.Reserved)
        ).Include(r => r.Guest).ToListAsync(ct);
       
        return await GetUsersByReservationsAsync(reservations, ct);
    }

    public async Task<UserReservationsWrapper> GetGuestsByCheckinDateAsync(DateOnly date, CancellationToken ct)
    {
        var reservations = await _context.Reservations.Where(r =>
            r.CheckInDate == date &&
            (r.Status == EReservationStatus.Confirmed || r.Status == EReservationStatus.Reserved)
        ).Include(r => r.Guest).ToListAsync(ct);
       
        return await GetUsersByReservationsAsync(reservations, ct);
    }

    public async Task<UserReservationsWrapper> GetAllUsersWithReservationsAsync(CancellationToken ct)
    {
        var guests = await _context.Guests
            .Include(g => g.Reservations)
            .Include(g => g.User)
            .ToListAsync(ct);
        var reservations = guests.SelectMany(g => g.Reservations).ToList();

        var usersDto = new List<UserDto>();
        foreach (var guest in guests)
        {
            if (guest.User == null)
                throw new NullReferenceException("Guest has no user");

            usersDto.Add(_mapper.Map<UserDto>(guest.User));
        }

        var reservationsDto = reservations.Select(r => _mapper.Map<ReservationDto>(r)).ToList();

        var UserReservation = new UserReservationsWrapper
        {
            Users = usersDto,
            Reservations = reservationsDto
        };
        
        return UserReservation;
    }

    private async Task<UserReservationsWrapper> GetUsersByReservationsAsync(List<Reservation> reservations,
        CancellationToken ct)
    {
        var userDtos = new List<UserDto>();
        var reservationDtos = new List<ReservationDto>();
        foreach (var reservation in reservations)
        {
            reservationDtos.Add(_mapper.Map<ReservationDto>(reservation));
           
            var u = await _context.Users.FindAsync([reservation.GuestEmail], ct);
            if (u == null)
                throw new NullReferenceException($"User with email {reservation.GuestEmail} was not found");
           
            var ud = _mapper.Map<UserDto>(u);
           
            if(!userDtos.Contains(ud)) 
                userDtos.Add(ud);
        }

        var userReservations = new UserReservationsWrapper
        {
            Users = userDtos.Distinct().ToList(),
            Reservations = reservationDtos
        };

        return userReservations;
    }

    public async Task<UserDto> GetGuestByEmailAsync(string email, CancellationToken ct)
    {
        var guest = await _context.Guests.FindAsync([email], ct);
        if(guest == null)
            throw new NullReferenceException($"Guest with email {email} was not found");
        
        var user = await _context.Users.FindAsync([email], ct);
        if(user == null)
            throw new NullReferenceException($"User with email {email} was not found");
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(string email, CancellationToken ct)
    {
        var reservations =
            await _context.Reservations.Where(r => r.GuestEmail == email).ToListAsync(ct);

        if (reservations.Count == 0)
            return new List<ReservationDto>();
        
        return reservations.Select(r => _mapper.Map<ReservationDto>(r));
    }

    public async Task<BookingWrapper> GetBookingAsync(string reservationId, CancellationToken ct)
    {
        var id = Guid.Parse(reservationId);
        if(id == Guid.Empty)
            throw new NullReferenceException("Reservation ID was not found");

        var reservation = await _context.Reservations
            .Include(r => r.Payment)
            .SingleAsync(r => r.Id == id, ct);
        
        if(reservation == null)
            throw new NullReferenceException($"Reservation with id {reservationId} was not found");
        
        var uniqueRoom = await _context.UniqueRooms.FindAsync([reservation.RoomType], ct);
        
        if(uniqueRoom == null)
            throw new NullReferenceException($"Room type {reservation.RoomType} was not found");
        
        var services = JsonSerializer.Deserialize<IEnumerable<ServiceDto>>(reservation.Services);
        
        if(services == null)
            throw new NullReferenceException($"Reservation service list was not found");

        var bookingWrapper = new BookingWrapper
        {
            Reservation = _mapper.Map<ReservationDto>(reservation),
            Payment = _mapper.Map<PaymentDto>(reservation.Payment),
            Services = services,
            Room = _mapper.Map<RoomDto>(uniqueRoom)
        };
        
        return bookingWrapper;
    }
}