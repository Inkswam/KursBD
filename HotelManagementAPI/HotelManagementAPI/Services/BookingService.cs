using System.Data;
using System.Text.Json;
using AutoMapper;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;
using HotelManagementAPI.Entities.Wrappers;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HotelManagementAPI.Services;

public class BookingService
{
    private readonly StringHelper _stringHelper;
    private readonly HotelManagementContext _context;
    private readonly IMapper _mapper;

    public BookingService(HotelManagementContext context, IMapper mapper, StringHelper stringHelper)
    {
        _context = context;
        _mapper = mapper;
        _stringHelper = stringHelper;
    }
    
    

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomTypesAsync(
        string roomType, 
        DateOnly checkIn, 
        DateOnly checkOut, 
        int floor, 
        CancellationToken ct)
    {
        var checkInParam = new NpgsqlParameter("@start_date", checkIn);
        var checkOutParam = new NpgsqlParameter("@end_date", checkOut);
        var floorParam = new NpgsqlParameter("@selected_floor", floor);

        var result = await _context.Database
            .SqlQuery<RoomDto>($"SELECT * FROM get_available_room_types({checkInParam}, {checkOutParam}, {floorParam})")
            .Select(r => new RoomDto
            {
                room_type = _stringHelper.ToUpper(r.room_type, 0),  
                capacity = r.capacity,
                price = r.price,
                image_url = r.image_url
            })
            .ToListAsync(ct);

        result = result.FindAll(r => 
            string.Equals(r.room_type, roomType, StringComparison.CurrentCultureIgnoreCase) ||
            string.Equals("Any", roomType, StringComparison.CurrentCultureIgnoreCase)
        );
        
        return result;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServices(CancellationToken ct)
    {
        var services = await _context.Services.ToListAsync(ct);
        
        var servicesDto = new List<ServiceDto>();
        foreach (var service in services)
        {
            servicesDto.Add(_mapper.Map<ServiceDto>(service));
        }
        return servicesDto;
    }

    public async Task<ReservationDto> SaveReservation(
        ReservationDto reservation, 
        IEnumerable<ServiceDto>? services, 
        int roomFloor,
        CancellationToken ct)
    {
        var reservationModel = _mapper.Map<Reservation>(reservation);
        reservationModel.Services = JsonSerializer.Serialize(services);

        var roomNumber =
            await GetFreeRoomAsync(
                roomFloor,
                reservationModel.RoomType,
                reservationModel.CheckInDate,
                reservationModel.CheckOutDate,
                ct);

        if (roomNumber == null)
        {
            throw new NullReferenceException("Room number is null");
        }
        
        reservationModel.RoomNumber = (int)roomNumber;
        
        await _context.Reservations.AddAsync(reservationModel, ct);
        await _context.SaveChangesAsync(ct);
        
        return reservation;
    }

    public async Task<PaymentDto> SavePayment(PaymentDto payment, CancellationToken ct)
    {
        var paymentModel = _mapper.Map<Payment>(payment);
        await _context.Payments.AddAsync(paymentModel, ct);
        await _context.SaveChangesAsync(ct);
        
        return payment;
    }
    
    private async Task<int?> GetFreeRoomAsync(int floor, ERoomType type, DateOnly checkinDate, DateOnly checkoutDate, CancellationToken ct)
    {

        var floorParam = new NpgsqlParameter("p_floor", floor);
        var typeParam = new NpgsqlParameter("p_type", type);
        var checkinDateParam = new NpgsqlParameter("p_checkin_date", checkinDate);
        var checkoutDateParam = new NpgsqlParameter("p_checkout_date", checkoutDate);
        var roomNumberParam = new NpgsqlParameter("p_room_number", DbType.Int32) { Direction = ParameterDirection.Output};

        var result = await _context.Database
            .SqlQuery<int>($"SELECT get_free_room({floorParam}, {typeParam}, {checkinDateParam}, {checkoutDateParam})").ToListAsync(ct);

        return result.First();
    }

    public async Task<IEnumerable<BookingWrapper>> GetReservationsByUserAsync(string email, CancellationToken ct)
    {
        var reservations = await _context.Reservations
            .Include(r => r.Payment)
            .Where(r => r.GuestEmail == email)
            .ToListAsync(ct);

        var bookingWrappersSerialized = new List<BookingWrapper>();
        foreach (var reservation in reservations)
        {
            var uniqueRoom = await _context.UniqueRooms.FindAsync([reservation.RoomType], ct);

            if (uniqueRoom == null)
                throw new NullReferenceException($"Room type {reservation.RoomType} was not found");

            var services = JsonSerializer.Deserialize<IEnumerable<ServiceDto>>(reservation.Services);

            if (services == null)
                throw new NullReferenceException($"Reservation service list was not found");
            
            var bookingWrapper = new BookingWrapper
            {
                Reservation = _mapper.Map<ReservationDto>(reservation),
                Payment = _mapper.Map<PaymentDto>(reservation.Payment),
                Services = services,
                Room = _mapper.Map<RoomDto>(uniqueRoom)
            };
            
            bookingWrappersSerialized.Add(bookingWrapper);
        }
        
        return bookingWrappersSerialized;
    }
}
