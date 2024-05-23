using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedExpired",
                table: "BookInfo",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LockedExpired",
                table: "BookInfo");
        }
    }
}
