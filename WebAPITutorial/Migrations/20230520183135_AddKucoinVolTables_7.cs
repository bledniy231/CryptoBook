using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPITutorial.Migrations
{
    /// <inheritdoc />
    public partial class AddKucoinVolTables_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "XRP_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "UNI_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "SOL_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "MATIC_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "LTC_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "ETH_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "DOGE_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "BTC_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "BNB_USDT_Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ChangePercentage",
                table: "ARB_USDT_Items",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "XRP_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "UNI_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SOL_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "MATIC_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "LTC_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ETH_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "DOGE_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "BTC_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "BNB_USDT_Items",
                newName: "ChangePercentage");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ARB_USDT_Items",
                newName: "ChangePercentage");
        }
    }
}
