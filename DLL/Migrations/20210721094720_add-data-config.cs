using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class Adddataconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HienThiKhiCongGop",
                table: "TruongDuLieus",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.InsertData(
                table: "NghiepVus",
                columns: new string[] { "NghiepVuId", "TenNghiepVu" },
                values: new object[,]
                {
                    {
                        "nv1",
                        "BangKeHDDT"
                    },
                    {
                        "nv2",
                        "BangKeChiTietHoaDon"
                    },
                });

            migrationBuilder.InsertData(
                table: "TruongDuLieus",
                columns: new string[] { "Id", "STT", "MaTruong", "TenTruong", "TenHienThi", "Status", "Default", "NghiepVuId" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        1,
                        "stt",
                        "STT",
                        "STT",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        2,
                        "ngayHoaDon",
                        "Ngày hóa đơn",
                        "Ngày hóa đơn",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        3,
                        "soHoaDon",
                        "Số hóa đơn",
                        "Số hóa đơn",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        4,
                        "mauSoHoaDon",
                        "Ký hiệu mẫu số hóa đơn",
                        "Ký hiệu mẫu số hóa đơn",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        5,
                        "kyHieuHoaDon",
                        "Ký hiệu hóa đơn",
                        "Ký hiệu hóa đơn",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        6,
                        "maKhachHang",
                        "Mã khách hàng",
                        "Mã khách hàng",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        7,
                        "tenKhachHang",
                        "Tên khách hàng",
                        "Tên khách hàng",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        8,
                        "diaChi",
                        "Địa chỉ",
                        "Địa chỉ",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        9,
                        "maSoThue",
                        "Mã số thuế",
                        "Mã số thuế",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        10,
                        "nguoiMuaHang",
                        "Người mua hàng",
                        "Người mua hàng",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        11,
                        "hinhThucThanhToan",
                        "Hình thức thanh toán",
                        "Hình thức thanh toán",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        12,
                        "loaiTien",
                        "Loại tiền",
                        "Loại tiền",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        13,
                        "tyGia",
                        "Tỷ giá",
                        "Tỷ giá",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        14,
                        "maHang",
                        "Mã hàng",
                        "Mã hàng",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        15,
                        "tenHang",
                        "Tên hàng",
                        "Tên hàng hóa, dịch vụ",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        16,
                        "donViTinh",
                        "ĐVT",
                        "ĐVT",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        17,
                        "soLuong",
                        "Số lượng",
                        "Số lượng",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        18,
                        "donGiaSauThue",
                        "Đơn giá sau thuế",
                        "Đơn giá sau thuế",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        19,
                        "donGia",
                        "Đơn giá",
                        "Đơn giá",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        20,
                        "thanhTienSauThue",
                        "Thành tiền sau thuế",
                        "Thành tiền sau thuế",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        21,
                        "thanhTien",
                        "Thành tiền",
                        "Thành tiền",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        22,
                        "thanhTienQuyDoi",
                        "Thành tiền quy đổi",
                        "Thành tiền quy đổi",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        23,
                        "tyLeChietKhau",
                        "Tỷ lệ chiết khấu",
                        "Tỷ lệ chiết khấu",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        24,
                        "tienChietKhau",
                        "Tiền chiết khấu",
                        "Tiền chiết khấu",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        25,
                        "tienChietKhauQuyDoi",
                        "Tiền chiết khấu quy đổi",
                        "Tiền chiết khấu quy đổi",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        26,
                        "doanhSoBanChuaThue",
                        "Doanh số bán chưa thuế",
                        "Doanh số bán chưa thuế",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        27,
                        "doanhSoBanChuaThueQuyDoi",
                        "Doanh số bán chưa thuế quy đổi",
                        "Doanh số bán chưa thuế quy đổi",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        28,
                        "thueGTGT",
                        "% Thuế GTGT",
                        "% Thuế GTGT",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        29,
                        "tienThueGTGT",
                        "Tiền thuế GTGT",
                        "Tiền thuế GTGT",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        30,
                        "tienThueGTGTQuyDoi",
                        "Tiền thuế GTGT quy đổi",
                        "Tiền thuế GTGT quy đổi",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        31,
                        "tongTienThanhToan",
                        "Tổng tiền thanh toán",
                        "Tổng tiền thanh toán",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        32,
                        "tongTienThanhToanQuyDoi",
                        "Tổng tiền thanh toán quy đổi",
                        "Tổng tiền thanh toán quy đổi",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        33,
                        "hangKhuyenMai",
                        "Hàng khuyến mại",
                        "Hàng khuyến mại",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        34,
                        "maQuyCach",
                        "Mã quy cách",
                        "Mã quy cách",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        35,
                        "soLo",
                        "Số lô",
                        "Số lô",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        36,
                        "hanSuDung",
                        "Hạn sử dụng",
                        "Hạn sử dụng",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        37,
                        "soKhung",
                        "Số khung",
                        "Số khung",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        38,
                        "soMay",
                        "Số máy",
                        "Số máy",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        39,
                        "xuatBanPhi",
                        "Xuất bản phí",
                        "Xuất bản phí",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        40,
                        "ghiChu",
                        "Ghi chú",
                        "Ghi chú",
                        false,
                        false,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        41,
                        "maNhanVien",
                        "Mã nhân viên",
                        "Mã nhân viên",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        42,
                        "tenNhanVien",
                        "Tên nhân viên",
                        "Tên nhân viên",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        43,
                        "loaiHoaDon",
                        "Loại hóa đơn",
                        "Loại hóa đơn",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        44,
                        "trangThaiHoaDon",
                        "Trạng thái hóa đơn",
                        "Trạng thái hóa đơn",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        45,
                        "trangThaiPhatHanh",
                        "Trạng thái phát hành",
                        "Trạng thái phát hành",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        46,
                        "maTraCuu",
                        "Mã tra cứu",
                        "Mã tra cứu",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        47,
                        "lyDoXoaBo",
                        "Lý do xóa bỏ",
                        "Lý do xóa bỏ",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        48,
                        "ngayLap",
                        "Ngày lập",
                        "Ngày lập",
                        true,
                        true,
                       "nv2"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        49,
                        "nguoiLap",
                        "Người lập",
                        "Người lập",
                        true,
                        true,
                       "nv2"
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HienThiKhiCongGop",
                table: "TruongDuLieus",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.DeleteData(
                table: "TruongDuLieus",
                keyColumn: "NghiepVuId",
                keyValue: "nv2"
                );
        }
    }
}
