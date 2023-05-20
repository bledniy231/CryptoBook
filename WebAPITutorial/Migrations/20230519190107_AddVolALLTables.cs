using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPITutorial.Migrations
{
    /// <inheritdoc />
    public partial class AddVolALLTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KucoinVolatilityItems",
                table: "KucoinVolatilityItems");

            migrationBuilder.RenameTable(
                name: "KucoinVolatilityItems",
                newName: "KucoinVolatilityEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KucoinVolatilityEntity",
                table: "KucoinVolatilityEntity",
                column: "KucoinVolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KucoinVolatilityEntity",
                table: "KucoinVolatilityEntity");

            migrationBuilder.RenameTable(
                name: "KucoinVolatilityEntity",
                newName: "KucoinVolatilityItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KucoinVolatilityItems",
                table: "KucoinVolatilityItems",
                column: "KucoinVolId");
        }
    }
}
