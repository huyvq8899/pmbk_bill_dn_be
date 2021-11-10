using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthongtinchungtotokhai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "STT",
                table: "ToKhaiDangKyThongTins",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ToKhaiDangKyThongTins",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "STT",
                table: "ToKhaiDangKyThongTins");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ToKhaiDangKyThongTins");
        }
    }
}
