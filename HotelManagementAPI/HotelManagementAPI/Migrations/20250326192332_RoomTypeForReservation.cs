using HotelManagementAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class RoomTypeForReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "price",
                schema: "public",
                table: "services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<ERoomType>(
                name: "room_type",
                schema: "public",
                table: "reservation",
                type: "room_type",
                nullable: false,
                defaultValue: ERoomType.Single);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "room_type",
                schema: "public",
                table: "reservation");

            migrationBuilder.AlterColumn<long>(
                name: "price",
                schema: "public",
                table: "services",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
