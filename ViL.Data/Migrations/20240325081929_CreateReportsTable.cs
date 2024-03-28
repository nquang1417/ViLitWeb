using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViL.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportedType = table.Column<int>(type: "int", nullable: false),
                    ReportedEntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportsId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
