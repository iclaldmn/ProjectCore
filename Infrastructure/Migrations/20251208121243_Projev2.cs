using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Projev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Bedeli",
                table: "Projeler",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "HedefKitle",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IhaleTuru",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "IlaveSozlesmeBedeli",
                table: "Projeler",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bedeli",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "HedefKitle",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "IhaleTuru",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "IlaveSozlesmeBedeli",
                table: "Projeler");
        }
    }
}
