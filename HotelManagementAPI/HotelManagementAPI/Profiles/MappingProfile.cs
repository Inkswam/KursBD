using AutoMapper;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;

namespace HotelManagementAPI.Profiles;

public class MappingProfile : Profile
{
    
    public MappingProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.PasswordHash,
                opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, 
                opt => opt.Ignore())
            .ForMember(dest => dest.UserRole,
                opt => opt
                    .MapFrom(ud => Enum.Parse<EUserRole>(ud.UserRole)))
            .ForMember(dest => dest.Administrator, 
                opt => opt.Ignore())
            .ForMember(dest => dest.Receptionist, 
                opt => opt.Ignore())
            .ForMember(dest => dest.Guest, 
                opt => opt.Ignore())
            .ForMember(dest => dest.BirthDate, 
                opt => opt
                    .MapFrom(ud => DateOnly.FromDateTime(ud.BirthDate)));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password,
                opt => opt.Ignore())
            .ForMember(dest => dest.UserRole,
                opt => opt
                    .MapFrom(um => um.UserRole.ToString()))
            .ForMember(dest => dest.BirthDate,
                opt => opt
                    .MapFrom(um => um.BirthDate.ToDateTime(new TimeOnly(0, 0, 0, 0))));
    }
}