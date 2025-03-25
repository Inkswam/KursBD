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
    public async Task<ActionResult> GetAvailableRoomTypes([FromQuery] DateOnly checkIn, [FromQuery] DateOnly checkOut, [FromQuery] int floor)
    {
        var availableRooms = await _bookingService.GetAvailableRoomTypesAsync(checkIn, checkOut, floor);
        return Ok(availableRooms);
    }
}