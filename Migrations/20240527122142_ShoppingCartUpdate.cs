using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smartphone_Store.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_UserId_SessionId",
                table: "ShoppingCarts");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "ShoppingCarts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserId_SessionId",
                table: "ShoppingCarts",
                columns: new[] { "UserId", "SessionId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
