using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addphieuxuatkhoinhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanCuSo",
                table: "HoaDonDienTus",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cua",
                table: "HoaDonDienTus",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiKhoNhanHang",
                table: "HoaDonDienTus",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiKhoXuatHang",
                table: "HoaDonDienTus",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DienGiai",
                table: "HoaDonDienTus",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTenNguoiNhanHang",
                table: "HoaDonDienTus",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTenNguoiXuatHang",
                table: "HoaDonDienTus",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HopDongVanChuyenSo",
                table: "HoaDonDienTus",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayCanCu",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucVanChuyen",
                table: "HoaDonDienTus",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiVanChuyen",
                table: "HoaDonDienTus",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SoLuongNhap",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanCuSo",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "Cua",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "DiaChiKhoNhanHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "DiaChiKhoXuatHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "DienGiai",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HoTenNguoiNhanHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HoTenNguoiXuatHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HopDongVanChuyenSo",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "NgayCanCu",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "PhuongThucVanChuyen",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TenNguoiVanChuyen",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoLuongNhap",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
