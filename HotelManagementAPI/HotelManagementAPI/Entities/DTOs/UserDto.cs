namespace HotelManagementAPI.Entities.DTOs;

public class UserDto
{
    public required string Email { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public required string PhoneNumber { get; set; } = string.Empty;
    public required DateTime BirthDate { get; set; }
    public required string UserRole { get; set; } = string.Empty;
}