using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddColThayTheHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_DoiTuongs_NguoiLapId",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_NguoiLapId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "NgayLap",
                table: "HoaDonDienTus");

            migrationBuilder.RenameColumn(
                name: "NguoiLapId",
                table: "HoaDonDienTus",
                newName: "ThayTheChoHoaDonId");

            migrationBuilder.AlterColumn<string>(
                name: "ThayTheChoHoaDonId",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDoThayThe",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDoThayThe",
                table: "HoaDonDienTus");

            migrationBuilder.RenameColumn(
                name: "ThayTheChoHoaDonId",
                table: "HoaDonDienTus",
                newName: "NguoiLapId");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiLapId",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayLap",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_NguoiLapId",
                table: "HoaDonDienTus",
                column: "NguoiLapId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_DoiTuongs_NguoiLapId",
                table: "HoaDonDienTus",
                column: "NguoiLapId",
                principalTable: "DoiTuongs",
                principalColumn: "DoiTuongId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
