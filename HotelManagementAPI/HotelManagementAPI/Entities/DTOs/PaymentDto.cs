namespace HotelManagementAPI.Entities.DTOs;

public class PaymentDto
{
    
    public required string Id { get; set; }
    public required string ReservationId { get; set; }
    public required DateTime Date { get; set; }
    public required int Amount { get; set; }
    public required string Method { get; set; }
}