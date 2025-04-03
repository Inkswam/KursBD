using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CheckInDate_CheckOutDate",
                table: "reservations",
                columns: new[] { "check_in_date", "check_out_date" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Status_CheckInDate_CheckOutDate",
                table: "reservations",
                columns: new[] { "status", "check_in_date", "check_out_date" });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Type_Status",
                table: "rooms",
                columns: new[] { "type", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_CheckInDate_CheckOutDate",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Status_CheckInDate_CheckOutDate",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Type_Status",
                table: "rooms");
            
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "users");
        }
    }
}
