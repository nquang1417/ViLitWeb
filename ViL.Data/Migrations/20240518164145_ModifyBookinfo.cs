using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBookinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LockedReason",
                table: "BookInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedReason",
                table: "BookInfo");
        }
    }
}
