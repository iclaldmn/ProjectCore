using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KategoriKontrol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                schema: "Ortak",
                table: "Kategori",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProjedeGoster",
                schema: "Ortak",
                table: "Kategori",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProjedeZorunlu",
                schema: "Ortak",
                table: "Kategori",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Aktif", "ProjedeGoster", "ProjedeZorunlu" },
                values: new object[] { false, false, false });

            migrationBuilder.UpdateData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Aktif", "ProjedeGoster", "ProjedeZorunlu" },
                values: new object[] { false, false, false });

            migrationBuilder.UpdateData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Aktif", "ProjedeGoster", "ProjedeZorunlu" },
                values: new object[] { false, false, false });

            migrationBuilder.UpdateData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Aktif", "ProjedeGoster", "ProjedeZorunlu" },
                values: new object[] { false, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktif",
                schema: "Ortak",
                table: "Kategori");

            migrationBuilder.DropColumn(
                name: "ProjedeGoster",
                schema: "Ortak",
                table: "Kategori");

            migrationBuilder.DropColumn(
                name: "ProjedeZorunlu",
                schema: "Ortak",
                table: "Kategori");
        }
    }
}
