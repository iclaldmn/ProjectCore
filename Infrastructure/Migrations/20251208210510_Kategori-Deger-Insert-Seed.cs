using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KategoriDegerInsertSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Ortak",
                table: "Kategori",
                columns: new[] { "Id", "Adi", "Silindi" },
                values: new object[,]
                {
                    { 1L, "Proje Tipi", false },
                    { 2L, "Proje Durumu", false },
                    { 3L, "İhale Türü", false },
                    { 4L, "Hedef Kitlesi", false }
                });

            migrationBuilder.InsertData(
                schema: "Ortak",
                table: "Deger",
                columns: new[] { "Id", "Adi", "KategoriId", "Kodu", "Silindi", "SiraNo" },
                values: new object[,]
                {
                    { 1L, "Yol", 1L, "YOL", false, 1 },
                    { 2L, "Asfalt", 1L, "ASF", false, 2 },
                    { 3L, "Bina", 1L, "BNA", false, 3 },
                    { 4L, "Tamamlandı", 2L, "TMMD", false, 1 },
                    { 5L, "Devam Ediyor", 2L, "DEV", false, 2 },
                    { 6L, "Açık İhale", 3L, "ACK", false, 1 },
                    { 7L, "DMO", 3L, "DMO", false, 2 },
                    { 8L, "Vatandaş", 4L, "VTN", false, 1 },
                    { 9L, "Personel", 4L, "PRS", false, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Deger",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "Ortak",
                table: "Kategori",
                keyColumn: "Id",
                keyValue: 4L);
        }
    }
}
