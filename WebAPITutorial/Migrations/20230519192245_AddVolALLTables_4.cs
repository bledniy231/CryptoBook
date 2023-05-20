using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPITutorial.Migrations
{
    /// <inheritdoc />
    public partial class AddVolALLTables_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ARB_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARB_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "BNB_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BNB_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "DOGE_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOGE_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "LTC_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LTC_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "MATIC_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATIC_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "SOL_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOL_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "UNI_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNI_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "XRP_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XRP_USDT_Items", x => x.KucoinVolId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARB_USDT_Items");

            migrationBuilder.DropTable(
                name: "BNB_USDT_Items");

            migrationBuilder.DropTable(
                name: "DOGE_USDT_Items");

            migrationBuilder.DropTable(
                name: "LTC_USDT_Items");

            migrationBuilder.DropTable(
                name: "MATIC_USDT_Items");

            migrationBuilder.DropTable(
                name: "SOL_USDT_Items");

            migrationBuilder.DropTable(
                name: "UNI_USDT_Items");

            migrationBuilder.DropTable(
                name: "XRP_USDT_Items");
        }
    }
}
