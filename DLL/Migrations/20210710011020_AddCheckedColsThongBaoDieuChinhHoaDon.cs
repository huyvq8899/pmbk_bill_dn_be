﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddCheckedColsThongBaoDieuChinhHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedDiaChi",
                table: "ThongBaoDieuChinhThongTinHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedDienThoai",
                table: "ThongBaoDieuChinhThongTinHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedTenDonVi",
                table: "ThongBaoDieuChinhThongTinHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheckedDiaChi",
                table: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "IsCheckedDienThoai",
                table: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "IsCheckedTenDonVi",
                table: "ThongBaoDieuChinhThongTinHoaDons");
        }
    }
}
