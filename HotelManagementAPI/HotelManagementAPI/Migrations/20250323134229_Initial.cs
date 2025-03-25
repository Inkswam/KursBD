using System;
using HotelManagementAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:payment_method", "apple_pay,cash,credit_card,google_pay,paypal")
                .Annotation("Npgsql:Enum:reservation_status", "closed,confirmed,expired,reserved")
                .Annotation("Npgsql:Enum:room_status", "available,booked,unavailable")
                .Annotation("Npgsql:Enum:room_type", "apartment,deluxe,double,inclusive,single")
                .Annotation("Npgsql:Enum:user_role", "administrator,guest,receptionist")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "blacklist",
                schema: "public",
                columns: table => new
                {
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blacklist", x => x.email);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                schema: "public",
                columns: table => new
                {
                    number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    floor = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<ERoomType>(type: "room_type", nullable: false),
                    status = table.Column<ERoomStatus>(type: "room_status", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.number);
                });

            migrationBuilder.CreateTable(
                name: "services",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    price = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    password_salt = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    user_role = table.Column<EUserRole>(type: "user_role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.email);
                });

            migrationBuilder.CreateTable(
                name: "administrators",
                schema: "public",
                columns: table => new
                {
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrators", x => x.email);
                    table.ForeignKey(
                        name: "FK_administrators_users_email",
                        column: x => x.email,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "guests",
                schema: "public",
                columns: table => new
                {
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    history = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guests", x => x.email);
                    table.ForeignKey(
                        name: "FK_guests_users_email",
                        column: x => x.email,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receptionists",
                schema: "public",
                columns: table => new
                {
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receptionists", x => x.email);
                    table.ForeignKey(
                        name: "FK_receptionists_users_email",
                        column: x => x.email,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    guest_email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    room_number = table.Column<int>(type: "integer", nullable: false),
                    check_in_date = table.Column<DateOnly>(type: "date", nullable: false),
                    check_out_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<EReservationStatus>(type: "reservation_status", nullable: false),
                    services = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_guests_guest_email",
                        column: x => x.guest_email,
                        principalSchema: "public",
                        principalTable: "guests",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_rooms_room_number",
                        column: x => x.room_number,
                        principalSchema: "public",
                        principalTable: "rooms",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reservation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    payment_method = table.Column<EPaymentMethod>(type: "payment_method", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_reservation_reservation_id",
                        column: x => x.reservation_id,
                        principalSchema: "public",
                        principalTable: "reservation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_reservation_id",
                schema: "public",
                table: "payments",
                column: "reservation_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservation_guest_email",
                schema: "public",
                table: "reservation",
                column: "guest_email");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_room_number",
                schema: "public",
                table: "reservation",
                column: "room_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrators",
                schema: "public");

            migrationBuilder.DropTable(
                name: "blacklist",
                schema: "public");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "receptionists",
                schema: "public");

            migrationBuilder.DropTable(
                name: "services",
                schema: "public");

            migrationBuilder.DropTable(
                name: "reservation",
                schema: "public");

            migrationBuilder.DropTable(
                name: "guests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "rooms",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
