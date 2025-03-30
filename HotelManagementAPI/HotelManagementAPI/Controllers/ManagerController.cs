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
            var userReservations = await _managementService.GetUserReservationsAsync(date, ct);
            return Ok(userReservations);
        });

    [HttpGet]
    public Task<ActionResult> GetGuestsByCheckinDate([FromQuery] DateOnly date, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userReservations = await _managementService.GetGuestsByCheckinDate(date, ct);
            return Ok(userReservations);
        });

    [HttpGet]
    public Task<ActionResult> GetAllGuestsWithReservations([FromQuery] DateOnly date, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var userReservations = await _managementService.GetAllUsersWithReservations(ct);
            return Ok(userReservations);
        });
}