using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddieuchinhhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DieuChinhChoHoaDonId",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoaiDieuChinh",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDoDieuChinh",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BienBanDieuChinhs",
                columns: table => new
                {
                    BienBanDieuChinhId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NoiDungBienBan = table.Column<string>(nullable: true),
                    NgayBienBan = table.Column<DateTime>(nullable: true),
                    TenDonViBenA = table.Column<string>(nullable: true),
                    DiaChiBenA = table.Column<string>(nullable: true),
                    MaSoThueBenA = table.Column<string>(nullable: true),
                    SoDienThoaiBenA = table.Column<string>(nullable: true),
                    DaiDienBenA = table.Column<string>(nullable: true),
                    ChucVuBenA = table.Column<string>(nullable: true),
                    NgayKyBenA = table.Column<DateTime>(nullable: true),
                    TenDonViBenB = table.Column<string>(nullable: true),
                    DiaChiBenB = table.Column<string>(nullable: true),
                    MaSoThueBenB = table.Column<string>(nullable: true),
                    SoDienThoaiBenB = table.Column<string>(nullable: true),
                    DaiDienBenB = table.Column<string>(nullable: true),
                    ChucVuBenB = table.Column<string>(nullable: true),
                    NgayKyBenB = table.Column<DateTime>(nullable: true),
                    LyDoDieuChinh = table.Column<string>(nullable: true),
                    TrangThaiBienBan = table.Column<int>(nullable: true),
                    FileDaKy = table.Column<string>(nullable: true),
                    FileChuaKy = table.Column<string>(nullable: true),
                    XMLChuaKy = table.Column<string>(nullable: true),
                    XMLDaKy = table.Column<string>(nullable: true),
                    HoaDonBiDieuChinhId = table.Column<string>(nullable: true),
                    HoaDonDieuChinhId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienBanDieuChinhs", x => x.BienBanDieuChinhId);
                    table.ForeignKey(
                        name: "FK_BienBanDieuChinhs_HoaDonDienTus_HoaDonBiDieuChinhId",
                        column: x => x.HoaDonBiDieuChinhId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BienBanDieuChinhs_HoaDonDienTus_HoaDonDieuChinhId",
                        column: x => x.HoaDonDieuChinhId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LuuTruTrangThaiBBDTs",
                columns: table => new
                {
                    LuuTruTrangThaiBBDTId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    BienBanDieuChinhId = table.Column<string>(nullable: true),
                    PdfChuaKy = table.Column<byte[]>(nullable: true),
                    PdfDaKy = table.Column<byte[]>(nullable: true),
                    XMLChuaKy = table.Column<byte[]>(nullable: true),
                    XMLDaKy = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuuTruTrangThaiBBDTs", x => x.LuuTruTrangThaiBBDTId);
                    table.ForeignKey(
                        name: "FK_LuuTruTrangThaiBBDTs_BienBanDieuChinhs_BienBanDieuChinhId",
                        column: x => x.BienBanDieuChinhId,
                        principalTable: "BienBanDieuChinhs",
                        principalColumn: "BienBanDieuChinhId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienBanDieuChinhs_HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs",
                column: "HoaDonBiDieuChinhId");

            migrationBuilder.CreateIndex(
                name: "IX_BienBanDieuChinhs_HoaDonDieuChinhId",
                table: "BienBanDieuChinhs",
                column: "HoaDonDieuChinhId");

            migrationBuilder.CreateIndex(
                name: "IX_LuuTruTrangThaiBBDTs_BienBanDieuChinhId",
                table: "LuuTruTrangThaiBBDTs",
                column: "BienBanDieuChinhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LuuTruTrangThaiBBDTs");

            migrationBuilder.DropTable(
                name: "BienBanDieuChinhs");

            migrationBuilder.DropColumn(
                name: "DieuChinhChoHoaDonId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "LoaiDieuChinh",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "LyDoDieuChinh",
                table: "HoaDonDienTus");
        }
    }
}
