using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNakdiVeFizikiGerceklesme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FizikiGerceklesmeOrani",
                schema: "Proje",
                table: "Proje",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NakdiGerceklesmeTutari",
                schema: "Proje",
                table: "Proje",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FizikiGerceklesmeOrani",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "NakdiGerceklesmeTutari",
                schema: "Proje",
                table: "Proje");
        }
    }
}
