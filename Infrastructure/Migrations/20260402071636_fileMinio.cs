using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fileMinio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileEntity",
                schema: "Ortak",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ObjectName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Bucket = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileReference",
                schema: "Ortak",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Silindi = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileReference_FileEntity_FileId",
                        column: x => x.FileId,
                        principalSchema: "Ortak",
                        principalTable: "FileEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileReference_Proje_ProjeId",
                        column: x => x.ProjeId,
                        principalSchema: "Proje",
                        principalTable: "Proje",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileEntity_ObjectName",
                schema: "Ortak",
                table: "FileEntity",
                column: "ObjectName");

            migrationBuilder.CreateIndex(
                name: "IX_FileReference_EntityId_EntityName",
                schema: "Ortak",
                table: "FileReference",
                columns: new[] { "EntityId", "EntityName" });

            migrationBuilder.CreateIndex(
                name: "IX_FileReference_FileId",
                schema: "Ortak",
                table: "FileReference",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileReference_ProjeId",
                schema: "Ortak",
                table: "FileReference",
                column: "ProjeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileReference",
                schema: "Ortak");

            migrationBuilder.DropTable(
                name: "FileEntity",
                schema: "Ortak");
        }
    }
}
