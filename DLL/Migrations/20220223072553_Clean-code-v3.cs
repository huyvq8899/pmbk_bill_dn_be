using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Cleancodev3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhThucThanhToans");

            migrationBuilder.DropTable(
                name: "QuyetDinhApDungHoaDonDieu1s");

            migrationBuilder.DropTable(
                name: "QuyetDinhApDungHoaDonDieu2s");

            migrationBuilder.DropTable(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoKetQuaHuyHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhChiTiets");

            migrationBuilder.DropTable(
                name: "QuyetDinhApDungHoaDons");

            migrationBuilder.DropTable(
                name: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.DropTable(
                name: "ThongBaoKetQuaHuyHoaDons");

            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhs");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ToKhaiDangKyThongTins",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<string>(
            //    name: "Id",
            //    table: "DangKyUyNhiems",
            //    maxLength: 36,
            //    nullable: false,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<string>(
            //    name: "Id",
            //    table: "ChungThuSoSuDungs",
            //    maxLength: 36,
            //    nullable: false,
            //    oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ToKhaiDangKyThongTins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DangKyUyNhiems",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ChungThuSoSuDungs",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36);

            migrationBuilder.CreateTable(
                name: "HinhThucThanhToans",
                columns: table => new
                {
                    HinhThucThanhToanId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    MoTa = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Ten = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucThanhToans", x => x.HinhThucThanhToanId);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhApDungHoaDons",
                columns: table => new
                {
                    QuyetDinhApDungHoaDonId = table.Column<string>(nullable: false),
                    CanCuDeBanHanhQuyetDinh = table.Column<string>(nullable: true),
                    ChucDanh = table.Column<string>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Dieu1 = table.Column<string>(nullable: true),
                    Dieu2 = table.Column<string>(nullable: true),
                    Dieu3 = table.Column<string>(nullable: true),
                    Dieu4 = table.Column<string>(nullable: true),
                    Dieu5 = table.Column<int>(nullable: true),
                    HasChungThuSo = table.Column<bool>(nullable: true),
                    HasMayIn = table.Column<bool>(nullable: true),
                    HasMayTinh = table.Column<bool>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    NgayHieuLuc = table.Column<DateTime>(nullable: true),
                    NgayQuyetDinh = table.Column<DateTime>(nullable: true),
                    NguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    NoiDungDieu3 = table.Column<string>(nullable: true),
                    NoiDungDieu4 = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    SoQuyetDinh = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhApDungHoaDons", x => x.QuyetDinhApDungHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoDieuChinhThongTinHoaDons",
                columns: table => new
                {
                    ThongBaoDieuChinhThongTinHoaDonId = table.Column<string>(nullable: false),
                    CoQuanThue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DiaChiCu = table.Column<string>(nullable: true),
                    DiaChiMoi = table.Column<string>(nullable: true),
                    DienThoaiCu = table.Column<string>(nullable: true),
                    DienThoaiMoi = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    NgayThongBaoDieuChinh = table.Column<DateTime>(nullable: true),
                    NgayThongBaoPhatHanh = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    TenDonViCu = table.Column<string>(nullable: true),
                    TenDonViMoi = table.Column<string>(nullable: true),
                    TrangThaiHieuLuc = table.Column<int>(nullable: false)
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
                    CoQuanThue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    NgayGioHuy = table.Column<DateTime>(nullable: true),
                    NgayThongBao = table.Column<DateTime>(nullable: true),
                    PhuongPhapHuy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    TrangThaiNop = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoKetQuaHuyHoaDons", x => x.ThongBaoKetQuaHuyHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoPhatHanhs",
                columns: table => new
                {
                    ThongBaoPhatHanhId = table.Column<string>(nullable: false),
                    CoQuanThue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DienThoai = table.Column<string>(nullable: true),
                    IsXacNhan = table.Column<bool>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Ngay = table.Column<DateTime>(nullable: false),
                    NguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    TrangThaiNop = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoPhatHanhs", x => x.ThongBaoPhatHanhId);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhApDungHoaDonDieu1s",
                columns: table => new
                {
                    QuyetDinhApDungHoaDonDieu1Id = table.Column<string>(nullable: false),
                    Checked = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Disabled = table.Column<bool>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true),
                    LoaiDieu1 = table.Column<int>(nullable: false),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    QuyetDinhApDungHoaDonId = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Ten = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhApDungHoaDonDieu1s", x => x.QuyetDinhApDungHoaDonDieu1Id);
                    table.ForeignKey(
                        name: "FK_QuyetDinhApDungHoaDonDieu1s_QuyetDinhApDungHoaDons_QuyetDinhApDungHoaDonId",
                        column: x => x.QuyetDinhApDungHoaDonId,
                        principalTable: "QuyetDinhApDungHoaDons",
                        principalColumn: "QuyetDinhApDungHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhApDungHoaDonDieu2s",
                columns: table => new
                {
                    QuyetDinhApDungHoaDonDieu2Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    MucDichSuDung = table.Column<string>(nullable: true),
                    QuyetDinhApDungHoaDonId = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhApDungHoaDonDieu2s", x => x.QuyetDinhApDungHoaDonDieu2Id);
                    table.ForeignKey(
                        name: "FK_QuyetDinhApDungHoaDonDieu2s_QuyetDinhApDungHoaDons_QuyetDinhApDungHoaDonId",
                        column: x => x.QuyetDinhApDungHoaDonId,
                        principalTable: "QuyetDinhApDungHoaDons",
                        principalColumn: "QuyetDinhApDungHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                columns: table => new
                {
                    ThongBaoDieuChinhThongTinHoaDonChiTietId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    ThongBaoDieuChinhThongTinHoaDonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoDieuChinhThongTinHoaDonChiTiets", x => x.ThongBaoDieuChinhThongTinHoaDonChiTietId);
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
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DenSo = table.Column<int>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    SoLuong = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    ThongBaoKetQuaHuyHoaDonId = table.Column<string>(nullable: true),
                    TuSo = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ThongBaoPhatHanhChiTiets",
                columns: table => new
                {
                    ThongBaoPhatHanhChiTietId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DenSo = table.Column<int>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    NgayBatDauSuDung = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    SoLuong = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    ThongBaoPhatHanhId = table.Column<string>(nullable: true),
                    TuSo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoPhatHanhChiTiets", x => x.ThongBaoPhatHanhChiTietId);
                    table.ForeignKey(
                        name: "FK_ThongBaoPhatHanhChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaoPhatHanhChiTiets_ThongBaoPhatHanhs_ThongBaoPhatHanhId",
                        column: x => x.ThongBaoPhatHanhId,
                        principalTable: "ThongBaoPhatHanhs",
                        principalColumn: "ThongBaoPhatHanhId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuyetDinhApDungHoaDonDieu1s_QuyetDinhApDungHoaDonId",
                table: "QuyetDinhApDungHoaDonDieu1s",
                column: "QuyetDinhApDungHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_QuyetDinhApDungHoaDonDieu2s_QuyetDinhApDungHoaDonId",
                table: "QuyetDinhApDungHoaDonDieu2s",
                column: "QuyetDinhApDungHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_MauHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_ThongBaoDieuChinhThongTinHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "ThongBaoDieuChinhThongTinHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoKetQuaHuyHoaDonChiTiets_MauHoaDonId",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoKetQuaHuyHoaDonChiTiets_ThongBaoKetQuaHuyHoaDonId",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                column: "ThongBaoKetQuaHuyHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhChiTiets_MauHoaDonId",
                table: "ThongBaoPhatHanhChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhChiTiets_ThongBaoPhatHanhId",
                table: "ThongBaoPhatHanhChiTiets",
                column: "ThongBaoPhatHanhId");
        }
    }
}
