using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhthongdiepguidulieutable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongDiepGuiHDDTKhongMaBytes");

            migrationBuilder.DropTable(
                name: "ThongDiepGuiHDDTKhongMaDuLieus");

            migrationBuilder.DropTable(
                name: "ThongDiepGuiHDDTKhongMas");

            migrationBuilder.CreateTable(
                name: "ThongDiepGuiDuLieuHDDTs",
                columns: table => new
                {
                    ThongDiepGuiDuLieuHDDTId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    PhienBan = table.Column<string>(nullable: true),
                    MaNoiGui = table.Column<string>(nullable: true),
                    MaNoiNhan = table.Column<string>(nullable: true),
                    MaLoaiThongDiep = table.Column<string>(nullable: true),
                    MaThongDiep = table.Column<string>(nullable: true),
                    MaThongDiepThamChieu = table.Column<string>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    SoLuong = table.Column<int>(nullable: false),
                    FileXMLGui = table.Column<string>(nullable: true),
                    FileXMLNhan = table.Column<string>(nullable: true),
                    TrangThaiGui = table.Column<int>(nullable: false),
                    TrangThaiTiepNhan = table.Column<int>(nullable: false),
                    HoaDonDienTuId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepGuiDuLieuHDDTs", x => x.ThongDiepGuiDuLieuHDDTId);
                });

            migrationBuilder.CreateTable(
                name: "ThongDiepGuiDuLieuHDDTChiTiets",
                columns: table => new
                {
                    ThongDiepGuiDuLieuHDDTChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    ThongDiepGuiDuLieuHDDTId = table.Column<string>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepGuiDuLieuHDDTChiTiets", x => x.ThongDiepGuiDuLieuHDDTChiTietId);
                    table.ForeignKey(
                        name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_ThongDiepGuiDuLieuHDDTId",
                        column: x => x.ThongDiepGuiDuLieuHDDTId,
                        principalTable: "ThongDiepGuiDuLieuHDDTs",
                        principalColumn: "ThongDiepGuiDuLieuHDDTId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepGuiDuLieuHDDTChiTiets_HoaDonDienTuId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "ThongDiepGuiDuLieuHDDTId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.DropTable(
                name: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.CreateTable(
                name: "ThongDiepGuiHDDTKhongMas",
                columns: table => new
                {
                    ThongDiepGuiHDDTKhongMaId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    FileXMLGui = table.Column<string>(nullable: true),
                    FileXMLNhan = table.Column<string>(nullable: true),
                    MaLoaiThongDiep = table.Column<string>(nullable: true),
                    MaNoiGui = table.Column<string>(nullable: true),
                    MaNoiNhan = table.Column<string>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    MaThongDiep = table.Column<string>(nullable: true),
                    MaThongDiepThamChieu = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    PhienBan = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    SoLuong = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    TrangThaiGui = table.Column<int>(nullable: false),
                    TrangThaiTiepNhan = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepGuiHDDTKhongMas", x => x.ThongDiepGuiHDDTKhongMaId);
                });

            migrationBuilder.CreateTable(
                name: "ThongDiepGuiHDDTKhongMaBytes",
                columns: table => new
                {
                    ThongDiepGuiHDDTKhongMaByteId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    FileXMLGuiByte = table.Column<string>(nullable: true),
                    FileXMLNhanByte = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    ThongDiepGuiHDDTKhongMaId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepGuiHDDTKhongMaBytes", x => x.ThongDiepGuiHDDTKhongMaByteId);
                    table.ForeignKey(
                        name: "FK_ThongDiepGuiHDDTKhongMaBytes_ThongDiepGuiHDDTKhongMas_ThongDiepGuiHDDTKhongMaId",
                        column: x => x.ThongDiepGuiHDDTKhongMaId,
                        principalTable: "ThongDiepGuiHDDTKhongMas",
                        principalColumn: "ThongDiepGuiHDDTKhongMaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongDiepGuiHDDTKhongMaDuLieus",
                columns: table => new
                {
                    ThongDiepGuiHDDTKhongMaDuLieuId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    ThongDiepGuiHDDTKhongMaId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepGuiHDDTKhongMaDuLieus", x => x.ThongDiepGuiHDDTKhongMaDuLieuId);
                    table.ForeignKey(
                        name: "FK_ThongDiepGuiHDDTKhongMaDuLieus_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongDiepGuiHDDTKhongMaDuLieus_ThongDiepGuiHDDTKhongMas_ThongDiepGuiHDDTKhongMaId",
                        column: x => x.ThongDiepGuiHDDTKhongMaId,
                        principalTable: "ThongDiepGuiHDDTKhongMas",
                        principalColumn: "ThongDiepGuiHDDTKhongMaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepGuiHDDTKhongMaBytes_ThongDiepGuiHDDTKhongMaId",
                table: "ThongDiepGuiHDDTKhongMaBytes",
                column: "ThongDiepGuiHDDTKhongMaId",
                unique: true,
                filter: "[ThongDiepGuiHDDTKhongMaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepGuiHDDTKhongMaDuLieus_HoaDonDienTuId",
                table: "ThongDiepGuiHDDTKhongMaDuLieus",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepGuiHDDTKhongMaDuLieus_ThongDiepGuiHDDTKhongMaId",
                table: "ThongDiepGuiHDDTKhongMaDuLieus",
                column: "ThongDiepGuiHDDTKhongMaId");
        }
    }
}
