using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class adddatafunctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Functions",
                columns: new string[] { "FunctionId", "FunctionName", "Title", "SubTitle", "CreatedDate", "Status", "Type", "STT" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "HeThong",
                        null,
                        "Hệ thống",
                        DateTime.Now,
                        true,
                        "Hệ thống",
                        1
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "NhapDuLieuTuExcel",
                        "Hệ thống",
                        "Nhập dữ liệu từ Excel",
                        DateTime.Now,
                        true,
                        "Hệ thống",
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "DanhMuc",
                        null,
                        "Danh mục",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        3
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "KhachHang",
                        "Danh mục",
                        "Khách hàng",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        4
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "NhanVien",
                        "Danh mục",
                        "Nhân viên",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        5
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HangHoaDichVu",
                        "Danh mục",
                        "Hàng hóa, dịch vụ",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        6
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "SubHangHoaDichVu",
                        "Hàng hóa, dịch vụ",
                        "Hàng hóa, dịch vụ",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        7
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "DonViTinh",
                        "Hàng hóa, dịch vụ",
                        "Đơn vị tính",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        8
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "LoaiTien",
                        "Danh mục",
                        "Loại tiền",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        9
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "QuanLyHoaDonDienTu",
                        "Danh mục",
                        "Quản lý hóa đơn điện tử",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        10
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HoSoHoaDonDienTu",
                        "Quản lý hóa đơn điện tử",
                        "Hồ sơ hóa đơn điện tử",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        11
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "MauHoaDon",
                        "Quản lý hóa đơn điện tử",
                        "Mẫu hóa đơn",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        12
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "ThongBaoPhatHanhHoaDon",
                        "Quản lý hóa đơn điện tử",
                        "Thông báo phát hành hóa đơn",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        12
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "ThongBaoKetQuaHuyHoaDon",
                        "Quản lý hóa đơn điện tử",
                        "Thông báo kết quả hủy hóa đơn",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        13
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "ThongBaoDieuChinhThongTin",
                        "Quản lý hóa đơn điện tử",
                        "Thông báo điều chỉnh thông tin",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        14
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HinhThucThanhToan",
                        "Danh mục",
                        "Hình thức thanh toán",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        15
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HoaDon",
                        null,
                        "Hóa đơn",
                        DateTime.Now,
                        true,
                        "Hóa đơn",
                        16
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BaoCao",
                        null,
                        "Báo cáo",
                        DateTime.Now,
                        true,
                        "Báo cáo",
                        17
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TienIch",
                        null,
                        "Tiện ích",
                        DateTime.Now,
                        true,
                        "Tiện ích",
                        18
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "NhatKyGuiEmail",
                        "Tiện ích",
                        "Nhật ký gửi email",
                        DateTime.Now,
                        true,
                        "Tiện ích",
                        19
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "NhatKyTruyCap",
                        "Tiện ích",
                        "Nhật ký truy cập",
                        DateTime.Now,
                        true,
                        "Tiện ích",
                        20
                    },
                });

            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "DM_FULL",
                        "Toàn quyền",
                        1
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "DM_VIEW",
                        "Xem",
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "DM_CREATE",
                        "Thêm",
                        3
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "DM_UPDATE",
                        "Sửa",
                        4
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "DM_DELETE",
                        "Xóa",
                        5
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BC_FULL",
                        "Toàn quyền",
                        1
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BC_VIEW",
                        "Xem",
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BC_CREATE",
                        "Thêm",
                        3
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BC_UPDATE",
                        "Sửa",
                        4
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BC_DELETE",
                        "Xóa",
                        5
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TOOL_VIEW",
                        "Xem",
                        5
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
