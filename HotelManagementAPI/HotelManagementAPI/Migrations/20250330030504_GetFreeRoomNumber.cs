using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class GetFreeRoomNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.get_free_room(
                    IN p_floor integer,
                    IN p_type room_type,
                    IN p_checkin_date date,
                    IN p_checkout_date date
                )
                RETURNS integer
                    LANGUAGE 'plpgsql'
                    VOLATILE
                    PARALLEL UNSAFE
                    COST 100
                    
                AS $BODY$
                DECLARE
                    free_room_number INT;
                BEGIN
                    SELECT r.number
                    INTO free_room_number
                    FROM rooms r
                    LEFT JOIN reservations res
                        ON r.number = res.room_number 
                        AND r.type = res.room_type
                        AND res.check_in_date < p_checkout_date
                        AND res.check_out_date > p_checkin_date
                    WHERE r.floor = p_floor
                      AND r.type = p_type
                      AND res.id IS NULL
                    LIMIT 1;
                    
                    RETURN free_room_number;
                END;
                $BODY$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_free_room(INT, room_type, DATE, DATE);");
        }
    }
}
