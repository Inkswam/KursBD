using System.Text.Json;
using HotelManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly ManagementService _managementService;

    public ManagerController(ManagementService managementService)
    {
        _managementService = managementService;
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
    public Task<ActionResult> GetBookedDates(CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var roomBookings = await _managementService.GetRoomBookingsAsync();
            var result = _managementService.GetFullyBookedDates(roomBookings);
            return Ok(result);
        });

    [HttpGet]
    public Task<ActionResult> GetGuestsByDate([FromQuery] DateOnly date, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userReservations = await _managementService.GetUserReservationsByDateAsync(date, ct);
            return Ok(userReservations);
        });

    [HttpGet]
    public Task<ActionResult> GetGuestsByCheckinDate([FromQuery] DateOnly date, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userReservations = await _managementService.GetGuestsByCheckinDateAsync(date, ct);
            return Ok(userReservations);
        });

    [HttpGet]
    public Task<ActionResult> GetAllGuestsWithReservations([FromQuery] DateOnly date, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userReservations = await _managementService.GetAllUsersWithReservationsAsync(ct);
            return Ok(userReservations);
        });

    [HttpGet]
    public Task<ActionResult> GetGuestByEmail([FromQuery] string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var guest = await _managementService.GetGuestByEmailAsync(email, ct);
            return Ok(guest);
        });

    [HttpGet]
    public Task<ActionResult> GetReservationsByGuestEmail([FromQuery] string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userReservations = await _managementService.GetUserReservationsAsync(email, ct);
            return Ok(userReservations);
        });

    [HttpGet]
    public Task<ActionResult> GetBooking([FromQuery] string reservationId, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var bookingWrapper = await _managementService.GetBookingAsync(reservationId, ct);
            return Ok(bookingWrapper);
        });
}