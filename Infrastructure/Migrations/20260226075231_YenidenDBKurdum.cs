using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class YenidenDBKurdum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_DegerId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_DegerId1",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_DegerId2",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_DegerId3",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjeKategoriDeger_Deger_DegerId",
                table: "ProjeKategoriDeger");

            migrationBuilder.DropIndex(
                name: "IX_Proje_DegerId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_DegerId1",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_DegerId2",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_DegerId3",
                schema: "Proje",
                table: "Proje");

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

            migrationBuilder.DropColumn(
                name: "DegerId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "DegerId1",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "DegerId2",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "DegerId3",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.RenameTable(
                name: "ProjeKategoriDeger",
                newName: "ProjeKategoriDeger",
                newSchema: "Proje");

            migrationBuilder.CreateIndex(
                name: "IX_Deger_KategoriId_Adi",
                schema: "Ortak",
                table: "Deger",
                columns: new[] { "KategoriId", "Adi" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjeKategoriDeger_Deger_DegerId",
                schema: "Proje",
                table: "ProjeKategoriDeger",
                column: "DegerId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjeKategoriDeger_Deger_DegerId",
                schema: "Proje",
                table: "ProjeKategoriDeger");

            migrationBuilder.DropIndex(
                name: "IX_Deger_KategoriId_Adi",
                schema: "Ortak",
                table: "Deger");

            migrationBuilder.RenameTable(
                name: "ProjeKategoriDeger",
                schema: "Proje",
                newName: "ProjeKategoriDeger");

            migrationBuilder.AddColumn<long>(
                name: "DegerId",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DegerId1",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DegerId2",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DegerId3",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Proje_DegerId",
                schema: "Proje",
                table: "Proje",
                column: "DegerId");

            migrationBuilder.CreateIndex(
                name: "IX_Proje_DegerId1",
                schema: "Proje",
                table: "Proje",
                column: "DegerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Proje_DegerId2",
                schema: "Proje",
                table: "Proje",
                column: "DegerId2");

            migrationBuilder.CreateIndex(
                name: "IX_Proje_DegerId3",
                schema: "Proje",
                table: "Proje",
                column: "DegerId3");

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_DegerId",
                schema: "Proje",
                table: "Proje",
                column: "DegerId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_DegerId1",
                schema: "Proje",
                table: "Proje",
                column: "DegerId1",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_DegerId2",
                schema: "Proje",
                table: "Proje",
                column: "DegerId2",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_DegerId3",
                schema: "Proje",
                table: "Proje",
                column: "DegerId3",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjeKategoriDeger_Deger_DegerId",
                table: "ProjeKategoriDeger",
                column: "DegerId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
