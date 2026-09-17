using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerLoginID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginID",
                table: "Customers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginID",
                table: "Customers");
        }
    }
}
