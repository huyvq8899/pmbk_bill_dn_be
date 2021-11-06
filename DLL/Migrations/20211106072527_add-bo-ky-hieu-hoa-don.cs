using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addbokyhieuhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoKyHieuHoaDons",
                columns: table => new
                {
                    BoKyHieuHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    UyNhiemLapHoaDon = table.Column<int>(nullable: false),
                    HinhThucHoaDon = table.Column<int>(nullable: false),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    KyHieu = table.Column<string>(nullable: true),
                    KyHieuMauSoHoaDon = table.Column<string>(nullable: true),
                    KyHieuHoaDon = table.Column<string>(nullable: true),
                    KyHieu1 = table.Column<string>(nullable: true),
                    KyHieu23 = table.Column<string>(nullable: true),
                    KyHieu4 = table.Column<string>(nullable: true),
                    KyHieu56 = table.Column<string>(nullable: true),
                    SoBatDau = table.Column<int>(nullable: true),
                    SoLonNhatDaLapDenHienTai = table.Column<int>(nullable: true),
                    SoToiDa = table.Column<int>(nullable: true),
                    IsTuyChinh = table.Column<bool>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    DangKyUyNhiemId = table.Column<string>(nullable: true),
                    TrangThaiSuDung = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoKyHieuHoaDons", x => x.BoKyHieuHoaDonId);
                    table.ForeignKey(
                        name: "FK_BoKyHieuHoaDons_DangKyUyNhiems_DangKyUyNhiemId",
                        column: x => x.DangKyUyNhiemId,
                        principalTable: "DangKyUyNhiems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoKyHieuHoaDons_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyXacThucBoKyHieus",
                columns: table => new
                {
                    NhatKyXacThucBoKyHieuId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    BoKyHieuHoaDonId = table.Column<string>(nullable: true),
                    TrangThaiSuDung = table.Column<int>(nullable: false),
                    NoiDung = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    ToKhaiId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyXacThucBoKyHieus", x => x.NhatKyXacThucBoKyHieuId);
                    table.ForeignKey(
                        name: "FK_NhatKyXacThucBoKyHieus_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                        column: x => x.BoKyHieuHoaDonId,
                        principalTable: "BoKyHieuHoaDons",
                        principalColumn: "BoKyHieuHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoKyHieuHoaDons_DangKyUyNhiemId",
                table: "BoKyHieuHoaDons",
                column: "DangKyUyNhiemId");

            migrationBuilder.CreateIndex(
                name: "IX_BoKyHieuHoaDons_MauHoaDonId",
                table: "BoKyHieuHoaDons",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyXacThucBoKyHieus_BoKyHieuHoaDonId",
                table: "NhatKyXacThucBoKyHieus",
                column: "BoKyHieuHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropTable(
                name: "BoKyHieuHoaDons");
        }
    }
}
