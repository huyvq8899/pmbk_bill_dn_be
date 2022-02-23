using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Update_hoadon_saisot_rasoat2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XMLData",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "XMLData",
                table: "ThongBaoHoaDonRaSoats");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiNopThue",
                table: "ThongDiepGuiCQTs",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaiDienNguoiNopThue",
                table: "ThongDiepGuiCQTs",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaCoQuanThue",
                table: "ThongDiepGuiCQTs",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaDiaDanh",
                table: "ThongDiepGuiCQTs",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepGuiCQTs",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenCoQuanThue",
                table: "ThongDiepGuiCQTs",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MauHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaCQTCap",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LyDo",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieuHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "STT",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThongBaoChiTietHDRaSoatId",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoThongBaoCuaCQT",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaiDienNguoiNopThue",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaCoQuanThue",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaDiaDanh",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "TenCoQuanThue",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "STT",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "ThongBaoChiTietHDRaSoatId",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiNopThue",
                table: "ThongDiepGuiCQTs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLData",
                table: "ThongDiepGuiCQTs",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MauHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaCQTCap",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LyDo",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KyHieuHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoThongBaoCuaCQT",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLData",
                table: "ThongBaoHoaDonRaSoats",
                nullable: true);
        }
    }
}
