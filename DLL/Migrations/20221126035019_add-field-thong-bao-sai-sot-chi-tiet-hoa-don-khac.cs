using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addfieldthongbaosaisotchitiethoadonkhac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KyHieu1",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu23",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu4",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu56",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieuHoaDon_2KyTuDau",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieuHoaDon_2KyTuDau_DatIn",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieuHoaDon_2SoCuoiNamThongBao",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieuHoaDon_HinhThucHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaCQT12",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaCQTChuoiKyTuSo",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaCQTToKhaiChapNhan",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MauSoHoaDon_LoaiHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MauSoHoaDon_SoLienHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MauSoHoaDon_SoThuTuMau",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KyHieu1",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieu23",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieu4",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieu56",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieuHoaDon_2KyTuDau",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieuHoaDon_2KyTuDau_DatIn",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieuHoaDon_2SoCuoiNamThongBao",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "KyHieuHoaDon_HinhThucHoaDon",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaCQT12",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaCQTChuoiKyTuSo",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaCQTToKhaiChapNhan",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MauSoHoaDon_LoaiHoaDon",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MauSoHoaDon_SoLienHoaDon",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MauSoHoaDon_SoThuTuMau",
                table: "ThongDiepChiTietGuiCQTs");
        }
    }
}
