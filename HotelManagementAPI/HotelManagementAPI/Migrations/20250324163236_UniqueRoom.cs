using HotelManagementAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UniqueRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "capacity",
                schema: "public",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "price",
                schema: "public",
                table: "rooms");

            migrationBuilder.AlterColumn<string>(
                name: "history",
                schema: "public",
                table: "guests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "unique_room",
                schema: "public",
                columns: table => new
                {
                    room_type = table.Column<ERoomType>(type: "room_type", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unique_room", x => x.room_type);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rooms_type",
                schema: "public",
                table: "rooms",
                column: "type");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_unique_room_type",
                schema: "public",
                table: "rooms",
                column: "type",
                principalSchema: "public",
                principalTable: "unique_room",
                principalColumn: "room_type",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_unique_room_type",
                schema: "public",
                table: "rooms");

            migrationBuilder.DropTable(
                name: "unique_room",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_rooms_type",
                schema: "public",
                table: "rooms");

            migrationBuilder.AddColumn<int>(
                name: "capacity",
                schema: "public",
                table: "rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "price",
                schema: "public",
                table: "rooms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "history",
                schema: "public",
                table: "guests",
                type: "character varying(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
