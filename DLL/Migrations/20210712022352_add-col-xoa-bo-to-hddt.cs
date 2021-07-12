using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcolxoabotohddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayXoaBo",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoCTXoaBo",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiBBXoaBo",
                table: "HoaDonDienTus",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayXoaBo",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoCTXoaBo",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TrangThaiBBXoaBo",
                table: "HoaDonDienTus");
        }
    }
}
