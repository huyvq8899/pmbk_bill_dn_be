using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addbangtonghopdulieuhoadontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BangTongHopDuLieuHoaDonChiTiets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BangTongHopDuLieuHoaDonId = table.Column<string>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    SoHoaDon = table.Column<string>(nullable: true),
                    NgayHoaDon = table.Column<DateTime>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    MaKhachHang = table.Column<string>(nullable: true),
                    TenKhachHang = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    HoTenNguoiMuaHang = table.Column<string>(nullable: true),
                    MaHang = table.Column<string>(nullable: true),
                    TenHang = table.Column<string>(nullable: true),
                    SoLuong = table.Column<decimal>(nullable: true),
                    DonViTinh = table.Column<string>(nullable: true),
                    ThanhTien = table.Column<decimal>(nullable: true),
                    ThueGTGT = table.Column<string>(nullable: true),
                    TienThueGTGT = table.Column<decimal>(nullable: true),
                    TongTienThanhToan = table.Column<decimal>(nullable: true),
                    TrangThaiHoaDon = table.Column<int>(nullable: true),
                    TenTrangThaiHoaDon = table.Column<string>(nullable: true),
                    MauSoHoaDonLienQuan = table.Column<string>(nullable: true),
                    KyHieuHoaDonLienQuan = table.Column<string>(nullable: true),
                    SoHoaDonLienQuan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangTongHopDuLieuHoaDonChiTiets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BangTongHopDuLieuHoaDons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PhienBan = table.Column<string>(maxLength: 6, nullable: false),
                    MauSo = table.Column<string>(maxLength: 15, nullable: false),
                    Ten = table.Column<string>(maxLength: 100, nullable: false),
                    SoBTHDLieu = table.Column<int>(nullable: false),
                    LoaiKyDuLieu = table.Column<string>(maxLength: 1, nullable: false),
                    KyDuLieu = table.Column<string>(maxLength: 10, nullable: false),
                    LanDau = table.Column<bool>(nullable: false),
                    BoSungLanThu = table.Column<int>(nullable: true),
                    NgayLap = table.Column<DateTime>(nullable: false),
                    TenNNT = table.Column<string>(maxLength: 400, nullable: false),
                    MaSoThue = table.Column<string>(maxLength: 14, nullable: false),
                    HDDIn = table.Column<int>(nullable: false),
                    LHHoa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangTongHopDuLieuHoaDons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "BangTongHopDuLieuHoaDons");
        }
    }
}
