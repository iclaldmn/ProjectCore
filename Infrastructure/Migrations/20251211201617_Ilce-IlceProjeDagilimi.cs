using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IlceIlceProjeDagilimi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ilce",
                schema: "Ortak",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilce", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjeIlceDagilimi",
                schema: "Proje",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IlceyeOdenenBedeli = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IlceId = table.Column<long>(type: "bigint", nullable: false),
                    ProjeId = table.Column<long>(type: "bigint", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjeIlceDagilimi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjeIlceDagilimi_Ilce_IlceId",
                        column: x => x.IlceId,
                        principalSchema: "Ortak",
                        principalTable: "Ilce",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjeIlceDagilimi_Proje_ProjeId",
                        column: x => x.ProjeId,
                        principalSchema: "Proje",
                        principalTable: "Proje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Ortak",
                table: "Ilce",
                columns: new[] { "Id", "Adi", "Silindi" },
                values: new object[,]
                {
                    { 1L, "Akyurt", false },
                    { 2L, "Altındağ", false },
                    { 3L, "Ayaş", false },
                    { 4L, "Bala", false },
                    { 5L, "Beypazarı", false },
                    { 6L, "Çamlıdere", false },
                    { 7L, "Çankaya", false },
                    { 8L, "Çubuk", false },
                    { 9L, "Elmadağ", false },
                    { 10L, "Etimesgut", false },
                    { 11L, "Evren", false },
                    { 12L, "Gölbaşı", false },
                    { 13L, "Güdül", false },
                    { 14L, "Haymana", false },
                    { 15L, "Kahramankazan", false },
                    { 16L, "Kalecik", false },
                    { 17L, "Keçiören", false },
                    { 18L, "Kızılcahamam", false },
                    { 19L, "Mamak", false },
                    { 20L, "Nallıhan", false },
                    { 21L, "Polatlı", false },
                    { 22L, "Pursaklar", false },
                    { 23L, "Sincan", false },
                    { 24L, "Şereflikoçhisar", false },
                    { 25L, "Yenimahalle", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjeIlceDagilimi_IlceId",
                schema: "Proje",
                table: "ProjeIlceDagilimi",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjeIlceDagilimi_ProjeId",
                schema: "Proje",
                table: "ProjeIlceDagilimi",
                column: "ProjeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjeIlceDagilimi",
                schema: "Proje");

            migrationBuilder.DropTable(
                name: "Ilce",
                schema: "Ortak");
        }
    }
}
