using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DaireBaskanliklariSorumluBirimler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<long>(
                name: "SorumluDaireBaskanligiId",
                schema: "Proje",
                table: "Proje",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DaireBaskanligiId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DaireBaskanliklari",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaireBaskanliklari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjePaydasBirim",
                schema: "Proje",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjeId = table.Column<long>(type: "bigint", nullable: false),
                    DaireBaskanligiId = table.Column<long>(type: "bigint", nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjePaydasBirim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjePaydasBirim_DaireBaskanliklari_DaireBaskanligiId",
                        column: x => x.DaireBaskanligiId,
                        principalTable: "DaireBaskanliklari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjePaydasBirim_Proje_ProjeId",
                        column: x => x.ProjeId,
                        principalSchema: "Proje",
                        principalTable: "Proje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proje_SorumluDaireBaskanligiId",
                schema: "Proje",
                table: "Proje",
                column: "SorumluDaireBaskanligiId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DaireBaskanligiId",
                table: "AspNetUsers",
                column: "DaireBaskanligiId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjePaydasBirim_DaireBaskanligiId",
                schema: "Proje",
                table: "ProjePaydasBirim",
                column: "DaireBaskanligiId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjePaydasBirim_ProjeId_DaireBaskanligiId",
                schema: "Proje",
                table: "ProjePaydasBirim",
                columns: new[] { "ProjeId", "DaireBaskanligiId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DaireBaskanliklari_DaireBaskanligiId",
                table: "AspNetUsers",
                column: "DaireBaskanligiId",
                principalTable: "DaireBaskanliklari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proje_DaireBaskanliklari_SorumluDaireBaskanligiId",
                schema: "Proje",
                table: "Proje",
                column: "SorumluDaireBaskanligiId",
                principalTable: "DaireBaskanliklari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DaireBaskanliklari_DaireBaskanligiId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Proje_DaireBaskanliklari_SorumluDaireBaskanligiId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropTable(
                name: "ProjePaydasBirim",
                schema: "Proje");

            migrationBuilder.DropTable(
                name: "DaireBaskanliklari");

            migrationBuilder.DropIndex(
                name: "IX_Proje_SorumluDaireBaskanligiId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DaireBaskanligiId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SorumluDaireBaskanligiId",
                schema: "Proje",
                table: "Proje");

            migrationBuilder.DropColumn(
                name: "DaireBaskanligiId",
                table: "AspNetUsers");

            
        }
    }
}
