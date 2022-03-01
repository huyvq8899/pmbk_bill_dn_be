using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addbangtonghopdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "ThoiGianGui",
            //    table: "BangTongHopDuLieuHoaDons",
            //    newName: "ThoiHanGui");

            //migrationBuilder.RenameColumn(
            //    name: "SuaDoiLanThu",
            //    table: "BangTongHopDuLieuHoaDons",
            //    newName: "STT");

            //migrationBuilder.RenameColumn(
            //    name: "TenTrangThaiHoaDon",
            //    table: "BangTongHopDuLieuHoaDonChiTiets",
            //    newName: "STBao");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "BangTongHopDuLieuHoaDons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThongDiepChungId",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SoHoaDon",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KDLDChinh",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LKDLDChinh",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NTBao",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "ThongDiepChungId",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "KDLDChinh",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "LKDLDChinh",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "NTBao",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "NgayHoaDonLienQuan",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.RenameColumn(
                name: "ThoiHanGui",
                table: "BangTongHopDuLieuHoaDons",
                newName: "ThoiGianGui");

            migrationBuilder.RenameColumn(
                name: "STT",
                table: "BangTongHopDuLieuHoaDons",
                newName: "SuaDoiLanThu");

            migrationBuilder.RenameColumn(
                name: "STBao",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                newName: "TenTrangThaiHoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "SoHoaDon",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}
