using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjeKategoriDegerAlanı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_HedefKitleId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_IhaleTuruId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_ProjeDurumuId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_Deger_ProjeTipiId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_HedefKitleId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_IhaleTuruId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_ProjeDurumuId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_Proje_ProjeTipiId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "HedefKitleId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "IhaleTuruId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "ProjeDurumuId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "ProjeTipiId",
                schema: "Proje",
                table: "Proje");

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

            migrationBuilder.CreateTable(
                name: "ProjeKategoriDeger",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjeId = table.Column<long>(type: "bigint", nullable: false),
                    KategoriId = table.Column<long>(type: "bigint", nullable: false),
                    DegerId = table.Column<long>(type: "bigint", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjeKategoriDeger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjeKategoriDeger_Deger_DegerId",
                        column: x => x.DegerId,
                        principalSchema: "Ortak",
                        principalTable: "Deger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjeKategoriDeger_Kategori_KategoriId",
                        column: x => x.KategoriId,
                        principalSchema: "Ortak",
                        principalTable: "Kategori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjeKategoriDeger_Proje_ProjeId",
                        column: x => x.ProjeId,
                        principalSchema: "Proje",
                        principalTable: "Proje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_ProjeKategoriDeger_DegerId",
                table: "ProjeKategoriDeger",
                column: "DegerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjeKategoriDeger_KategoriId",
                table: "ProjeKategoriDeger",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjeKategoriDeger_ProjeId",
                table: "ProjeKategoriDeger",
                column: "ProjeId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "ProjeKategoriDeger");

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

            migrationBuilder.AddColumn<long>(
                name: "HedefKitleId",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IhaleTuruId",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProjeDurumuId",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProjeTipiId",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_HedefKitleId",
                schema: "Proje",
                table: "Proje",
                column: "HedefKitleId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_IhaleTuruId",
                schema: "Proje",
                table: "Proje",
                column: "IhaleTuruId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_ProjeDurumuId",
                schema: "Proje",
                table: "Proje",
                column: "ProjeDurumuId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_Deger_ProjeTipiId",
                schema: "Proje",
                table: "Proje",
                column: "ProjeTipiId",
                principalSchema: "Ortak",
                principalTable: "Deger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
