using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShalehPrpject.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalDate",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartureDate",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Reset",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Customers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DepartureDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Reset",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Customers");
        }
    }
}
