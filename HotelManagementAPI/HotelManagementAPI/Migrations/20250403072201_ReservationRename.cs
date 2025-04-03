using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReservationRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_reservation_reservation_id",
                schema: "public",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_reservation_guests_guest_email",
                schema: "public",
                table: "reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_reservation_rooms_room_number",
                schema: "public",
                table: "reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reservation",
                schema: "public",
                table: "reservation");

            migrationBuilder.RenameTable(
                name: "reservation",
                schema: "public",
                newName: "reservations",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_reservation_room_number",
                schema: "public",
                table: "reservations",
                newName: "IX_reservations_room_number");

            migrationBuilder.RenameIndex(
                name: "IX_reservation_guest_email",
                schema: "public",
                table: "reservations",
                newName: "IX_reservations_guest_email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reservations",
                schema: "public",
                table: "reservations",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_reservations_reservation_id",
                schema: "public",
                table: "payments",
                column: "reservation_id",
                principalSchema: "public",
                principalTable: "reservations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_guests_guest_email",
                schema: "public",
                table: "reservations",
                column: "guest_email",
                principalSchema: "public",
                principalTable: "guests",
                principalColumn: "email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_rooms_room_number",
                schema: "public",
                table: "reservations",
                column: "room_number",
                principalSchema: "public",
                principalTable: "rooms",
                principalColumn: "number",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_reservations_reservation_id",
                schema: "public",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_guests_guest_email",
                schema: "public",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_rooms_room_number",
                schema: "public",
                table: "reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reservations",
                schema: "public",
                table: "reservations");

            migrationBuilder.RenameTable(
                name: "reservations",
                schema: "public",
                newName: "reservation",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_reservations_room_number",
                schema: "public",
                table: "reservation",
                newName: "IX_reservation_room_number");

            migrationBuilder.RenameIndex(
                name: "IX_reservations_guest_email",
                schema: "public",
                table: "reservation",
                newName: "IX_reservation_guest_email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reservation",
                schema: "public",
                table: "reservation",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_reservation_reservation_id",
                schema: "public",
                table: "payments",
                column: "reservation_id",
                principalSchema: "public",
                principalTable: "reservation",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservation_guests_guest_email",
                schema: "public",
                table: "reservation",
                column: "guest_email",
                principalSchema: "public",
                principalTable: "guests",
                principalColumn: "email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reservation_rooms_room_number",
                schema: "public",
                table: "reservation",
                column: "room_number",
                principalSchema: "public",
                principalTable: "rooms",
                principalColumn: "number",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
