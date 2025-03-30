using System.Text.Json;
using HotelManagementAPI.Entities.Wrappers;
using HotelManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
    
    protected async Task<ActionResult> ExecuteSafely(Func<Task<ActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, 
                JsonSerializer.Serialize(new { message = "Operation was canceled" }));
        }
        catch (NullReferenceException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, 
                JsonSerializer.Serialize(new { message = e.Message }));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, 
                JsonSerializer.Serialize(new { message = e.Message }));
        }
    }


    [HttpGet]
    public Task<ActionResult> GetAvailableRoomTypes(
        [FromQuery] string roomType,
        [FromQuery] DateOnly checkIn,
        [FromQuery] DateOnly checkOut,
        [FromQuery] int floor,
        CancellationToken ct) =>
        ExecuteSafely(async () =>

        {
            var rooms =
                await _bookingService.GetAvailableRoomTypesAsync(roomType, checkIn, checkOut, floor, ct);

            return Ok(rooms);
        });

    [HttpGet]
    public Task<ActionResult> GetServices(CancellationToken ct) =>
        ExecuteSafely(async () =>

        {
            var services = await _bookingService.GetAllServices(ct);
            return Ok(services);
        });

    [Authorize]
    [HttpPost]
    public Task<ActionResult> PlaceReservation
    (
        [FromBody] ReservationPaymentWrapper reservationPayment,
        CancellationToken ct
    ) =>
        ExecuteSafely(async () =>

        {
            await _bookingService.SaveReservation(
                reservationPayment.Reservation,
                reservationPayment.Services,
                reservationPayment.RoomFloor,
                ct);
            await _bookingService.SavePayment(reservationPayment.Payment, ct);

            return Ok();
        });
}