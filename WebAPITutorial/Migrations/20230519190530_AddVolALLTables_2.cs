using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPITutorial.Migrations
{
    /// <inheritdoc />
    public partial class AddVolALLTables_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KucoinVolatilityEntity",
                table: "KucoinVolatilityEntity");

            migrationBuilder.RenameTable(
                name: "KucoinVolatilityEntity",
                newName: "ARB_USDT_Items");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ARB_USDT_Items",
                table: "ARB_USDT_Items",
                column: "KucoinVolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ARB_USDT_Items",
                table: "ARB_USDT_Items");

            migrationBuilder.RenameTable(
                name: "ARB_USDT_Items",
                newName: "KucoinVolatilityEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KucoinVolatilityEntity",
                table: "KucoinVolatilityEntity",
                column: "KucoinVolId");
        }
    }
}
