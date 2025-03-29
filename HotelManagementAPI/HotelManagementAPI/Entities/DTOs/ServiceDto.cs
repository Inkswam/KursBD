namespace HotelManagementAPI.Entities.DTOs;

public class ServiceDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required long  Price { get; set; }
}