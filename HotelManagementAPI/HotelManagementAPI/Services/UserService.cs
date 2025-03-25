using AutoMapper;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;

namespace HotelManagementAPI.Services;

public class UserService
{
    private readonly HotelManagementContext _context;
    private readonly CredentialsService _credentialsService;
    private readonly IMapper _mapper;

    public UserService(
        HotelManagementContext context,
        IMapper mapper,
        CredentialsService credentialsService)
    {
        _context = context;
        _mapper = mapper;
        _credentialsService = credentialsService;
    }

    
    public async Task<UserDto> CreateGuestUserAsync(UserDto user, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([user.Email], ct);
        if (userModel != null)
            throw new Exception("User already exists");

        user.UserRole = "Guest";
        
        userModel = _mapper.Map<User>(user);
        
        var (hash, salt) = _credentialsService.HashPassword(user.Password!);
        userModel.PasswordHash = hash;
        userModel.PasswordSalt = salt;

        await _context.Users.AddAsync(userModel, ct);
        await _context.Guests.AddAsync(new Guest { Email = user.Email}, ct);
        await _context.SaveChangesAsync(ct);
        
        return user;
    }

    public async Task<UserDto> VerifyUserAsync(UserDto user, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([user.Email], cancellationToken: ct);
        if (userModel == null)
            throw new Exception("User doesn't exist");

        if (!_credentialsService.VerifyPasswordAsync(userModel, user.Password!, ct))
            throw new Exception("Password does not match");
        
        return _mapper.Map<UserDto>(userModel);
    }
}