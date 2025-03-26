using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iptv.Api.Migrations
{
    /// <inheritdoc />
    public partial class quantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TvBox");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Order",
                type: "INT",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TvBox",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
