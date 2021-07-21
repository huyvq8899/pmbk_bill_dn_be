using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddBoSungNhatKyGuiEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Ngay",
                table: "NhatKyGuiEmails",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailGui",
                table: "NhatKyGuiEmails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoaiEmail",
                table: "NhatKyGuiEmails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TieuDeEmail",
                table: "NhatKyGuiEmails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileChuaKy",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLChuaKy",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLDaKy",
                table: "BienBanXoaBos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailGui",
                table: "NhatKyGuiEmails");

            migrationBuilder.DropColumn(
                name: "LoaiEmail",
                table: "NhatKyGuiEmails");

            migrationBuilder.DropColumn(
                name: "TieuDeEmail",
                table: "NhatKyGuiEmails");

            migrationBuilder.DropColumn(
                name: "FileChuaKy",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "XMLChuaKy",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "XMLDaKy",
                table: "BienBanXoaBos");

            migrationBuilder.AlterColumn<string>(
                name: "Ngay",
                table: "NhatKyGuiEmails",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
