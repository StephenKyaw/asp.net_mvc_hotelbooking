using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class bookingitems_udpate_room : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoomId",
                table: "BookingItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingItems_RoomId",
                table: "BookingItems",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingItems_Rooms_RoomId",
                table: "BookingItems",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingItems_Rooms_RoomId",
                table: "BookingItems");

            migrationBuilder.DropIndex(
                name: "IX_BookingItems_RoomId",
                table: "BookingItems");

            migrationBuilder.AlterColumn<string>(
                name: "RoomId",
                table: "BookingItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
