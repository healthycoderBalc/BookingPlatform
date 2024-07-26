using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfAdultsAndChildrenToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isConfirmed",
                table: "Bookings",
                newName: "IsConfirmed");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfAdults",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAdults",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Bookings",
                newName: "isConfirmed");
        }
    }
}
