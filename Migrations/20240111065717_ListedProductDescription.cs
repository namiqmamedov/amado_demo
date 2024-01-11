using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amado.Migrations
{
    /// <inheritdoc />
    public partial class ListedProductDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Descriptions_ProductID",
                table: "Descriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_ProductID",
                table: "Descriptions",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Descriptions_ProductID",
                table: "Descriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_ProductID",
                table: "Descriptions",
                column: "ProductID",
                unique: true);
        }
    }
}
