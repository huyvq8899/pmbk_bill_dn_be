using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addngaykyhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiGui",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKy",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayKy",
                table: "HoaDonDienTus");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiGui",
                table: "ThongDiepChungs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
