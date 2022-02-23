using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class cleandata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuLieuKyToKhais");

            migrationBuilder.DropColumn(
                name: "ContentXMLChuaKy",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "IdThongDiepGoc",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "LanGui",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "LanThu",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "NoiNhan",
                table: "ThongDiepChungs");

            migrationBuilder.AlterColumn<string>(
                name: "PhienBan",
                table: "ThongDiepChungs",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaThongDiepThamChieu",
                table: "ThongDiepChungs",
                maxLength: 46,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaThongDiep",
                table: "ThongDiepChungs",
                maxLength: 46,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepChungs",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaNoiNhan",
                table: "ThongDiepChungs",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaNoiGui",
                table: "ThongDiepChungs",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu56",
                table: "DangKyUyNhiems",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu4",
                table: "DangKyUyNhiems",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu23",
                table: "DangKyUyNhiems",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu1",
                table: "DangKyUyNhiems",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiHoaDon",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienThueGTGT",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThueGTGT",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTien",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenKhachHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SoLuong",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19, 4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayHoaDon",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MauSoHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MauSo",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaKhachHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieuHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HoTenNguoiMuaHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonViTinh",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BangTongHopDuLieuHoaDonId",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoaiHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BangTongHopDuLieuHoaDonChiTiets_BangTongHopDuLieuHoaDonId",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                column: "BangTongHopDuLieuHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BangTongHopDuLieuHoaDonChiTiets_BangTongHopDuLieuHoaDons_BangTongHopDuLieuHoaDonId",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                column: "BangTongHopDuLieuHoaDonId",
                principalTable: "BangTongHopDuLieuHoaDons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BangTongHopDuLieuHoaDonChiTiets_BangTongHopDuLieuHoaDons_BangTongHopDuLieuHoaDonId",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_BangTongHopDuLieuHoaDonChiTiets_BangTongHopDuLieuHoaDonId",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "LoaiHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.AddColumn<byte[]>(
                name: "ContentXMLChuaKy",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhienBan",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaThongDiepThamChieu",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 46,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaThongDiep",
                table: "ThongDiepChungs",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 46);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "MaNoiNhan",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "MaNoiGui",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14);

            migrationBuilder.AddColumn<string>(
                name: "IdThongDiepGoc",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LanGui",
                table: "ThongDiepChungs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanThu",
                table: "ThongDiepChungs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NoiNhan",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu56",
                table: "DangKyUyNhiems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu4",
                table: "DangKyUyNhiems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu23",
                table: "DangKyUyNhiems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu1",
                table: "DangKyUyNhiems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiHoaDon",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19, 4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienThueGTGT",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19, 4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThueGTGT",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTien",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19, 4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenKhachHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SoLuong",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                type: "decimal(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19, 4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayHoaDon",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "MauSoHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MauSo",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "MaKhachHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieuHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieu",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<string>(
                name: "HoTenNguoiMuaHang",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonViTinh",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BangTongHopDuLieuHoaDonId",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DuLieuKyToKhais",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FileXMLDaKy = table.Column<string>(nullable: true),
                    IdToKhai = table.Column<string>(nullable: true),
                    MST = table.Column<string>(nullable: true),
                    NgayKy = table.Column<DateTime>(nullable: true),
                    NoiDungKy = table.Column<byte[]>(nullable: true),
                    Seri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuLieuKyToKhais", x => x.Id);
                });
        }
    }
}
