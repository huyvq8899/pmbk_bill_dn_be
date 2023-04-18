using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addthongbaotrangthaixoabo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaGuiThongBaoXoaBoHoaDon",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailNguoiNhan",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoaiNguoiNhan",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiNhan",
                table: "BienBanXoaBos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaGuiThongBaoXoaBoHoaDon",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "EmailNguoiNhan",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "SoDienThoaiNguoiNhan",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "TenNguoiNhan",
                table: "BienBanXoaBos");
        }
    }
}
