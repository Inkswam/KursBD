﻿using AutoMapper;
using HotelManagementAPI.Entities.DTOs;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Entities.Models;

namespace HotelManagementAPI.Profiles;

public class MappingProfile : Profile
{
    
    public MappingProfile()
    {
        
        // USER SECTION
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
                    .MapFrom(ud => DateOnly.FromDateTime(ud.BirthDate).AddDays(1)));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password,
                opt => opt.Ignore())
            .ForMember(dest => dest.UserRole,
                opt => opt
                    .MapFrom(um => um.UserRole.ToString()))
            .ForMember(dest => dest.BirthDate,
                opt => opt
                    .MapFrom(um => um.BirthDate.ToDateTime(new TimeOnly(0, 0, 0, 0))));

        // SERVICE SECTION
        CreateMap<ServiceDto, Service>()
            .ForMember(dest => dest.Id,
                opt => opt
                    .MapFrom(sd => Guid.Parse(sd.Id)));

        CreateMap<Service, ServiceDto>()
            .ForMember(dest => dest.Id,
                opt => opt
                    .MapFrom(s => s.Id.ToString()));
        
        // RESERVATION SECTION
        CreateMap<ReservationDto, Reservation>()
            .ForMember(dest => dest.Id,
                opt => opt
                    .MapFrom(rd => Guid.Parse(rd.Id)))
            .ForMember(dest => dest.RoomType,
                opt => opt
                    .MapFrom(rd => Enum.Parse<ERoomType>(rd.RoomType)))
            .ForMember(dest => dest.Services,
                opt => opt.Ignore())
            .ForMember(dest => dest.Room,
                opt => opt.Ignore())
            .ForMember(dest => dest.Guest,
                opt => opt.Ignore())
            .ForMember(dest => dest.Payment,
                opt => opt.Ignore())
            .ForMember(dest => dest.CheckInDate,
                opt => opt
                    .MapFrom(rd => DateOnly.FromDateTime(rd.CheckinDate).AddDays(1)))
            .ForMember(dest => dest.CheckOutDate,
                opt => opt
                    .MapFrom(rd => DateOnly.FromDateTime(rd.CheckoutDate).AddDays(1)))
            .ForMember(dest => dest.Status,
                opt => opt
                    .MapFrom(rd => Enum.Parse<EReservationStatus>(rd.Status)));

        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.Id,
                opt => opt
                    .MapFrom(r => r.Id.ToString()))
            .ForMember(dest => dest.RoomType,
                opt => opt
                    .MapFrom(rd => rd.RoomType.ToString()))
            .ForMember(dest => dest.Status,
                opt => opt
                    .MapFrom(r => r.Status.ToString()))
            .ForMember(dest => dest.CheckinDate,
                opt => opt
                    .MapFrom(r => r.CheckInDate.ToDateTime(new TimeOnly(0, 0, 0, 0))))
            .ForMember(dest => dest.CheckoutDate,
                opt => opt
                    .MapFrom(r => r.CheckOutDate.ToDateTime(new TimeOnly(0, 0, 0, 0))));
        
        // PAYMENT SECTION
        CreateMap<PaymentDto, Payment>()
            .ForMember(dest => dest.Id,
                opt => opt
                    .MapFrom(pd => Guid.Parse(pd.Id)))
            .ForMember(dest => dest.Reservation,
                opt => opt.Ignore())
            .ForMember(dest => dest.ReservationId,
                opt => opt
                    .MapFrom(pd => Guid.Parse(pd.ReservationId)))
            .ForMember(dest => dest.Date,
                opt => opt
                    .MapFrom(pd => DateOnly.FromDateTime(pd.Date).AddDays(1)))
            .ForMember(dest => dest.PaymentMethod,
                opt => opt
                    .MapFrom(pd => Enum.Parse<EPaymentMethod>(pd.Method)));
        
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.Id,
                opt => opt
                    .MapFrom(u => u.Id.ToString()))
            .ForMember(dest => dest.ReservationId,
                opt => opt
                    .MapFrom(u => u.ReservationId.ToString()))
            .ForMember(dest => dest.Date,
                opt => opt
                .MapFrom(u => u.Date.ToDateTime(new TimeOnly(0, 0, 0, 0))))
            .ForMember(dest => dest.Method,
                opt => opt
                .MapFrom(u => u.PaymentMethod.ToString()));
        
        CreateMap<RoomDto, UniqueRoom>()
            .ForMember(dest => dest.RoomType,
                opt => opt
                    .MapFrom(ud => Enum.Parse<ERoomType>(ud.room_type)))
            .ForMember(dest => dest.ImageUrl,
                opt => opt
                    .MapFrom(ud => ud.image_url));
        
        CreateMap<UniqueRoom, RoomDto>()
            .ForMember(dest => dest.room_type,
                opt => opt
                    .MapFrom(u => u.RoomType.ToString()))
            .ForMember(dest => dest.image_url,
                opt => opt
                    .MapFrom(ud => ud.ImageUrl));
    }
}