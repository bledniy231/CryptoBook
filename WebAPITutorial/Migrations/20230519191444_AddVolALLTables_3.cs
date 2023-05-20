﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPITutorial.Migrations
{
    /// <inheritdoc />
    public partial class AddVolALLTables_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARB_USDT_Items");

            migrationBuilder.CreateTable(
                name: "BTC_USDT_Items",
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
                    table.PrimaryKey("PK_BTC_USDT_Items", x => x.KucoinVolId);
                });

            migrationBuilder.CreateTable(
                name: "ETH_USDT_Items",
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
                    table.PrimaryKey("PK_ETH_USDT_Items", x => x.KucoinVolId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BTC_USDT_Items");

            migrationBuilder.DropTable(
                name: "ETH_USDT_Items");

            migrationBuilder.CreateTable(
                name: "ARB_USDT_Items",
                columns: table => new
                {
                    KucoinVolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangePercentage = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARB_USDT_Items", x => x.KucoinVolId);
                });
        }
    }
}
