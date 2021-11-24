using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_bang_tbao_hoa_don_ra_soat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongBaoChiTietHoaDonRaSoats",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    ThongBaoHoaDonRaSoatId = table.Column<string>(maxLength: 36, nullable: true),
                    MauHoaDon = table.Column<string>(maxLength: 15, nullable: true),
                    KyHieuHoaDon = table.Column<string>(maxLength: 10, nullable: true),
                    SoHoaDon = table.Column<string>(maxLength: 10, nullable: true),
                    NgayLapHoaDon = table.Column<DateTime>(nullable: true),
                    LoaiApDungHD = table.Column<byte>(nullable: false),
                    LyDoRaSoat = table.Column<string>(maxLength: 300, nullable: true),
                    DaGuiThongBao = table.Column<bool>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 36, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoChiTietHoaDonRaSoats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoHoaDonRaSoats",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    SoThongBaoCuaCQT = table.Column<string>(maxLength: 50, nullable: true),
                    NgayThongBao = table.Column<DateTime>(nullable: false),
                    TenCQTCapTren = table.Column<string>(maxLength: 120, nullable: true),
                    TenCQTRaThongBao = table.Column<string>(maxLength: 120, nullable: true),
                    TenNguoiNopThue = table.Column<string>(maxLength: 400, nullable: true),
                    MaSoThue = table.Column<string>(maxLength: 20, nullable: true),
                    ThoiHan = table.Column<byte>(nullable: false),
                    Lan = table.Column<byte>(nullable: false),
                    HinhThuc = table.Column<string>(maxLength: 50, nullable: true),
                    ChucDanh = table.Column<string>(maxLength: 50, nullable: true),
                    FileDinhKem = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 36, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoHoaDonRaSoats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongBaoChiTietHoaDonRaSoats");

            migrationBuilder.DropTable(
                name: "ThongBaoHoaDonRaSoats");
        }
    }
}
