using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoltohoadondientu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MauSo",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenMauSo",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaHang",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenHang",
                table: "HoaDonDienTuChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MauSo",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TenMauSo",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "MaHang",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TenHang",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
