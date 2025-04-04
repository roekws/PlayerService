using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayerService.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Players_DotaId",
                table: "Players",
                column: "DotaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_DotaId",
                table: "Players");
        }
    }
}
