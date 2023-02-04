using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ella.DAL.Migrations
{
    public partial class DeletetDescriptionFromTeamsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Teams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
