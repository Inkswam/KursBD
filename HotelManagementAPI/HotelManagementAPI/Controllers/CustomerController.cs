using System.Text.Json;
using HotelManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;


[Route("[controller]/[action]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly BookingService _bookingService;

    public CustomerController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    
    [HttpGet]
    public async Task<ActionResult> GetAvailableRoomTypes(
        [FromQuery] string roomType,
        [FromQuery] DateOnly checkIn, 
        [FromQuery] DateOnly checkOut, 
        [FromQuery] int floor)
    {
        var availableRooms = await _bookingService.GetAvailableRoomTypesAsync(roomType, checkIn, checkOut, floor);
        
        var rooms = new List<string>();
        foreach (var room in availableRooms)
        {
            rooms.Add(JsonSerializer.Serialize(room));
        }
        return Ok(rooms);
    }
}