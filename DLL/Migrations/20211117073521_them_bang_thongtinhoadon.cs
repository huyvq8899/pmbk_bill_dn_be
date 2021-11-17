using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_bang_thongtinhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileXMLDaKy",
                table: "ThongDiepGuiCQTs",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileDinhKem",
                table: "ThongDiepGuiCQTs",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileDinhKem",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);
           
            migrationBuilder.CreateTable(
                name: "ThongTinHoaDons",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    HinhThucApDung = table.Column<byte>(nullable: false),
                    MaCQTCap = table.Column<string>(maxLength: 40, nullable: true),
                    MauSoHoaDon = table.Column<string>(maxLength: 15, nullable: true),
                    KyHieuHoaDon = table.Column<string>(maxLength: 10, nullable: true),
                    SoHoaDon = table.Column<string>(maxLength: 10, nullable: true),
                    NgayHoaDon = table.Column<DateTime>(nullable: true),
                    FileDinhKem = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinHoaDons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongTinHoaDons");

            migrationBuilder.AlterColumn<string>(
                name: "FileXMLDaKy",
                table: "ThongDiepGuiCQTs",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileDinhKem",
                table: "ThongDiepGuiCQTs",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileDinhKem",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
