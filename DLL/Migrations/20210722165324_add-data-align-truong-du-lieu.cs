﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Adddataaligntruongdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TruongDuLieus",
                column: "Align",
                values: new object[] {
                    "center",
                    "right",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "right",
                    "left",
                    "left",
                    "left",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "right",
                    "center",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "left",
                    "center",
                    "left",
                },
                keyColumn: "MaTruong",
                keyValues: new object[]
                {
                    "stt",
                    "ngayHoaDon",
                    "soHoaDon",
                    "mauSoHoaDon",
                    "kyHieuHoaDon",
                    "maKhachHang",
                    "tenKhachHang",
                    "diaChi",
                    "maSoThue",
                    "nguoiMuaHang",
                    "hinhThucThanhToan",
                    "loaiTien",
                    "tyGia",
                    "maHang",
                    "tenHang",
                    "donViTinh",
                    "soLuong",
                    "donGiaSauThue",
                    "donGia",
                    "thanhTienSauThue",
                    "thanhTien",
                    "thanhTienQuyDoi",
                    "tyLeChietKhau",
                    "tienChietKhau",
                    "tienChietKhauQuyDoi",
                    "doanhSoBanChuaThue",
                    "doanhSoBanChuaThueQuyDoi",
                    "thueGTGT",
                    "tienThueGTGT",
                    "tienThueGTGTQuyDoi",
                    "tongTienThanhToan",
                    "tongTienThanhToanQuyDoi",
                    "hangKhuyenMai",
                    "maQuyCach",
                    "soLo",
                    "hanSuDung",
                    "soKhung",
                    "soMay",
                    "xuatBanPhi",
                    "ghiChu",
                    "maNhanVien",
                    "tenNhanVien",
                    "loaiHoaDon",
                    "trangThaiHoaDon",
                    "trangThaiPhatHanh",
                    "maTraCuu",
                    "lyDoXoaBo",
                    "ngayLap",
                    "nguoiLap"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
