using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProjeDeger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projeler");

            migrationBuilder.EnsureSchema(
                name: "Proje");

            migrationBuilder.CreateTable(
                name: "Proje",
                schema: "Proje",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Bedeli = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IlaveSozlesmeBedeli = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IhaleTuruId = table.Column<long>(type: "bigint", nullable: false),
                    HedefKitleId = table.Column<long>(type: "bigint", nullable: false),
                    ProjeTipiId = table.Column<long>(type: "bigint", nullable: false),
                    ProjeDurumuId = table.Column<long>(type: "bigint", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeZamani = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullanici = table.Column<long>(type: "bigint", nullable: false),
                    GuncelleyenKullanici = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proje_Deger_HedefKitleId",
                        column: x => x.HedefKitleId,
                        principalSchema: "Ortak",
                        principalTable: "Deger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proje_Deger_IhaleTuruId",
                        column: x => x.IhaleTuruId,
                        principalSchema: "Ortak",
                        principalTable: "Deger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proje_Deger_ProjeDurumuId",
                        column: x => x.ProjeDurumuId,
                        principalSchema: "Ortak",
                        principalTable: "Deger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proje_Deger_ProjeTipiId",
                        column: x => x.ProjeTipiId,
                        principalSchema: "Ortak",
                        principalTable: "Deger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proje_HedefKitleId",
                schema: "Proje",
                table: "Proje",
                column: "HedefKitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Proje_IhaleTuruId",
                schema: "Proje",
                table: "Proje",
                column: "IhaleTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Proje_ProjeDurumuId",
                schema: "Proje",
                table: "Proje",
                column: "ProjeDurumuId");

            migrationBuilder.CreateIndex(
                name: "IX_Proje_ProjeTipiId",
                schema: "Proje",
                table: "Proje",
                column: "ProjeTipiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proje",
                schema: "Proje");

            migrationBuilder.CreateTable(
                name: "Projeler",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bedeli = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeZamani = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuncelleyenKullanici = table.Column<long>(type: "bigint", nullable: true),
                    HedefKitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IhaleTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlaveSozlesmeBedeli = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OlusturanKullanici = table.Column<long>(type: "bigint", nullable: false),
                    OlusturmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeler", x => x.Id);
                });
        }
    }
}
