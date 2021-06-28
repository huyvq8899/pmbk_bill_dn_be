using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtablehinhthucthanhtoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailNguoiMuaHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhThucThanhToanId",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTenNguoiMuaHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoaiNguoiMuaHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoTaiKhoanNganHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenKhachHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNganHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HinhThucThanhToans",
                columns: table => new
                {
                    HinhThucThanhToanId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucThanhToans", x => x.HinhThucThanhToanId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_HinhThucThanhToanId",
                table: "HoaDonDienTus",
                column: "HinhThucThanhToanId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_HinhThucThanhToans_HinhThucThanhToanId",
                table: "HoaDonDienTus",
                column: "HinhThucThanhToanId",
                principalTable: "HinhThucThanhToans",
                principalColumn: "HinhThucThanhToanId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_HinhThucThanhToans_HinhThucThanhToanId",
                table: "HoaDonDienTus");

            migrationBuilder.DropTable(
                name: "HinhThucThanhToans");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_HinhThucThanhToanId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "EmailNguoiMuaHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HinhThucThanhToanId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HoTenNguoiMuaHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoDienThoaiNguoiMuaHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoTaiKhoanNganHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TenKhachHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TenNganHang",
                table: "HoaDonDienTus");
        }
    }
}
