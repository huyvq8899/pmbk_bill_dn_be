using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Services.Helper.Params.HeThong
{
    public class NhapKhauParams
    {
        public IList<IFormFile> Files { get; set; }
        public int ModeValue { get; set; } //chế độ nhập khẩu: 1: nhập khẩu thêm mới; 2: nhâp khẩu cập nhật
        public int? FileType { get; set; } // Loại file import: 1 or NULL: excel, 2: xml
        public string BoKyHieuHoaDonId { get; set; }
        public int? LoaiHoaDon { get; set; }
    }

    public enum ImpExcelHDGTGT
    {
        None,
        [Description("Số thứ tự hóa đơn")]
        STTHoaDon,
        [Description("Ký hiệu mẫu số hóa đơn")]
        MauSo,
        [Description("Ký hiệu hóa đơn")]
        KyHieu,
        [Description("Ngày hóa đơn")]
        NgayHoaDon,
        [Description("Mã số thuế")]
        MaSoThue,
        [Description("Mã khách hàng")]
        MaKhachHang,
        [Description("Tên khách hàng")]
        TenKhachHang,
        [Description("Địa chỉ")]
        DiaChi,
        [Description("Người mua hàng")]
        NguoiMuaHang,
        [Description("Số điện thoại")]
        SoDienThoai,
        [Description("Email")]
        Email,
        [Description("Số tài khoản ngân hàng")]
        SoTaiKhoanNganHang,
        [Description("Tên ngân hàng")]
        TenNganHang,
        [Description("Hình thức thanh toán")]
        HinhThucThanhToan,
        [Description("NV bán hàng")]
        NhanVienBanHang,
        [Description("Loại tiền")]
        LoaiTien,
        [Description("Tỷ giá")]
        TyGia,
        [Description("Tính chất")]
        TinhChat,
        [Description("Mã hàng")]
        MaHang,
        [Description("Tên hàng")]
        TenHang,
        [Description("ĐVT")]
        DonViTinh,
        [Description("Số lượng")]
        SoLuong,
        [Description("Đơn giá")]
        DonGia,
        [Description("Tỷ lệ CK")]
        TyLeChietKhau,
        [Description("Tiền chiết khấu")]
        TienChietKhau,
        [Description("Thành tiền")]
        ThanhTien,
        [Description("Thuế suất")]
        ThueSuat
    }

    public enum MaTruongDLHDExcel
    {
        [Description("NV bán hàng")]
        NVBANHANG,
        [Description("Ngày hóa đơn")]
        NGAYHOADON,
        [Description("Người mua hàng")]
        NM1,
        [Description("Mã khách hàng")]
        NM2,
        [Description("Tên khách hàng")]
        NM3,
        [Description("Địa chỉ")]
        NM4,
        [Description("Mã số thuế")]
        NM5,
        [Description("Hình thức thanh toán")]
        NM6,
        [Description("Email")]
        NM7,
        [Description("Số điện thoại")]
        NM8,
        [Description("Tài khoản ngân hàng")]
        NM9,
        [Description("Tên ngân hàng")]
        NM10,
        /////////////////////////////////////
        [Description("Loại tiền")]
        LOAITIEN,
        [Description("Tỷ giá")]
        TYGIA,
        /////////////////////////////////////
        [Description("Mã hàng")]
        HHDV2,
        [Description("Tên hàng")]
        HHDV3,
        [Description("Tính chất")]
        HHDV4,
        [Description("Mã quy cách")]
        HHDV5,
        [Description("ĐVT")]
        HHDV6,
        [Description("Số lượng")]
        HHDV7,
        [Description("Đơn giá")]
        HHDV9,
        [Description("Thành tiền")]
        HHDV11,
        [Description("Thành tiền quy đổi")]
        HHDV12,
        [Description("Tỷ lệ chiết khấu")]
        HHDV13,
        [Description("Tiền chiết khấu")]
        HHDV14,
        [Description("Tiền chiết khấu quy đổi")]
        HHDV15,
        [Description("% Thuế GTGT")]
        HHDV16,
        [Description("Tiền thuế GTGT")]
        HHDV17,
        [Description("Tiền thuế GTGT quy đổi")]
        HHDV18,
    }

    public class TruongDLHDExcel
    {
        public MaTruongDLHDExcel Ma { get; set; }
        public int ColIndex { get; set; }
        public string TenTruong { get; set; }
        public int NhomThongTin { get; set; } // 1: thông tin chung, 2: thong tin chi tiết
        public string TenTruongExcel { get; set; }

        public List<EnumModel> GetTruongDLHDExcels()
        {
            List<EnumModel> enums = ((MaTruongDLHDExcel[])Enum.GetValues(typeof(MaTruongDLHDExcel)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription(),
                    NameOfKey = Enum.GetName(typeof(MaTruongDLHDExcel), c)
                }).ToList();
            return enums;
        }
    }
}
