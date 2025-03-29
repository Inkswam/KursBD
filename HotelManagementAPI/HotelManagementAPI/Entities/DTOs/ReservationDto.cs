namespace HotelManagementAPI.Entities.DTOs;

public class ReservationDto
{
    public required string Id { get; set; }
    public required string GuestEmail { get; set; }
    public int? RoomNumber { get; set; }
    public required string RoomType { get; set; }
    public required DateTime CheckinDate { get; set; }
    public required DateTime CheckoutDate { get; set; }
    public required string Status { get; set; }
}