using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ella.DAL.Migrations
{
    public partial class CreatedEnumByProductSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSize",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSize",
                table: "Product");
        }
    }
}
