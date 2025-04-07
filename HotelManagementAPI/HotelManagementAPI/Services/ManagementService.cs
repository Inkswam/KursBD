using System.Resources;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
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

        var blacklist = await _context.Blacklists.ToListAsync(ct);
        var usersDto = new List<UserDto>();
        foreach (var guest in guests)
        {
            if (guest.User == null)
                throw new NullReferenceException("Guest has no user");
            
            if(blacklist.All(b => b.Email != guest.Email))
                usersDto.Add(_mapper.Map<UserDto>(guest.User));
        }

        var reservationsDto = reservations.Select(r => _mapper.Map<ReservationDto>(r)).ToList();

        var userReservation = new UserReservationsWrapper
        {
            Users = usersDto,
            Reservations = reservationsDto
        };
        
        return userReservation;
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


    public async Task<(byte[], string, string)> GetFinancialReportAsync(IEnumerable<DateOnly> dateRange, CancellationToken ct)
    {
        var dates = dateRange.ToList();
        if (dates.Count != 2)
            throw new BadHttpRequestException("Number of dates must be 2");

        var reservations = await _context.Reservations
            .Include(r => r.Payment)
            .Where(r => r.Payment != null && r.Payment.Date >= dates.First() && r.Payment.Date <= dates.Last()).ToListAsync(ct);

        
        var csv = new StringBuilder();
        csv.AppendLine("Id,GuestEmail,RoomNumber,CheckInDate,CheckOutDate,Status,PaymentMethod,PaymentDate,PaymentAmount");

        long totalAmount = 0;
        foreach (var res in reservations)
        {
            csv.AppendLine($"" +
                           $"{res.Id}," +
                           $"{res.GuestEmail}," +
                           $"{res.RoomNumber}," +
                           $"{res.CheckInDate}," +
                           $"{res.CheckOutDate}," +
                           $"{res.Status}," +
                           $"{res.Payment!.PaymentMethod}," +
                           $"{res.Payment!.Date}," +
                           $"{res.Payment!.Amount / 100}");
            
            totalAmount+= res.Payment!.Amount;
        }

        csv.AppendLine();
        csv.AppendLine($"Date range:,{dates.First()},{dates.Last()}");
        csv.AppendLine($"Total Amount:,{totalAmount / 100}");
        
        
        var fileName = $"FinancialReport_{dates.First():yyyyMMdd}_{dates.Last():yyyyMMdd}.csv";
        var fileBytes = Encoding.UTF8.GetBytes(csv.ToString());

        return (fileBytes, "text/csv", fileName);
    }

    public async Task<IEnumerable<int>> GetFreeRoomsAsync(DateOnly startDate, DateOnly endDate, string roomType, CancellationToken ct)
    {
        var type = Enum.Parse<ERoomType>(roomType);
        
        var rooms = await _context.Rooms
            .Include(r => r.Reservations)
            .Where(r => 
                (r.Reservations.Count == 0 || 
                !r.Reservations.Any(res => 
                    (res.CheckInDate >= startDate && res.CheckInDate < endDate) ||
                    (res.CheckOutDate > startDate && res.CheckOutDate <= endDate) ||
                    (res.CheckInDate <= startDate && res.CheckOutDate >= endDate)
                )) && r.Type == type
            ).Select(room => room.Number).ToListAsync(ct);
        
        return rooms;
    }

    public async Task<int> SetNewRoomAsync(string reservationId, int roomNumber, CancellationToken ct)
    {
        var id = Guid.Parse(reservationId);
        var reservation = await _context.Reservations.FindAsync([id], ct);
        
        if(reservation == null)
            throw new NullReferenceException($"Reservation with id {id} was not found");
        
        reservation.RoomNumber = roomNumber;
        
        _context.Update(reservation);
        await _context.SaveChangesAsync(ct);

        return roomNumber;
    }

    public ChartStatistic GetGuestChart(string period, CancellationToken ct)
    {
        
        var currentDate = DateOnly.FromDateTime(DateTime.Today);
        var startDate = currentDate;

        if (period == "Week")
        {
            startDate = currentDate.AddDays(-7);
        }
        else if (period == "Month")
        {
            startDate = currentDate.AddMonths(-1);
        }
        else if (period == "Year")
        {
            startDate = currentDate.AddYears(-1);
        }

        var chart = GetGuestsPerDate(startDate, currentDate);
        
        var comparisonDate = startDate.AddDays(startDate.DayNumber - currentDate.DayNumber);
        var oldStatistic = GetGuestsPerDate(comparisonDate, startDate);

        double newSum = 0;
        foreach (var c in chart)
        {
            newSum += c.Value;
        }

        double oldSum = 0;
        foreach (var os in oldStatistic)
        {
            oldSum += os.Value;
        }
        
        var percentage = oldSum == 0 ? 100 : newSum * 100.0 / oldSum - 100.0;
        
        var chartStatistic = new ChartStatistic
        {
            ChartsData = chart,
            ValueSum = newSum,
            Percentage = percentage
        };
        
        return chartStatistic;
    }

    private List<ChartData> GetGuestsPerDate(DateOnly startDate, DateOnly endDate)
    {
        
        var guestCountPerDate = _context.Reservations
            .AsEnumerable() // Switch to client-side for date iteration logic
            .SelectMany(res => Enumerable
                .Range(0, (res.CheckOutDate.DayNumber - res.CheckInDate.DayNumber))
                .Select(offset => new
                {
                    Date = res.CheckInDate.AddDays(offset),
                    GuestEmail = res.GuestEmail
                }))
            .Where(x => x.Date > startDate && x.Date <= endDate)
            .GroupBy(x => x.Date)
            .Select(g => new
            {
                Date = g.Key,
                GuestCount = g.Select(x => x.GuestEmail).Distinct().Count()
            })
            .OrderBy(x => x.Date)
            .ToList();
        
        return guestCountPerDate
            .Select(g => new ChartData { 
                Type = g.Date.ToString("MM/dd/yyyy"), 
                Value = g.GuestCount })
            .ToList();
        
    }
    
    public async Task<ChartStatistic> GetEarningsChart(DateOnly start, DateOnly end, CancellationToken ct)
    {
        var paymentsByDate = await GetPaymentsByDate(start, end, ct);

        var comparisonDate = start.AddDays(start.DayNumber - end.DayNumber);
        var oldStatistic = await GetPaymentsByDate(comparisonDate, start, ct);
        
        var newSum = (double)paymentsByDate.Sum(x => x.Value);
        var oldSum = (double)oldStatistic.Sum(x => x.Value);
        
        var percentage = oldSum == 0 ? 100 : newSum * 100.0 / oldSum - 100.0;
        
        var chartStatistic = new ChartStatistic
        {
            ChartsData = paymentsByDate,
            ValueSum = newSum,
            Percentage = percentage
        };
        
        return chartStatistic;
    }

    public async Task<List<ChartData>> GetPaymentsByDate(DateOnly start, DateOnly end, CancellationToken ct)
    {
        return await _context.Payments
            .Where(p => p.Date >= start && p.Date <= end)
            .GroupBy(p => p.Date)
            .OrderBy(g => g.Key) 
            .Select(g => new ChartData
            {
                Type = g.Key.ToString("yyyy-MM-dd"), // or any format you prefer
                Value = (int)g.Sum(p => p.Amount / 100)
            })
            .ToListAsync(ct);
    }
}