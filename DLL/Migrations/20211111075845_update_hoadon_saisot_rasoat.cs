using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class update_hoadon_saisot_rasoat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "XMLData",
                table: "ThongDiepGuiCQTs",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "XMLData",
                table: "ThongBaoHoaDonRaSoats",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "XMLData",
                table: "ThongDiepGuiCQTs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "XMLData",
                table: "ThongBaoHoaDonRaSoats",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
