﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class adddatatotabletruongdulieuhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TruongDuLieuHoaDons",
                columns: new string[] { 
                    "Id",
                    "STT",
                    "MaTruong",
                    "TenTruong", 
                    "TenTruongData",
                    "GhiChu", 
                    "IsChiTiet",
                    "Status",
                    "Default",
                    "Size", 
                    "Align", 
                    "DefaultSTT", 
                    "DinhDangSo" },
                values: new object[,] {
                    {
                        Guid.NewGuid().ToString(),
                        1,
                        "NHD",
                        "Ngày hóa đơn",
                        "NgayHoaDon",
                        "",
                        false,
                        true,
                        true,
                        150,
                        "center",
                        1,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        2,
                        "SHD",
                        "Số hóa đơn",
                        "SoHoaDon",
                        "",
                        false,
                        true,
                        true,
                        130,
                        "left",
                        2,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        3,
                        "MSHD",
                        "Ký hiệu mẫu số hóa đơn",
                        "MauSo",
                        "",
                        false,
                        true,
                        true,
                        250,
                        "left",
                        3,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        4,
                        "KHHD",
                        "Ký hiệu hóa đơn",
                        "KyHieu",
                        "",
                        false,
                        true,
                        true,
                        180,
                        "left",
                        4,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        5,
                        "MKH",
                        "Mã khách hàng",
                        "MaKhachHang",
                        "",
                        false,
                        true,
                        false,
                        150,
                        "left",
                        5,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        6,
                        "TKH",
                        "Tên khách hàng",
                        "TenKhachHang",
                        "",
                        false,
                        true,
                        true,
                        200,
                        "left",
                        6,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        7,
                        "DC",
                        "Địa chỉ",
                        "DiaChi",
                        "",
                        false,
                        true,
                        true,
                        200,
                        "left",
                        7,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        8,
                        "MST",
                        "Mã số thuế",
                        "MaSoThue",
                        "",
                        false,
                        true,
                        true,
                        150,
                        "left",
                        8,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        9,
                        "NMH",
                        "Người mua hàng",
                        "HoTenNguoiMuaHang",
                        "",
                        false,
                        true,
                        false,
                        200,
                        "left",
                        9,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        10,
                        "NVBH",
                        "NV bán hàng",
                        "NhanVienBanHang",
                        "",
                        false,
                        true,
                        false,
                        200,
                        "left",
                        10,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        11,
                        "TT",
                        "Tổng tiền thanh toán",
                        "TongTienThanhToan",
                        "",
                        false,
                        true,
                        true,
                        200,
                        "right",
                        11,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        12,
                        "LHD",
                        "Loại hóa đơn",
                        "LoaiHoaDon",
                        "",
                        false,
                        true,
                        true,
                        150,
                        "left",
                        12,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        13,
                        "TTHD",
                        "Trạng thái hóa đơn",
                        "TrangThai",
                        "",
                        false,
                        true,
                        true,
                        150,
                        "left",
                        13,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        14,
                        "TTPH",
                        "Trạng thái phát hành",
                        "TrangThaiPhatHanh",
                        "",
                        false,
                        true,
                        true,
                        150,
                        "left",
                        14,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        15,
                        "MTC",
                        "Mã tra cứu",
                        "MaTraCuu",
                        "",
                        false,
                        true,
                        false,
                        150,
                        "left",
                        15,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        16,
                        "TTGHD",
                        "Trạng thái gửi hóa đơn",
                        "TrangThaiGuiHoaDon",
                        "",
                        false,
                        true,
                        true,
                        180,
                        "left",
                        16,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        17,
                        "TNN",
                        "Tên người nhận",
                        "HoTenNguoiNhanHoaDon",
                        "",
                        false,
                        true,
                        false,
                        200,
                        "left",
                        17,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        18,
                        "ENN",
                        "Email người nhận",
                        "EmailNguoiNhanHoaDon",
                        "",
                        false,
                        true,
                        false,
                        180,
                        "left",
                        18,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        19,
                        "SDTNN",
                        "Số điện thoại người nhận",
                        "SoDienThoaiNguoiNhan",
                        "",
                        false,
                        true,
                        true,
                        210,
                        "left",
                        19,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        20,
                        "KHDN",
                        "Khách hàng đã nhận hóa đơn",
                        "DaNhanHoaDon",
                        "",
                        false,
                        true,
                        true,
                        250,
                        "center",
                        20,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        21,
                        "CHDG",
                        "Số lần chuyển thành hóa đơn giấy",
                        "SoLanChuyenThanhHoaDonGiay",
                        "",
                        false,
                        true,
                        true,
                        260,
                        "right",
                        21,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        22,
                        "LCT",
                        "Loại chứng từ",
                        "LoaiChungTu",
                        "",
                        false,
                        true,
                        true,
                        130,
                        "left",
                        22,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        23,
                        "LDXB",
                        "Lý do xóa bỏ",
                        "LyDoXoaBo",
                        "",
                        false,
                        true,
                        true,
                        200,
                        "left",
                        23,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        24,
                        "TLDK",
                        "Tài liệu đính kèm",
                        "TaiLieuDinhKem",
                        "",
                        false,
                        true,
                        true,
                        150,
                        "left",
                        24,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        25,
                        "NL",
                        "Người lập",
                        "NguoiLap",
                        "",
                        false,
                        true,
                        true,
                        200,
                        "left",
                        25,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        26,
                        "NL",
                        "Ngày lập",
                        "NgayLap",
                        "",
                        false,
                        true,
                        true,
                        200,
                        "left",
                        25,
                        false
                    },
                });

            migrationBuilder.InsertData(
                table: "TruongDuLieuHoaDons",
                columns: new string[] {
                       "Id"
                    , "STT"
                    , "MaTruong"
                    , "TenTruong"
                    , "TenTruongData"
                    , "GhiChu"
                    , "IsChiTiet"
                    , "Status"
                    , "Default"
                    , "Size"
                    , "Align"
                    , "DefaultSTT"
                    , "DinhDangSo" },
                values: new object[,] {
                    {
                        Guid.NewGuid().ToString(),
                        1,
                        "HHDV 1",
                        "STT",
                        "STT",
                        "",
                        true,
                        true,
                        true,
                        50,
                        "center",
                        1,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        2,
                        "HHDV 2",
                        "Mã hàng",
                        "MaHang",
                        "",
                        true,
                        true,
                        true,
                        120,
                        "left",
                        2,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        3,
                        "HHDV 3",
                        "Tên hàng",
                        "TenHang",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "left",
                        3,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        4,
                        "HHDV 4",
                        "Hàng khuyến mại",
                        "HangKhuyenMai",
                        "",
                        true,
                        true,
                        true,
                        130,
                        "center",
                        4,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        5,
                        "HHDV 5",
                        "Mã quy cách",
                        "MaQuyCach",
                        "",
                        true,
                        false,
                        false,
                        130,
                        "left",
                        5,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        6,
                        "HHDV 6",
                        "ĐVT",
                        "DonViTinh",
                        "",
                        true,
                        true,
                        true,
                        100,
                        "left",
                        6,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        7,
                        "HHDV 7",
                        "Số lượng",
                        "SoLuong",
                        "",
                        true,
                        true,
                        true,
                        200,
                        "left",
                        7,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        8,
                        "HHDV 8",
                        "Đơn giá sau thuế",
                        "DonGiaSauThue",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        8,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        9,
                        "HHDV 9",
                        "Đơn giá",
                        "DonGia",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "left",
                        9,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        10,
                        "HHDV 10",
                        "Thành tiền sau thuế",
                        "ThanhTienSauThue",
                        "",
                        true,
                        false,
                        false,
                        200,
                        "left",
                        10,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        11,
                        "HHDV 11",
                        "Thành tiền",
                        "ThanhTien",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "right",
                        11,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        12,
                        "HHDV 12",
                        "Thành tiền quy đổi",
                        "ThanhTienQuyDoi",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        12,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        13,
                        "HHDV 13",
                        "Tỷ lệ chiết khấu",
                        "TyLeChietKhau",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "right",
                        13,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        14,
                        "HHDV 14",
                        "Tiền chiết khấu",
                        "TienChietKhau",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "right",
                        14,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        15,
                        "HHDV 15",
                        "Tiền chiết khấu quy đổi",
                        "TienChietKhauQuyDoi",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "right",
                        15,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        16,
                        "HHDV 16",
                        "% Thuế GTGT",
                        "ThueGTGT",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "right",
                        16,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        17,
                        "HHDV 17",
                        "Tiền thuế GTGT",
                        "TienThueGTGT",
                        "",
                        true,
                        true,
                        true,
                        150,
                        "left",
                        17,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        18,
                        "HHDV 18",
                        "Tiền thuế GTGT quy đổi",
                        "TienThueGTGTQuyDoi",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "right",
                        18,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        19,
                        "HHDV 19",
                        "Số lô",
                        "SoLo",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        19,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        20,
                        "HHDV 20",
                        "Hạn sử dụng",
                        "HanSuDung",
                        "",
                        true,
                        false,
                        false,
                        180,
                        "center",
                        20,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        21,
                        "HHDV 21",
                        "Số khung",
                        "SoKhung",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        21,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        22,
                        "HHDV 22",
                        "Số máy",
                        "SoMay",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        22,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        23,
                        "HHDV 23",
                        "Xuất bản phí",
                        "XuatBanPhi",
                        "",
                        true,
                        false,
                        false,
                        200,
                        "right",
                        23,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        24,
                        "HHDV 24",
                        "Ghi chú",
                        "GhiChu",
                        "",
                        true,
                        false,
                        false,
                        200,
                        "left",
                        24,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        25,
                        "HHDV 25",
                        "Mã nhân viên",
                        "MaNhanVien",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        25,
                        false
                    },
                    {
                        Guid.NewGuid().ToString(),
                        26,
                        "HHDV 26",
                        "Tên nhân viên",
                        "TenNhanVien",
                        "",
                        true,
                        false,
                        false,
                        150,
                        "left",
                        26,
                        false
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from TruongDuLieuHoaDons");
        }
    }
}
