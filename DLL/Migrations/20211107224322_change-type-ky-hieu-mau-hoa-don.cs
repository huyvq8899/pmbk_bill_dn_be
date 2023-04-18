using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changetypekyhieumauhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "KyHieuMauSoHoaDon",
                table: "BoKyHieuHoaDons",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KyHieuMauSoHoaDon",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
