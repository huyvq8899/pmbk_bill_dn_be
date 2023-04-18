using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addnamtbaotomccqtinsaisot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaCQT45",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaCQT45",
                table: "ThongDiepChiTietGuiCQTs");
        }
    }
}
