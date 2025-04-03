using AutoMapper;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

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


    public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([email], cancellationToken: ct);
        if (userModel == null)
            throw new NullReferenceException("User not found");
        
        return _mapper.Map<UserDto>(userModel);
    }
    
    public async Task<UserDto> CreateGuestUserAsync(UserDto user, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([user.Email], ct);
        if (userModel != null)
            throw new Exception("User already exists");
        
        if(await _context.Blacklists.FindAsync([user.Email], ct) != null)
            throw new AccessViolationException("User is blacklisted");

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

    public async Task<UserDto> CreateReceptionistUserAsync(UserDto user, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([user.Email], ct);
        if (userModel != null)
            throw new Exception("User already exists");
        
        user.UserRole = "Receptionist";
        userModel = _mapper.Map<User>(user);
        
        var (hash, salt) = _credentialsService.HashPassword(user.Password!);
        userModel.PasswordHash = hash;
        userModel.PasswordSalt = salt;
        
        await _context.Users.AddAsync(userModel, ct);
        await _context.Receptionists.AddAsync(new Receptionist { Email = user.Email}, ct);
        await _context.SaveChangesAsync(ct);
        
        return user;
    }

    public async Task<UserDto> VerifyUserAsync(UserDto user, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([user.Email], cancellationToken: ct);
        if (userModel == null)
            throw new Exception("User doesn't exist");
        
        if(await _context.Blacklists.FindAsync([user.Email], ct) != null)
            throw new AccessViolationException("User is blacklisted");

        if (!_credentialsService.VerifyPasswordAsync(userModel, user.Password!, ct))
            throw new Exception("Password does not match");
        
        return _mapper.Map<UserDto>(userModel);
    }

    public async Task<IEnumerable<UserDto>> GetBlacklistedUsersAsync(CancellationToken ct)
    {
        var blacklistedUsers = await _context.Blacklists.ToListAsync(ct);

        var blacklistedUsersDto = new List<UserDto>();
        foreach (var user in blacklistedUsers)
        {
            var userData = await _context.Users.FindAsync([user.Email], ct);
            if (userData != null)
                blacklistedUsersDto.Add(_mapper.Map<UserDto>(userData));
            else
            {
                var notExistingUser = new UserDto
                {
                    Email = user.Email,
                    FirstName = "Unknown",
                    LastName = "Unknown",
                    PhoneNumber = "Unknown",
                    BirthDate = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserRole = "Guest"
                };
                blacklistedUsersDto.Add(notExistingUser);
            }
        }
        
        return blacklistedUsersDto;
    }

    public async Task RemoveFromBlacklistAsync(string email, CancellationToken ct)
    {
        var userModel = await _context.Blacklists.FindAsync([email], ct);
        if(userModel == null)
            throw new NullReferenceException("User is not blacklisted");
        
        _context.Blacklists.Remove(userModel);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<UserDto> AddUserToBlacklistAsync(string email, CancellationToken ct)
    {
        var userAlreadyInBlacklist = await _context.Blacklists.FindAsync([email], ct);
        if(userAlreadyInBlacklist != null)
            throw new Exception("User is already in blacklist");

        var blacklistedUser = new Blacklist
        {
            Email = email
        };
        await _context.Blacklists.AddAsync(blacklistedUser, ct);
        await _context.SaveChangesAsync(ct);
        
        var user = await _context.Users.FindAsync([email], ct);
        if (user != null)
            return _mapper.Map<UserDto>(user);

        return new UserDto
        {
            Email = email,
            FirstName = "Unknown",
            LastName = "Unknown",
            PhoneNumber = "Unknown",
            BirthDate = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UserRole = "Guest"
        };
    }

    public async Task<UserDto> UpdateUserAsync(UserDto user, CancellationToken ct)
    {
        var userModel = await _context.Users.FindAsync([user.Email], ct);
        if(userModel == null)
            throw new NullReferenceException("User not found");
        
        userModel.Email = user.Email;
        userModel.FirstName = user.FirstName;
        userModel.LastName = user.LastName;
        userModel.BirthDate = DateOnly.FromDateTime(user.BirthDate);
        userModel.PhoneNumber = user.PhoneNumber;
        
        _context.Users.Update(userModel);
        await _context.SaveChangesAsync(ct);

        return user;
    }
}