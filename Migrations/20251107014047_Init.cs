using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToolCaptureAppClean.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToolTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToolId = table.Column<string>(type: "TEXT", nullable: false),
                    TestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Operator = table.Column<string>(type: "TEXT", nullable: true),
                    Material = table.Column<string>(type: "TEXT", nullable: true),
                    Operation = table.Column<string>(type: "TEXT", nullable: true),
                    Coolant = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    PhotoPath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToolTestId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrandName = table.Column<string>(type: "TEXT", nullable: false),
                    Vc = table.Column<decimal>(type: "TEXT", nullable: true),
                    Ae = table.Column<decimal>(type: "TEXT", nullable: true),
                    Ap = table.Column<decimal>(type: "TEXT", nullable: true),
                    Fz = table.Column<decimal>(type: "TEXT", nullable: true),
                    Flutes = table.Column<int>(type: "INTEGER", nullable: true),
                    ResultNotes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandResults_ToolTests_ToolTestId",
                        column: x => x.ToolTestId,
                        principalTable: "ToolTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandResults_ToolTestId",
                table: "BrandResults",
                column: "ToolTestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandResults");

            migrationBuilder.DropTable(
                name: "ToolTests");
        }
    }
}
