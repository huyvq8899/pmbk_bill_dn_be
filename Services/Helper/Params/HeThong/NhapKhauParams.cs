using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;

namespace Services.Helper.Params.HeThong
{
    public class NhapKhauParams
    {
        public IList<IFormFile> Files { get; set; }
        public int ModeValue { get; set; } //chế độ nhập khẩu: 1: nhập khẩu thêm mới; 2: nhâp khẩu cập nhật
        public int? FileType { get; set; } // Loại file import: 1 or NULL: excel, 2: xml
    }

    public enum ImpExcelHDGTGT
    {
        None,
        [Description("Ký hiệu mẫu số hóa đơn")]
        MauSo,
        [Description("Ký hiệu hóa đơn")]
        KyHieu,
        [Description("Ngày hóa đơn")]
        NgayHoaDon,
        [Description("Số hóa đơn")]
        SoHoaDon,
        [Description("Mã số thuế")]
        MaSoThue,
        [Description("Mã khách hàng")]
        MaKhachHang,
        [Description("Tên khách hàng")]
        TenKhachHang
    }
}
