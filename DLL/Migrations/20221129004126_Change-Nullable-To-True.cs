using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeNullableToTrue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeUpdateByNSD",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "PhatHanhNgayPos",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeUpdateByNSD",
                table: "HoaDonDienTus",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PhatHanhNgayPos",
                table: "HoaDonDienTus",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
