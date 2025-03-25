using System.Text;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.Enums;
using HotelManagementAPI.Profiles;
using HotelManagementAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<CredentialsService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookingService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1", policy =>
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };

    // Extract token from the cookie
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["auth_token"]; // Read JWT from cookies
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
}).AddCookie("Cookies", options =>
{
    options.Cookie.Name = "auth_token";
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<HotelManagementContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection"),
        o => o
            .MapEnum<EUserRole>("user_role")
            .MapEnum<ERoomType>("room_type")
            .MapEnum<ERoomStatus>("room_status")
            .MapEnum<EPaymentMethod>("payment_method")
            .MapEnum<EReservationStatus>("reservation_status"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Policy1");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();