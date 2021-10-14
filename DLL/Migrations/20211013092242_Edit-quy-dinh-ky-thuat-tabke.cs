using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Editquydinhkythuattabke : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianGui",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsThemMoi",
                table: "ToKhaiDangKyThongTins",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LoaiUyNhiem",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NhanUyNhiem",
                table: "ToKhaiDangKyThongTins",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKy",
                table: "DuLieuKyToKhais",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiGianGui",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "IsThemMoi",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "LoaiUyNhiem",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "NhanUyNhiem",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "NgayKy",
                table: "DuLieuKyToKhais");
        }
    }
}
