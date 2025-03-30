using HotelManagementAPI.Entities.DTOs;

namespace HotelManagementAPI.Entities.Wrappers;

public class UserReservationsWrapper
{
    public IEnumerable<UserDto> Users { get; set; }
    public IEnumerable<ReservationDto> Reservations { get; set; }
}