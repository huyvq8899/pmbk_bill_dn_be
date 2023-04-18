using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_tbao_saisot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDaLapThongBao04",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LanGui04",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayGuiTBaoSaiSotKhongPhaiLapHD",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiGui04",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDaLapThongBao04",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "LanGui04",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "NgayGuiTBaoSaiSotKhongPhaiLapHD",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TrangThaiGui04",
                table: "HoaDonDienTus");
        }
    }
}
