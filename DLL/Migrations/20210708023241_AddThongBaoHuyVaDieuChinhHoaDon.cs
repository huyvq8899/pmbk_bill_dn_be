using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddThongBaoHuyVaDieuChinhHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongBaoDieuChinhThongTinHoaDons",
                columns: table => new
                {
                    ThongBaoDieuChinhThongTinHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NgayThongBaoDieuChinh = table.Column<DateTime>(nullable: true),
                    NgayThongBaoPhatHanh = table.Column<DateTime>(nullable: true),
                    TaiLieuDinhKem = table.Column<string>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    TrangThaiNop = table.Column<int>(nullable: false),
                    TenDonViCu = table.Column<string>(nullable: true),
                    TenDonViMoi = table.Column<string>(nullable: true),
                    DiaChiCu = table.Column<string>(nullable: true),
                    DiaChiMoi = table.Column<string>(nullable: true),
                    DienThoaiCu = table.Column<string>(nullable: true),
                    DienThoaiMoi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoDieuChinhThongTinHoaDons", x => x.ThongBaoDieuChinhThongTinHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoKetQuaHuyHoaDons",
                columns: table => new
                {
                    ThongBaoKetQuaHuyHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true),
                    NgayGioHuy = table.Column<DateTime>(nullable: true),
                    PhuongPhapHuy = table.Column<string>(nullable: true),
                    TaiLieuDinhKem = table.Column<string>(nullable: true),
                    NgayThongBao = table.Column<DateTime>(nullable: true),
                    TrangThaiNop = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoKetQuaHuyHoaDons", x => x.ThongBaoKetQuaHuyHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                columns: table => new
                {
                    ThongBaoDieuChinhThongTinHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    ThongBaoDieuChinhThongTinHoaDonChiTietId = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoDieuChinhThongTinHoaDonChiTiets", x => x.ThongBaoDieuChinhThongTinHoaDonId);
                    table.ForeignKey(
                        name: "FK_ThongBaoDieuChinhThongTinHoaDonChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaoDieuChinhThongTinHoaDonChiTiets_ThongBaoDieuChinhThongTinHoaDons_ThongBaoDieuChinhThongTinHoaDonId",
                        column: x => x.ThongBaoDieuChinhThongTinHoaDonId,
                        principalTable: "ThongBaoDieuChinhThongTinHoaDons",
                        principalColumn: "ThongBaoDieuChinhThongTinHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoKetQuaHuyHoaDonChiTiets",
                columns: table => new
                {
                    ThongBaoKetQuaHuyHoaDonChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    ThongBaoKetQuaHuyHoaDonId = table.Column<string>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    TuSo = table.Column<int>(nullable: true),
                    DenSo = table.Column<int>(nullable: true),
                    SoLuong = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoKetQuaHuyHoaDonChiTiets", x => x.ThongBaoKetQuaHuyHoaDonChiTietId);
                    table.ForeignKey(
                        name: "FK_ThongBaoKetQuaHuyHoaDonChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaoKetQuaHuyHoaDonChiTiets_ThongBaoKetQuaHuyHoaDons_ThongBaoKetQuaHuyHoaDonId",
                        column: x => x.ThongBaoKetQuaHuyHoaDonId,
                        principalTable: "ThongBaoKetQuaHuyHoaDons",
                        principalColumn: "ThongBaoKetQuaHuyHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_MauHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoKetQuaHuyHoaDonChiTiets_MauHoaDonId",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoKetQuaHuyHoaDonChiTiets_ThongBaoKetQuaHuyHoaDonId",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                column: "ThongBaoKetQuaHuyHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoKetQuaHuyHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.DropTable(
                name: "ThongBaoKetQuaHuyHoaDons");
        }
    }
}
