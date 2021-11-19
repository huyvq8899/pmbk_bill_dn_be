using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_cot_thongtinhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ThongTinHoaDons",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ThongTinHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "ThongTinHoaDons",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "ThongTinHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiBienBanXoaBo",
                table: "ThongTinHoaDons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "TrangThaiBienBanXoaBo",
                table: "ThongTinHoaDons");
        }
    }
}
