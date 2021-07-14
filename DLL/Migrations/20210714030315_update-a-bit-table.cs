using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updateabittable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoDienThoaiBenB",
                table: "BienBanXoaBos",
                newName: "TenCongTyBenA");

            migrationBuilder.RenameColumn(
                name: "DaiDienBenB",
                table: "BienBanXoaBos",
                newName: "SoDienThoaiBenA");

            migrationBuilder.RenameColumn(
                name: "ChucVuBenB",
                table: "BienBanXoaBos",
                newName: "MaSoThueBenA");

            migrationBuilder.AddColumn<byte[]>("XMLDaKyTmp", "LuuTruTrangThaiFileHDDTs", nullable: true);
            migrationBuilder.Sql("Update LuuTruTrangThaiFileHDDTs SET XMLDaKyTmp = Convert(varbinary, XMLDaKy)");
            migrationBuilder.DropColumn("XMLDaKy", "LuuTruTrangThaiFileHDDTs");
            migrationBuilder.RenameColumn("XMLDaKyTmp", "LuuTruTrangThaiFileHDDTs", "XMLDaKy");

            migrationBuilder.AddColumn<byte[]>("XMLChuaKyTmp", "LuuTruTrangThaiFileHDDTs", nullable: true);
            migrationBuilder.Sql("Update LuuTruTrangThaiFileHDDTs SET XMLChuaKyTmp = Convert(varbinary, XMLChuaKy)");
            migrationBuilder.DropColumn("XMLChuaKy", "LuuTruTrangThaiFileHDDTs");
            migrationBuilder.RenameColumn("XMLChuaKyTmp", "LuuTruTrangThaiFileHDDTs", "XMLChuaKy");

            migrationBuilder.AddColumn<byte[]>("PdfDaKyTmp", "LuuTruTrangThaiFileHDDTs", nullable: true);
            migrationBuilder.Sql("Update LuuTruTrangThaiFileHDDTs SET PdfDaKyTmp = Convert(varbinary, PdfDaKy)");
            migrationBuilder.DropColumn("PdfDaKy","LuuTruTrangThaiFileHDDTs");
            migrationBuilder.RenameColumn("PdfDaKyTmp", "LuuTruTrangThaiFileHDDTs", "PdfDaKy");

            migrationBuilder.AddColumn<byte[]>("PdfChuaKyTmp", "LuuTruTrangThaiFileHDDTs", nullable: true);
            migrationBuilder.Sql("Update LuuTruTrangThaiFileHDDTs SET PdfChuaKyTmp = Convert(varbinary, PdfChuaKy)");
            migrationBuilder.DropColumn("PdfChuaKy","LuuTruTrangThaiFileHDDTs");
            migrationBuilder.RenameColumn("PdfChuaKyTmp", "LuuTruTrangThaiFileHDDTs", "PdfChuaKy");

            migrationBuilder.AddColumn<string>(
                name: "ChucVuBenA",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaiDienBenA",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiBenA",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileDaKy",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKyBenA",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKyBenB",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LuuTruTrangThaiBBXBs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BienBanXoaBoId = table.Column<string>(nullable: true),
                    PdfChuaKy = table.Column<byte[]>(nullable: true),
                    PdfDaKy = table.Column<byte[]>(nullable: true),
                    XMLChuaKy = table.Column<byte[]>(nullable: true),
                    XMLDaKy = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuuTruTrangThaiBBXBs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LuuTruTrangThaiBBXBs_BienBanXoaBos_BienBanXoaBoId",
                        column: x => x.BienBanXoaBoId,
                        principalTable: "BienBanXoaBos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LuuTruTrangThaiBBXBs_BienBanXoaBoId",
                table: "LuuTruTrangThaiBBXBs",
                column: "BienBanXoaBoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LuuTruTrangThaiBBXBs");

            migrationBuilder.DropColumn(
                name: "ChucVuBenA",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "DaiDienBenA",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "DiaChiBenA",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "FileDaKy",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "NgayKyBenA",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "NgayKyBenB",
                table: "BienBanXoaBos");

            migrationBuilder.RenameColumn(
                name: "TenCongTyBenA",
                table: "BienBanXoaBos",
                newName: "SoDienThoaiBenB");

            migrationBuilder.RenameColumn(
                name: "SoDienThoaiBenA",
                table: "BienBanXoaBos",
                newName: "DaiDienBenB");

            migrationBuilder.RenameColumn(
                name: "MaSoThueBenA",
                table: "BienBanXoaBos",
                newName: "ChucVuBenB");

            migrationBuilder.AlterColumn<string>(
                name: "XMLDaKy",
                table: "LuuTruTrangThaiFileHDDTs",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "XMLChuaKy",
                table: "LuuTruTrangThaiFileHDDTs",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PdfDaKy",
                table: "LuuTruTrangThaiFileHDDTs",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PdfChuaKy",
                table: "LuuTruTrangThaiFileHDDTs",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }
    }
}
