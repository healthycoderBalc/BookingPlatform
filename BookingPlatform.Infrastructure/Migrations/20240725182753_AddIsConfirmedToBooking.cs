using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsConfirmedToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isConfirmed",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isConfirmed",
                table: "Bookings");
        }
    }
}
