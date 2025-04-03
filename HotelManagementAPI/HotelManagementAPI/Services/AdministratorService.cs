using AutoMapper;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Services;

public class AdministratorService
{
    private readonly HotelManagementContext _context;
    private readonly IMapper _mapper;

    public AdministratorService(HotelManagementContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> GetAllUsers(CancellationToken ct)
    {
        var users =  await _context.Users.ToListAsync(ct);
        var usersDto = users.Select(u => _mapper.Map<UserDto>(u)).ToList();
        
        return usersDto;
    }

    public async Task<UserDto> PromoteToManagerAsync(string email, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync([email], ct);
        if(user == null)
            throw new NullReferenceException("User not found");
        
        var guest = await _context.Guests.FindAsync([user.Email], ct);
        if(guest == null)
            throw new NullReferenceException("User was not in a Guests table");
        
        if(user.UserRole != EUserRole.Guest)
            throw new BadHttpRequestException("This user role is not 'Guest'");
        
        user.UserRole = EUserRole.Receptionist;
        
        _context.Guests.Remove(guest);
        await _context.Receptionists.AddAsync(new Receptionist{Email = email}, ct);
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> DegradeToGuestAsync(string email, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync([email], ct);
        if(user == null)
            throw new NullReferenceException("User not found");
        
        var receptionist = await _context.Receptionists.FindAsync([user.Email], ct);
        if(receptionist == null)
            throw new NullReferenceException("User was not in a Receptionists table");
        
        if(user.UserRole != EUserRole.Receptionist)
            throw new BadHttpRequestException("This user role is not 'Receptionist'");
        
        
        user.UserRole = EUserRole.Guest;
        _context.Receptionists.Remove(receptionist);
        await _context.Guests.AddAsync(new Guest{Email = email}, ct);
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
        
        return _mapper.Map<UserDto>(user);
    }
}