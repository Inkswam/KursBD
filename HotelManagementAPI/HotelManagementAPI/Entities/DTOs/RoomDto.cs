using HotelManagementAPI.Entities.Enums;

namespace HotelManagementAPI.Entities.DTOs;

public class RoomDto
{
    public required string room_type { get; set; }
    public required int capacity { get; set; }
    public required long price { get; set; }
    public string? image_url { get; set; }
}