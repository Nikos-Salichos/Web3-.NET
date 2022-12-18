using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abi",
                table: "SmartContract");

            migrationBuilder.AddColumn<int>(
                name: "Chain",
                table: "SmartContract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chain",
                table: "SmartContract");

            migrationBuilder.AddColumn<string>(
                name: "Abi",
                table: "SmartContract",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
