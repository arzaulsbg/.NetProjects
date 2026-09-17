using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddTheatreSeatCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoldSeats",
                table: "Theatres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlatinumSeats",
                table: "Theatres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SilverSeats",
                table: "Theatres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoldSeats",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "PlatinumSeats",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "SilverSeats",
                table: "Theatres");
        }
    }
}
