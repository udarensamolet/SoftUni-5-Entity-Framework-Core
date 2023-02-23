using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealer.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedCarProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TravelledDistance",
                table: "Cars",
                newName: "TraveledDistance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TraveledDistance",
                table: "Cars",
                newName: "TravelledDistance");
        }
    }
}
