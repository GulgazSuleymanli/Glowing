using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlowingTemplate.Migrations
{
    public partial class AddIsPrimary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrime",
                table: "ProductImages",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrime",
                table: "ProductImages");
        }
    }
}
