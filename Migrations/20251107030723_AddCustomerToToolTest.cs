using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToolCaptureAppClean.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerToToolTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "ToolTests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "ToolTests");
        }
    }
}
