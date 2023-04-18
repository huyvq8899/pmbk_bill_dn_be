using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtinhchattohdct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HangKhuyenMai",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.AddColumn<int>(
                name: "TinhChat",
                table: "HoaDonDienTuChiTiets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhChat",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.AddColumn<bool>(
                name: "HangKhuyenMai",
                table: "HoaDonDienTuChiTiets",
                nullable: true);
        }
    }
}
