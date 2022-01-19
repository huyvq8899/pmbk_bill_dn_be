using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addsobienbanchobbdc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoBienBan",
                table: "BienBanDieuChinhs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoBienBan",
                table: "BienBanDieuChinhs");
        }
    }
}
