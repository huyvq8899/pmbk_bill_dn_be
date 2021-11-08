using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addphuongphaptinhthuegtgt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhuongPhapTinhThueGTGT",
                table: "HoSoHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NNT",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NamDuLieu",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDuLieu",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuyDuLieu",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuaDoiLanThu",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThangDuLieu",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhuongPhapTinhThueGTGT",
                table: "HoSoHDDTs");

            migrationBuilder.DropColumn(
                name: "NNT",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "NamDuLieu",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "NgayDuLieu",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "QuyDuLieu",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "SuaDoiLanThu",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "ThangDuLieu",
                table: "BangTongHopDuLieuHoaDons");
        }
    }
}
