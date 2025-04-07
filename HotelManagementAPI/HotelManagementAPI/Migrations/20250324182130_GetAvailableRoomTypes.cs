using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class GetAvailableRoomTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_available_room_types(
                    start_date DATE, 
                    end_date DATE, 
                    selected_floor INT
                ) 
                RETURNS TABLE (
                    room_type room_type,
                    capacity INT,
                    price BIGINT,
                    image_url TEXT
                ) 
                AS $$
                BEGIN
                    RETURN QUERY 
                    SELECT ur.room_type, ur.capacity, ur.price, ur.image_url
                    FROM unique_room ur
                    WHERE EXISTS (
                        SELECT 1 
                        FROM rooms r
                        WHERE r.type = ur.room_type 
                        AND r.floor = selected_floor
                        AND NOT EXISTS (
                            SELECT 1 
                            FROM reservations res
                            WHERE res.room_number = r.number
                            AND (res.check_in_date, res.check_out_date) OVERLAPS (start_date, end_date)
                        )
                    )
                    LIMIT 5;
                END; 
                $$ LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_available_room_types(DATE, DATE, INT);");
        }
    }
}
