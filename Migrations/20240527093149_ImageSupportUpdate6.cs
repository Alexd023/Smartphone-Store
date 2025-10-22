using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smartphone_Store.Migrations
{
    /// <inheritdoc />
    public partial class ImageSupportUpdate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Products",
                newName: "ModelName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "Products",
                newName: "Model");
        }
    }
}
