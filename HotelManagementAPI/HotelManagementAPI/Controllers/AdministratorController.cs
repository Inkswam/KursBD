using System.Text.Json;
using HotelManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;


[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class AdministratorController : ControllerBase
{
    private readonly AdministratorService _administratorService;
    private readonly SeederService _seederService;

    public AdministratorController(AdministratorService administratorService, SeederService seederService)
    {
        _administratorService = administratorService;
        _seederService = seederService;
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
    public Task<ActionResult> GetAllUsers(CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var users = await _administratorService.GetAllUsers(ct);
            return Ok(users);
        });

    [HttpPut]
    public Task<ActionResult> PromoteToManager([FromQuery] string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var users = await _administratorService.PromoteToManagerAsync(email, ct);
            return Ok(users);
        });

    [HttpPut]
    public Task<ActionResult> DegradeToGuest([FromQuery] string email, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var users = await _administratorService.DegradeToGuestAsync(email, ct);
            return Ok(users);
        });

    [AllowAnonymous]
    [HttpPost]
    public Task<ActionResult> SeedBd(CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            await _seederService.SeedAsync(ct);
            return Ok();
        });
}