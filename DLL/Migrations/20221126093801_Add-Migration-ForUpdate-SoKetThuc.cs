using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddMigrationForUpdateSoKetThuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpdateSoBatDau",
                table: "SinhSoHDMayTinhTiens",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdateSoKetThuc",
                table: "SinhSoHDMayTinhTiens",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PhatHanhNgayPos",
                table: "HoaDonDienTus",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpdateSoBatDau",
                table: "SinhSoHDMayTinhTiens");

            migrationBuilder.DropColumn(
                name: "IsUpdateSoKetThuc",
                table: "SinhSoHDMayTinhTiens");

            migrationBuilder.DropColumn(
                name: "PhatHanhNgayPos",
                table: "HoaDonDienTus");
        }
    }
}
