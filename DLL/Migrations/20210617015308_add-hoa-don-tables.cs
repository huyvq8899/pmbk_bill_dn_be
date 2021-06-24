using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addhoadontables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MauHoaDons",
                columns: table => new
                {
                    MauHoaDonId = table.Column<string>(nullable: false),
                    MauSo = table.Column<string>(nullable: true),
                    TenMauSo = table.Column<string>(nullable: true),
                    DienGiai = table.Column<string>(nullable: true),
                    TuNhap = table.Column<bool>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    STT = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDons", x => x.MauHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonDienTus",
                columns: table => new
                {
                    HoaDonDienTuId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NgayHoaDon = table.Column<DateTime>(nullable: true),
                    SoHoaDon = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    KhachHangId = table.Column<string>(nullable: true),
                    NhanVienBanHangId = table.Column<string>(nullable: true),
                    LoaiTienId = table.Column<string>(nullable: true),
                    TyGia = table.Column<decimal>(nullable: true),
                    TrangThai = table.Column<int>(nullable: true),
                    TrangThaiPhatHanh = table.Column<int>(nullable: true),
                    MaTraCuu = table.Column<string>(nullable: true),
                    TrangThaiGuiHoaDon = table.Column<int>(nullable: true),
                    KhachHangDaNhan = table.Column<bool>(nullable: true),
                    SoLanChuyenDoi = table.Column<int>(nullable: false),
                    LyDoXoaBo = table.Column<string>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    NgayLap = table.Column<DateTime>(nullable: false),
                    NguoiLapId = table.Column<string>(nullable: true),
                    LoaiChungTu = table.Column<int>(nullable: false),
                    ThamChieu = table.Column<string>(nullable: true),
                    TaiLieuDinhKem = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonDienTus", x => x.HoaDonDienTuId);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_DoiTuongs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_LoaiTiens_LoaiTienId",
                        column: x => x.LoaiTienId,
                        principalTable: "LoaiTiens",
                        principalColumn: "LoaiTienId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_DoiTuongs_NguoiLapId",
                        column: x => x.NguoiLapId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_DoiTuongs_NhanVienBanHangId",
                        column: x => x.NhanVienBanHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonDienTuChiTiets",
                columns: table => new
                {
                    HoaDonDienTuChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    HangHoaDichVuId = table.Column<string>(nullable: true),
                    HangKhuyenMai = table.Column<bool>(nullable: true),
                    DonViTinhId = table.Column<string>(nullable: true),
                    SoLuong = table.Column<decimal>(nullable: true),
                    DonGia = table.Column<decimal>(nullable: true),
                    DonGiaQuyDoi = table.Column<decimal>(nullable: true),
                    ThanhTien = table.Column<decimal>(nullable: true),
                    ThanhTienQuyDoi = table.Column<decimal>(nullable: true),
                    TyLeChietKhau = table.Column<decimal>(nullable: true),
                    TienChietKhau = table.Column<decimal>(nullable: true),
                    TienChietKhauQuyDoi = table.Column<decimal>(nullable: true),
                    ThueGTGT = table.Column<decimal>(nullable: true),
                    TienThueGTGT = table.Column<decimal>(nullable: true),
                    TienThueGTGTQuyDoi = table.Column<decimal>(nullable: true),
                    SoLo = table.Column<string>(nullable: true),
                    HanSuDung = table.Column<DateTime>(nullable: true),
                    SoKhung = table.Column<string>(nullable: true),
                    SoMay = table.Column<string>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonDienTuChiTiets", x => x.HoaDonDienTuChiTietId);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTuChiTiets_DonViTinhs_DonViTinhId",
                        column: x => x.DonViTinhId,
                        principalTable: "DonViTinhs",
                        principalColumn: "DonViTinhId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTuChiTiets_HangHoaDichVus_HangHoaDichVuId",
                        column: x => x.HangHoaDichVuId,
                        principalTable: "HangHoaDichVus",
                        principalColumn: "HangHoaDichVuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTuChiTiets_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_DonViTinhId",
                table: "HoaDonDienTuChiTiets",
                column: "DonViTinhId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_HangHoaDichVuId",
                table: "HoaDonDienTuChiTiets",
                column: "HangHoaDichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_HoaDonDienTuId",
                table: "HoaDonDienTuChiTiets",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_KhachHangId",
                table: "HoaDonDienTus",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_LoaiTienId",
                table: "HoaDonDienTus",
                column: "LoaiTienId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_MauHoaDonId",
                table: "HoaDonDienTus",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_NguoiLapId",
                table: "HoaDonDienTus",
                column: "NguoiLapId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_NhanVienBanHangId",
                table: "HoaDonDienTus",
                column: "NhanVienBanHangId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoaDonDienTuChiTiets");

            migrationBuilder.DropTable(
                name: "HoaDonDienTus");

            migrationBuilder.DropTable(
                name: "MauHoaDons");
        }
    }
}
