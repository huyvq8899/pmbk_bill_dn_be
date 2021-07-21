using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class adddataconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HienThiKhiCongGop",
                table: "TruongDuLieus",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            string id1 = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();

            migrationBuilder.InsertData(
                table: "NghiepVus",
                columns: new string[] { "NghiepVuId", "TenNghiepVu" },
                values: new object[,]
                {
                    {
                        id1,
                        "BangKeHDDT"
                    },
                    {
                        id2,
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
                        "fSTT",
                        "STT",
                        "STT",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        2,
                        "fNgayHoaDon",
                        "Ngày hóa đơn",
                        "Ngày hóa đơn",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        3,
                        "fSoHoaDon",
                        "Số hóa đơn",
                        "Số hóa đơn",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        4,
                        "fMauSoHoaDon",
                        "Ký hiệu mẫu số hóa đơn",
                        "Ký hiệu mẫu số hóa đơn",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        5,
                        "fKyHieuHoaDon",
                        "Ký hiệu hóa đơn",
                        "Ký hiệu hóa đơn",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        6,
                        "fMaKhachHang",
                        "Mã khách hàng",
                        "Mã khách hàng",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        7,
                        "fTenKhachHang",
                        "Tên khách hàng",
                        "Tên khách hàng",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        8,
                        "fDiaChi",
                        "Địa chỉ",
                        "Địa chỉ",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        9,
                        "fMaSoThue",
                        "Mã số thuế",
                        "Mã số thuế",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        10,
                        "fNguoiMuaHang",
                        "Người mua hàng",
                        "Người mua hàng",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        11,
                        "fHinhThucThanhToan",
                        "Hình thức thanh toán",
                        "Hình thức thanh toán",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        12,
                        "fLoaiTien",
                        "Loại tiền",
                        "Loại tiền",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        13,
                        "fTyGia",
                        "Tỷ giá",
                        "Tỷ giá",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        14,
                        "fMaHang",
                        "Mã hàng",
                        "Mã hàng",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        15,
                        "fTenHang",
                        "Tên hàng",
                        "Tên hàng hóa, dịch vụ",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        16,
                        "fDVT",
                        "ĐVT",
                        "ĐVT",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        17,
                        "fSoLuong",
                        "Số lượng",
                        "Số lượng",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        18,
                        "fDonGiaSauThue",
                        "Đơn giá sau thuế",
                        "Đơn giá sau thuế",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        19,
                        "fDonGia",
                        "Đơn giá",
                        "Đơn giá",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        20,
                        "fThanhTienSauThue",
                        "Thành tiền sau thuế",
                        "Thành tiền sau thuế",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        21,
                        "fThanhTien",
                        "Thành tiền",
                        "Thành tiền",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        22,
                        "fThanhTienQuyDoi",
                        "Thành tiền quy đổi",
                        "Thành tiền quy đổi",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        23,
                        "fTyLeChietKhau",
                        "Tỷ lệ chiết khấu",
                        "Tỷ lệ chiết khấu",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        24,
                        "fTienChietKhau",
                        "Tiền chiết khấu",
                        "Tiền chiết khấu",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        25,
                        "fTienChietKhauQuyDoi",
                        "Tiền chiết khấu quy đổi",
                        "Tiền chiết khấu quy đổi",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        26,
                        "fDoanhSoBanChuaThue",
                        "Doanh số bán chưa thuế",
                        "Doanh số bán chưa thuế",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        27,
                        "fDoanhSoBanChuaThueQuyDoi",
                        "Doanh số bán chưa thuế quy đổi",
                        "Doanh số bán chưa thuế quy đổi",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        28,
                        "fThueGTGT",
                        "% Thuế GTGT",
                        "% Thuế GTGT",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        29,
                        "fTienThueGTGT",
                        "Tiền thuế GTGT",
                        "Tiền thuế GTGT",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        30,
                        "fTienThueGTGTQuyDoi",
                        "Tiền thuế GTGT quy đổi",
                        "Tiền thuế GTGT quy đổi",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        31,
                        "fTongTienThanhToan",
                        "Tổng tiền thanh toán",
                        "Tổng tiền thanh toán",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        32,
                        "fTongTienThanhToanQuyDoi",
                        "Tổng tiền thanh toán quy đổi",
                        "Tổng tiền thanh toán quy đổi",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        33,
                        "fHangKhuyenMai",
                        "Hàng khuyến mại",
                        "Hàng khuyến mại",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        34,
                        "fMaQuyCach",
                        "Mã quy cách",
                        "Mã quy cách",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        35,
                        "fSoLo",
                        "Số lô",
                        "Số lô",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        36,
                        "fHanSuDung",
                        "Hạn sử dụng",
                        "Hạn sử dụng",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        37,
                        "fSoKhung",
                        "Số khung",
                        "Số khung",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        38,
                        "fSoMay",
                        "Số máy",
                        "Số máy",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        39,
                        "fXuatBanPhi",
                        "Xuất bản phí",
                        "Xuất bản phí",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        40,
                        "fGhiChu",
                        "Ghi chú",
                        "Ghi chú",
                        false,
                        false,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        41,
                        "fMaNhanVien",
                        "Mã nhân viên",
                        "Mã nhân viên",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        42,
                        "fTenNhanVien",
                        "Tên nhân viên",
                        "Tên nhân viên",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        43,
                        "fLoaiHoaDon",
                        "Loại hóa đơn",
                        "Loại hóa đơn",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        44,
                        "fTrangThaiHoaDon",
                        "Trạng thái hóa đơn",
                        "Trạng thái hóa đơn",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        45,
                        "fTrangThaiPhatHanh",
                        "Trạng thái phát hành",
                        "Trạng thái phát hành",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        46,
                        "fMaTraCuu",
                        "Mã tra cứu",
                        "Mã tra cứu",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        47,
                        "fLyDoXoaBo",
                        "Lý do xóa bỏ",
                        "Lý do xóa bỏ",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        48,
                        "fNgayLap",
                        "Ngày lập",
                        "Ngày lập",
                        true,
                        true,
                        id2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        49,
                        "fNguoiLap",
                        "Người lập",
                        "Người lập",
                        true,
                        true,
                        id2
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
        }
    }
}
