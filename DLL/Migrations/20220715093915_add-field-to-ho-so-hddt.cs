using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addfieldtohosohddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenCoQuanThueCapCuc",
                table: "HoSoHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenCoQuanThueQuanLy",
                table: "HoSoHDDTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenCoQuanThueCapCuc",
                table: "HoSoHDDTs");

            migrationBuilder.DropColumn(
                name: "TenCoQuanThueQuanLy",
                table: "HoSoHDDTs");
        }
    }
}
