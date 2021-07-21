using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtablebaocaotinhhinhsudunghoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaoCaoTinhHinhSuDungHoaDons",
                columns: table => new
                {
                    BaoCaoTinhHinhSuDungHoaDonId = table.Column<string>(nullable: false),
                    Nam = table.Column<int>(nullable: false),
                    Thang = table.Column<int>(nullable: true),
                    Quy = table.Column<int>(nullable: true),
                    NgayLap = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoTinhHinhSuDungHoaDons", x => x.BaoCaoTinhHinhSuDungHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                columns: table => new
                {
                    BaoCaoTinhHinhSuDungHoaDonChiTietId = table.Column<string>(nullable: false),
                    BaoCaoTinhHinhSuDungHoaDonId = table.Column<string>(nullable: true),
                    TenLoaiHoaDon = table.Column<string>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    TongSo = table.Column<int>(nullable: false),
                    TonDauKyTu = table.Column<string>(nullable: true),
                    TonDauKyDen = table.Column<string>(nullable: true),
                    TrongKyTu = table.Column<string>(nullable: true),
                    TrongKyDen = table.Column<string>(nullable: true),
                    DaSuDung = table.Column<int>(nullable: false),
                    DaXoaBo = table.Column<int>(nullable: false),
                    SoXoaBo = table.Column<string>(nullable: true),
                    DaMat = table.Column<int>(nullable: false),
                    SoMat = table.Column<string>(nullable: true),
                    DaHuy = table.Column<int>(nullable: false),
                    SoHuy = table.Column<string>(nullable: true),
                    TonCuoiKyTu = table.Column<string>(nullable: true),
                    TonCuoiKyDen = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoTinhHinhSuDungHoaDonChiTiets", x => x.BaoCaoTinhHinhSuDungHoaDonChiTietId);
                    table.ForeignKey(
                        name: "FK_BaoCaoTinhHinhSuDungHoaDonChiTiets_BaoCaoTinhHinhSuDungHoaDons_BaoCaoTinhHinhSuDungHoaDonId",
                        column: x => x.BaoCaoTinhHinhSuDungHoaDonId,
                        principalTable: "BaoCaoTinhHinhSuDungHoaDons",
                        principalColumn: "BaoCaoTinhHinhSuDungHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTinhHinhSuDungHoaDonChiTiets_BaoCaoTinhHinhSuDungHoaDonId",
                table: "BaoCaoTinhHinhSuDungHoaDonChiTiets",
                column: "BaoCaoTinhHinhSuDungHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoTinhHinhSuDungHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "BaoCaoTinhHinhSuDungHoaDons");
        }
    }
}
