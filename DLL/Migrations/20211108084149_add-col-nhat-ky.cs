using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addcolnhatky : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToKhaiId",
                table: "NhatKyXacThucBoKyHieus",
                newName: "ThongDiepId");

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiepGui",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoSeriChungThu",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenMauHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiXacThuc",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenToChucChungThuc",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiDiemChapNhan",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianSuDungDen",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianSuDungTu",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianXacThuc",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaThongDiepGui",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "SoSeriChungThu",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "TenMauHoaDon",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "TenNguoiXacThuc",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "TenToChucChungThuc",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "ThoiDiemChapNhan",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "ThoiGianSuDungDen",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "ThoiGianSuDungTu",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "ThoiGianXacThuc",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.RenameColumn(
                name: "ThongDiepId",
                table: "NhatKyXacThucBoKyHieus",
                newName: "ToKhaiId");
        }
    }
}
