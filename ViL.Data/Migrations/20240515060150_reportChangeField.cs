using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViL.Data.Migrations
{
    /// <inheritdoc />
    public partial class reportChangeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "ReportContent",
                table: "Reports",
                newName: "Message");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Reports",
                newName: "ReportContent");

        }
    }
}
