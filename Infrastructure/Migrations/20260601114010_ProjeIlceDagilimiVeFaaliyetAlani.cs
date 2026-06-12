using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProjeIlceDagilimiVeFaaliyetAlani : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjeFaaliyetAlanlari",
                schema: "Proje",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Yil = table.Column<short>(type: "smallint", nullable: false),
                    Ay = table.Column<byte>(type: "tinyint", nullable: false),
                    FaaliyetMiktari = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    KategoriDegerId = table.Column<long>(type: "bigint", nullable: false),
                    IlceDagilimiId = table.Column<long>(type: "bigint", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjeFaaliyetAlanlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjeFaaliyetAlanlari_ProjeIlceDagilimi_IlceDagilimiId",
                        column: x => x.IlceDagilimiId,
                        principalSchema: "Proje",
                        principalTable: "ProjeIlceDagilimi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjeFaaliyetAlanlari_ProjeKategoriDeger_KategoriDegerId",
                        column: x => x.KategoriDegerId,
                        principalSchema: "Proje",
                        principalTable: "ProjeKategoriDeger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjeFaaliyetAlanlari_IlceDagilimiId_KategoriDegerId_Yil_Ay",
                schema: "Proje",
                table: "ProjeFaaliyetAlanlari",
                columns: new[] { "IlceDagilimiId", "KategoriDegerId", "Yil", "Ay" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjeFaaliyetAlanlari_KategoriDegerId",
                schema: "Proje",
                table: "ProjeFaaliyetAlanlari",
                column: "KategoriDegerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjeFaaliyetAlanlari");
        }
    }
}
