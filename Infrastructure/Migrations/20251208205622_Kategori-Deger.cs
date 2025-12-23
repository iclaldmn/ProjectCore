using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KategoriDeger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ortak");

            migrationBuilder.CreateTable(
                name: "Kategori",
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
                    table.PrimaryKey("PK_Kategori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deger",
                schema: "Ortak",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KategoriId = table.Column<long>(type: "bigint", nullable: false),
                    Kodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiraNo = table.Column<int>(type: "int", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deger_Kategori_KategoriId",
                        column: x => x.KategoriId,
                        principalSchema: "Ortak",
                        principalTable: "Kategori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deger_KategoriId",
                schema: "Ortak",
                table: "Deger",
                column: "KategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deger",
                schema: "Ortak");

            migrationBuilder.DropTable(
                name: "Kategori",
                schema: "Ortak");
        }
    }
}
