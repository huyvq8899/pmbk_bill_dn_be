using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_bang_thong_diep_gui : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongDiepChiTietGuiCQTs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    ThongDiepGuiCQTId = table.Column<string>(maxLength: 36, nullable: true),
                    HoaDonDienTuId = table.Column<string>(maxLength: 36, nullable: true),
                    MaCQTCap = table.Column<string>(maxLength: 50, nullable: true),
                    MauHoaDon = table.Column<string>(maxLength: 50, nullable: true),
                    KyHieuHoaDon = table.Column<string>(maxLength: 50, nullable: true),
                    SoHoaDon = table.Column<string>(maxLength: 50, nullable: true),
                    NgayLapHoaDon = table.Column<DateTime>(nullable: true),
                    LoaiApDungHoaDon = table.Column<byte>(nullable: false),
                    PhanLoaiHDSaiSot = table.Column<byte>(nullable: false),
                    LyDo = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 36, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepChiTietGuiCQTs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThongDiepGuiCQTs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    MaThongDiep = table.Column<string>(maxLength: 50, nullable: true),
                    NgayGui = table.Column<DateTime>(nullable: false),
                    NgayLap = table.Column<DateTime>(nullable: false),
                    FileDinhKem = table.Column<string>(nullable: true),
                    NguoiNopThue = table.Column<string>(maxLength: 50, nullable: true),
                    DiaDanh = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 36, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepGuiCQTs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropTable(
                name: "ThongDiepGuiCQTs");
        }
    }
}
