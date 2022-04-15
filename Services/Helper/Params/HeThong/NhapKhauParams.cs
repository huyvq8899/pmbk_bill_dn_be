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
        public TruongDLHDExcel()
        {
        }

        public TruongDLHDExcel(MaTruongDLHDExcel ma)
        {
            switch (ma)
            {
                case MaTruongDLHDExcel.NVBANHANG:
                case MaTruongDLHDExcel.NGAYHOADON:
                case MaTruongDLHDExcel.NM2: // mã khách hàng
                    DoRong = 100;
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.NM1: // người mua hàng
                case MaTruongDLHDExcel.NM3: // tên khách hàng
                case MaTruongDLHDExcel.NM4: // địa chỉ
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.NM5: // mã số thuế
                    DoRong = 120;
                    break;
                case MaTruongDLHDExcel.NM6: // hình thức thanh toán
                    DoRong = 130;
                    break;
                case MaTruongDLHDExcel.NM7: // email
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.NM8: // số điện thoại
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.NM9: // tài khoản ngân hàng
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.NM10: // tên ngân hàng
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.LOAITIEN:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TYGIA:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HTCK:
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV2: // mã hàng
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV3: // tên hàng
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.HHDV4: // tính chất
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV5: // mã quy cách
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV6: // đvt
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV7: // số lượng
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV9: // đơn giá
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV11: // thành tiền
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV12: // thành tiền quy đổi
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV13: // tỷ lệ chiết khấu
                    DoRong = 120;
                    break;
                case MaTruongDLHDExcel.HHDV14: // tiền chiết khấu
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV15: // tiền chiết khấu quy đổi
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV16: // thuế gtgt
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV17: // tiền thuế
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV18: // tiền thuế gtgt
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV19:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV20:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV21:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV22:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV23:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV24:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV25:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV27:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV28:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV29:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV30:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV31:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV32:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV33:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV34:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV35:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV36:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HHDV37: // tỷ lệ % doanh thu
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV38: // tiền giảm
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.HHDV39: // tiền giảm quy đổi
                    DoRong = 150;
                    break;
                default:
                    break;
            }
        }

        public MaTruongDLHDExcel Ma { get; set; }
        public int ColIndex { get; set; }
        public string TenTruong { get; set; }
        public int NhomThongTin { get; set; } // 1: thông tin chung, 2: thong tin chi tiết
        public string TenTruongExcel { get; set; }
        public string TenEnum { get; set; }
        public int DoRong { get; set; }


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
