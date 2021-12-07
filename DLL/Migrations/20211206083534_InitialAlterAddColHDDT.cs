using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class InitialAlterAddColHDDT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackUpTrangThai",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HinhThucXoabo",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackUpTrangThai",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HinhThucXoabo",
                table: "HoaDonDienTus");
        }
    }
}
