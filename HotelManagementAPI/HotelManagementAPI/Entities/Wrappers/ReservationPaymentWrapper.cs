using HotelManagementAPI.Entities.DTOs;

namespace HotelManagementAPI.Entities.Wrappers;

public class ReservationPaymentWrapper
{
    public required ReservationDto Reservation { get; set; }
    public required PaymentDto Payment { get; set; }
    public required int RoomFloor { get; set; }
    public IEnumerable<ServiceDto>? Services { get; set; }
}