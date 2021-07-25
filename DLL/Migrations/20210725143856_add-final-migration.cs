using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addfinalmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenLoaiHoaDon",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                newName: "SuDungTu");

            migrationBuilder.AddColumn<DateTime>(
                name: "DenNgay",
                table: "BaoCaoTinhHinhSuDungHoaDons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DienGiai",
                table: "BaoCaoTinhHinhSuDungHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NguoiLapId",
                table: "BaoCaoTinhHinhSuDungHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiDaiDienPhapLuat",
                table: "BaoCaoTinhHinhSuDungHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiLap",
                table: "BaoCaoTinhHinhSuDungHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TuNgay",
                table: "BaoCaoTinhHinhSuDungHoaDons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LoaiHoaDon",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoLuongTon",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SuDungDen",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TongSoSuDung",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DenNgay",
                table: "BaoCaoTinhHinhSuDungHoaDons");

            migrationBuilder.DropColumn(
                name: "DienGiai",
                table: "BaoCaoTinhHinhSuDungHoaDons");

            migrationBuilder.DropColumn(
                name: "NguoiLapId",
                table: "BaoCaoTinhHinhSuDungHoaDons");

            migrationBuilder.DropColumn(
                name: "TenNguoiDaiDienPhapLuat",
                table: "BaoCaoTinhHinhSuDungHoaDons");

            migrationBuilder.DropColumn(
                name: "TenNguoiLap",
                table: "BaoCaoTinhHinhSuDungHoaDons");

            migrationBuilder.DropColumn(
                name: "TuNgay",
                table: "BaoCaoTinhHinhSuDungHoaDons");

            migrationBuilder.DropColumn(
                name: "LoaiHoaDon",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "SoLuongTon",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "SuDungDen",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "TongSoSuDung",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets");

            migrationBuilder.RenameColumn(
                name: "SuDungTu",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                newName: "TenLoaiHoaDon");
        }
    }
}
