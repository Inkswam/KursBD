﻿// <auto-generated />
using System;
using HotelManagementAPI.Context;
using HotelManagementAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    [DbContext(typeof(HotelManagementContext))]
    [Migration("20250326192332_RoomTypeForReservation")]
    partial class RoomTypeForReservation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "payment_method", new[] { "apple_pay", "cash", "credit_card", "google_pay", "paypal" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "reservation_status", new[] { "closed", "confirmed", "expired", "reserved" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "room_status", new[] { "available", "booked", "unavailable" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "room_type", new[] { "apartment", "deluxe", "double", "inclusive", "single" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "user_role", new[] { "administrator", "guest", "receptionist" });
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Administrator", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.HasKey("Email");

                    b.ToTable("administrators", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Blacklist", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.Property<string>("Reason")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("reason");

                    b.HasKey("Email");

                    b.ToTable("blacklist", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Guest", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.Property<string>("History")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("history");

                    b.HasKey("Email");

                    b.ToTable("guests", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint")
                        .HasColumnName("amount");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<EPaymentMethod>("PaymentMethod")
                        .HasColumnType("payment_method")
                        .HasColumnName("payment_method");

                    b.Property<Guid>("ReservationId")
                        .HasColumnType("uuid")
                        .HasColumnName("reservation_id");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId")
                        .IsUnique();

                    b.ToTable("payments", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Receptionist", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.HasKey("Email");

                    b.ToTable("receptionists", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("CheckInDate")
                        .HasColumnType("date")
                        .HasColumnName("check_in_date");

                    b.Property<DateOnly>("CheckOutDate")
                        .HasColumnType("date")
                        .HasColumnName("check_out_date");

                    b.Property<string>("GuestEmail")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("guest_email");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer")
                        .HasColumnName("room_number");

                    b.Property<ERoomType>("RoomType")
                        .HasColumnType("room_type")
                        .HasColumnName("room_type");

                    b.Property<string>("Services")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("services");

                    b.Property<EReservationStatus>("Status")
                        .HasColumnType("reservation_status")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("GuestEmail");

                    b.HasIndex("RoomNumber");

                    b.ToTable("reservation", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Room", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Number"));

                    b.Property<int>("Floor")
                        .HasColumnType("integer")
                        .HasColumnName("floor");

                    b.Property<ERoomStatus>("Status")
                        .HasColumnType("room_status")
                        .HasColumnName("status");

                    b.Property<ERoomType>("Type")
                        .HasColumnType("room_type")
                        .HasColumnName("type");

                    b.HasKey("Number");

                    b.HasIndex("Type");

                    b.ToTable("rooms", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<long>("Price")
                        .HasColumnType("bigint")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.ToTable("services", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.UniqueRoom", b =>
                {
                    b.Property<ERoomType>("RoomType")
                        .HasColumnType("room_type")
                        .HasColumnName("room_type");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer")
                        .HasColumnName("capacity");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<long>("Price")
                        .HasColumnType("bigint")
                        .HasColumnName("price");

                    b.HasKey("RoomType");

                    b.ToTable("unique_room", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("birth_date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("password_salt");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("phone_number");

                    b.Property<EUserRole>("UserRole")
                        .HasColumnType("user_role")
                        .HasColumnName("user_role");

                    b.HasKey("Email");

                    b.ToTable("users", "public");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Administrator", b =>
                {
                    b.HasOne("HotelManagementAPI.Entities.Models.User", "User")
                        .WithOne("Administrator")
                        .HasForeignKey("HotelManagementAPI.Entities.Models.Administrator", "Email")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Guest", b =>
                {
                    b.HasOne("HotelManagementAPI.Entities.Models.User", "User")
                        .WithOne("Guest")
                        .HasForeignKey("HotelManagementAPI.Entities.Models.Guest", "Email")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Payment", b =>
                {
                    b.HasOne("HotelManagementAPI.Entities.Models.Reservation", "Reservation")
                        .WithOne("Payment")
                        .HasForeignKey("HotelManagementAPI.Entities.Models.Payment", "ReservationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Receptionist", b =>
                {
                    b.HasOne("HotelManagementAPI.Entities.Models.User", "User")
                        .WithOne("Receptionist")
                        .HasForeignKey("HotelManagementAPI.Entities.Models.Receptionist", "Email")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Reservation", b =>
                {
                    b.HasOne("HotelManagementAPI.Entities.Models.Guest", "Guest")
                        .WithMany("Reservations")
                        .HasForeignKey("GuestEmail")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HotelManagementAPI.Entities.Models.Room", "Room")
                        .WithMany("Reservations")
                        .HasForeignKey("RoomNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Room", b =>
                {
                    b.HasOne("HotelManagementAPI.Entities.Models.UniqueRoom", "UniqueRoom")
                        .WithMany("Rooms")
                        .HasForeignKey("Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UniqueRoom");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Guest", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Reservation", b =>
                {
                    b.Navigation("Payment");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.Room", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.UniqueRoom", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HotelManagementAPI.Entities.Models.User", b =>
                {
                    b.Navigation("Administrator");

                    b.Navigation("Guest");

                    b.Navigation("Receptionist");
                });
#pragma warning restore 612, 618
        }
    }
}
