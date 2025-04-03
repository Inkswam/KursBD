using System.Security.Claims;
using System.Text.Json;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
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
        catch (AccessViolationException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, 
                JsonSerializer.Serialize(new { message = e.Message }));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, 
                JsonSerializer.Serialize(new { message = e.Message }));
        }
    }

    private readonly JwtService _jwtService;
    private readonly UserService _userService;

    public AuthenticationController(
        JwtService jwtService,
        UserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }
    
    [HttpGet]
    public Task<ActionResult> GetUser(CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var email = (User.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var userDto = await _userService.GetUserByEmailAsync(email, ct);

            return Ok(userDto);
        });
    
    [HttpGet]
    public Task<ActionResult> IsAuthenticated()
    {
        if(User.Identity is { IsAuthenticated: true })
        {
            return Task.FromResult<ActionResult>(Ok(new { isAuthenticated = true }));
        }

        return Task.FromResult<ActionResult>(Unauthorized(new {isAuthenticated = false}));
    }

    [HttpPost]
    public Task<ActionResult> RegisterAsGuest([FromBody] UserDto user, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var createdUser = await _userService.CreateGuestUserAsync(user, ct);

            var accessToken = _jwtService.GenerateToken(createdUser.Email);
            var cookieOptions = _jwtService.GetCookieOptions(createdUser.Email);
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);

            return Ok(createdUser);
        });

    [HttpPut]
    public Task<ActionResult> UpdateUser([FromBody] UserDto user, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var updatedUser = await _userService.UpdateUserAsync(user, ct);
            return Ok(updatedUser);
        });

    [HttpPost]
    public Task<ActionResult> Login(UserDto user, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var verifiedUser = await _userService.VerifyUserAsync(user, ct);

            var accessToken = _jwtService.GenerateToken(verifiedUser.Email);
            var cookieOptions = _jwtService.GetCookieOptions(verifiedUser.Email);
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);

            return Ok(verifiedUser);
        });

    [HttpDelete]
    public Task<ActionResult> Logout(CancellationToken ct) =>
        ExecuteSafely( () =>
        {
            HttpContext.Response.Cookies.Delete("auth_token");
            return Task.FromResult<ActionResult>(Ok());
        });

    [HttpPost]
    public Task<ActionResult> RegisterReceptionist([FromBody] UserDto user, CancellationToken ct) =>
        ExecuteSafely(async () =>
        {
            var createdUser = await _userService.CreateGuestUserAsync(user, ct);

            var accessToken = _jwtService.GenerateToken(createdUser.Email);
            var cookieOptions = _jwtService.GetCookieOptions(createdUser.Email);
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);

            return Ok(createdUser);
        });

}