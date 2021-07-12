using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addbienbanxoabo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrangThaiBBXoaBo",
                table: "HoaDonDienTus",
                newName: "TrangThaiBienBanXoaBo");

            migrationBuilder.CreateTable(
                name: "BienBanXoaBos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NgayBienBan = table.Column<DateTime>(nullable: true),
                    SoBienBan = table.Column<string>(nullable: true),
                    KhachHangId = table.Column<string>(nullable: true),
                    TenKhachHang = table.Column<string>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    SoDienThoai = table.Column<string>(nullable: true),
                    DaiDien = table.Column<string>(nullable: true),
                    ChucVu = table.Column<string>(nullable: true),
                    SoDienThoaiBenB = table.Column<string>(nullable: true),
                    DaiDienBenB = table.Column<string>(nullable: true),
                    ChucVuBenB = table.Column<string>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    LyDoXoaBo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienBanXoaBos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BienBanXoaBos_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BienBanXoaBos_DoiTuongs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienBanXoaBos_HoaDonDienTuId",
                table: "BienBanXoaBos",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_BienBanXoaBos_KhachHangId",
                table: "BienBanXoaBos",
                column: "KhachHangId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BienBanXoaBos");

            migrationBuilder.RenameColumn(
                name: "TrangThaiBienBanXoaBo",
                table: "HoaDonDienTus",
                newName: "TrangThaiBBXoaBo");
        }
    }
}
