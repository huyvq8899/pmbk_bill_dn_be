using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddColTypeSignToTableChungThuSoSuDungs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeSign",
                table: "ChungThuSoSuDungs",
                nullable: true,defaultValue:0);
            migrationBuilder.AddColumn<string>(
                name: "UserkeySign",
                table: "ChungThuSoSuDungs",
                nullable: true, maxLength: 255);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeSign",
                table: "ChungThuSoSuDungs");
            migrationBuilder.DropColumn(
                name: "UserkeySign",
                table: "ChungThuSoSuDungs");
        }
    }
}
