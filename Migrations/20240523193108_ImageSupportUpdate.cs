using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smartphone_Store.Migrations
{
    /// <inheritdoc />
    public partial class ImageSupportUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePaths",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ImagePaths",
                columns: table => new
                {
                    ImagePathId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePaths", x => x.ImagePathId);
                    table.ForeignKey(
                        name: "FK_ImagePaths_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagePaths_ProductId",
                table: "ImagePaths",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagePaths");

            migrationBuilder.AddColumn<string>(
                name: "ImagePaths",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
