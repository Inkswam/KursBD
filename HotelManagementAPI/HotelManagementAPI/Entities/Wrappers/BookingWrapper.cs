using HotelManagementAPI.Entities.DTOs;

namespace HotelManagementAPI.Entities.Wrappers;

public class BookingWrapper
{
    public required ReservationDto Reservation { get; set; }
    public required PaymentDto Payment { get; set; }
    public required RoomDto Room { get; set; }
    public IEnumerable<ServiceDto>? Services { get; set; }
    
}