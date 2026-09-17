using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerID",
                table: "Bookings",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShowID",
                table: "Bookings",
                column: "ShowID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Customers_CustomerID",
                table: "Bookings",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Shows_ShowID",
                table: "Bookings",
                column: "ShowID",
                principalTable: "Shows",
                principalColumn: "ShowID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Customers_CustomerID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Shows_ShowID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CustomerID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ShowID",
                table: "Bookings");
        }
    }
}
