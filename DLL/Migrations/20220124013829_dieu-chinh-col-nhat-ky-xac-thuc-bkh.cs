using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhcolnhatkyxacthucbkh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHetSoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.AddColumn<int>(
                name: "LoaiHetHieuLuc",
                table: "NhatKyXacThucBoKyHieus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiHetHieuLuc",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "NgayHoaDon",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.AddColumn<bool>(
                name: "IsHetSoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);
        }
    }
}
