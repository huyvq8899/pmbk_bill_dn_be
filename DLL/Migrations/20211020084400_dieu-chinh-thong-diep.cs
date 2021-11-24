using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhthongdiep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_ThongDiepGuiDuLieuHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.DropColumn(
                name: "FileXMLGui",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "FileXMLNhan",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "MaLoaiThongDiep",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "MaNoiGui",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "MaNoiNhan",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "MaThongDiep",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "MaThongDiepThamChieu",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "PhienBan",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "TrangThaiGui",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropColumn(
                name: "TrangThaiTiepNhan",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.RenameColumn(
                name: "ThongDiepGuiDuLieuHDDTId",
                table: "ThongDiepGuiDuLieuHDDTs",
                newName: "DuLieuGuiHDDTId");

            migrationBuilder.RenameColumn(
                name: "ThongDiepGuiDuLieuHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "DuLieuGuiHDDTId");

            migrationBuilder.RenameColumn(
                name: "ThongDiepGuiDuLieuHDDTChiTietId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "DuLieuGuiHDDTChiTietId");

            migrationBuilder.RenameIndex(
                name: "IX_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "IX_ThongDiepGuiDuLieuHDDTChiTiets_DuLieuGuiHDDTId");

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "ThongDiepChungs",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileXML",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThongBao",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "DuLieuGuiHDDTId",
                principalTable: "ThongDiepGuiDuLieuHDDTs",
                principalColumn: "DuLieuGuiHDDTId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.DropColumn(
                name: "FileXML",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "NgayThongBao",
                table: "ThongDiepChungs");

            migrationBuilder.RenameColumn(
                name: "DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTs",
                newName: "ThongDiepGuiDuLieuHDDTId");

            migrationBuilder.RenameColumn(
                name: "DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "ThongDiepGuiDuLieuHDDTId");

            migrationBuilder.RenameColumn(
                name: "DuLieuGuiHDDTChiTietId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "ThongDiepGuiDuLieuHDDTChiTietId");

            migrationBuilder.RenameIndex(
                name: "IX_ThongDiepGuiDuLieuHDDTChiTiets_DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "IX_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTId");

            migrationBuilder.AddColumn<string>(
                name: "FileXMLGui",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileXMLNhan",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaLoaiThongDiep",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaNoiGui",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaNoiNhan",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiep",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiepThamChieu",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhienBan",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiGui",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiTiepNhan",
                table: "ThongDiepGuiDuLieuHDDTs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SoLuong",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_ThongDiepGuiDuLieuHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "ThongDiepGuiDuLieuHDDTId",
                principalTable: "ThongDiepGuiDuLieuHDDTs",
                principalColumn: "ThongDiepGuiDuLieuHDDTId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
