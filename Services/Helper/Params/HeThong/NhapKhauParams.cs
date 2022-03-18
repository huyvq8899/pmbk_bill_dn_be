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
        [Description("Hình thức chiết khấu")]
        HTCK,
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
        [Description("Số lô")]
        HHDV19,
        [Description("Hạn sử dụng")]
        HHDV20,
        [Description("Số khung")]
        HHDV21,
        [Description("Số máy")]
        HHDV22,
        [Description("Xuất bản phí")]
        HHDV23,
        [Description("Ghi chú")]
        HHDV24,
        [Description("Mã nhân viên")]
        HHDV25,
        [Description("Trường thông tin bổ sung 1")]
        HHDV27,
        [Description("Trường thông tin bổ sung 2")]
        HHDV28,
        [Description("Trường thông tin bổ sung 3")]
        HHDV29,
        [Description("Trường thông tin bổ sung 4")]
        HHDV30,
        [Description("Trường thông tin bổ sung 5")]
        HHDV31,
        [Description("Trường thông tin bổ sung 6")]
        HHDV32,
        [Description("Trường thông tin bổ sung 7")]
        HHDV33,
        [Description("Trường thông tin bổ sung 8")]
        HHDV34,
        [Description("Trường thông tin bổ sung 9")]
        HHDV35,
        [Description("Trường thông tin bổ sung 10")]
        HHDV36,
        [Description("Trường Tỷ lệ % trên doanh thu")]
        HHDV37,
        [Description("Trường Tiền giảm 20% mức tỷ lệ")]
        HHDV38,
        [Description("Trường Tiền giảm 20% mức tỷ lệ quy đổi")]
        HHDV39
    }

    public class TruongDLHDExcel
    {
        public MaTruongDLHDExcel Ma { get; set; }
        public int ColIndex { get; set; }
        public string TenTruong { get; set; }
        public int NhomThongTin { get; set; } // 1: thông tin chung, 2: thong tin chi tiết
        public string TenTruongExcel { get; set; }
        public string TenEnum { get; set; }

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
