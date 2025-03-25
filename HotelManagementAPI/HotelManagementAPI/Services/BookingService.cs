using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Services;

public class BookingService
{
    private readonly HotelManagementContext _context;

    public BookingService(HotelManagementContext context)
    {
        _context = context;
    }
    
    

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomTypesAsync(DateOnly checkIn, DateOnly checkOut, int floor)
    {
        var checkInParam = new Npgsql.NpgsqlParameter("@start_date", checkIn);
        var checkOutParam = new Npgsql.NpgsqlParameter("@end_date", checkOut);
        var floorParam = new Npgsql.NpgsqlParameter("@selected_floor", floor);

        var result = await _context.Database
            .SqlQuery<RoomDto>($"SELECT * FROM get_available_room_types({checkInParam}, {checkOutParam}, {floorParam})")
            .Select(r => new RoomDto
            {
                room_type = r.room_type,  // Assuming 'r.Type' is already of type ERoomType
                capacity = r.capacity,
                price = r.price
            })
            .ToListAsync();

        return result;
    }
}