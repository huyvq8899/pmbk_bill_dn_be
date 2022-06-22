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
        [Description("NVBANHANG")]
        NhanVienBanHangId,
        [Description("NGAYHOADON")]
        NgayHoaDon,
        [Description("NM 1")] // người mua hàng
        HoTenNguoiMuaHang,
        [Description("NM 2")] // mã khách hàng
        MaKhachHang,
        [Description("NM 3")] // tên khách hàng
        TenKhachHang,
        [Description("NM 4")] // địa chỉ
        DiaChi,
        [Description("NM 5")] // mã số thuế
        MaSoThue,
        [Description("NM 6")] // hình thức thanh toán
        HinhThucThanhToanId,
        [Description("NM 7")] // email
        EmailNguoiMuaHang,
        [Description("NM 8")] // số điện thoại
        SoDienThoaiNguoiMuaHang,
        [Description("NM 9")] // tài khoản ngân hàng
        SoTaiKhoanNganHang,
        [Description("NM 10")] // tên ngân hàng
        TenNganHang,
        /////////////////////////////////////
        [Description("LOAITIEN")]
        LoaiTienId,
        [Description("TYGIA")]
        TyGia,
        /////////////////////////////////////
        [Description("HHDV 2")] // mã hàng
        MaHang,
        [Description("HHDV 3")] // tên hàng
        TenHang,
        [Description("HHDV 4")] // tính chất
        TinhChat,
        [Description("HHDV 5")] // mã quy cách
        MaQuyCach,
        [Description("HHDV 6")] // ĐVT
        DonViTinhId,
        [Description("HHDV 7")] // số lượng
        SoLuong,
        [Description("HHDV 9")] // đơn giá
        DonGia,
        [Description("HHDV 11")] // thành tiền
        ThanhTien,
        [Description("HHDV 12")] // thành tiền quy đổi
        ThanhTienQuyDoi,
        [Description("HHDV 13")] // tỷ lệ ck
        TyLeChietKhau,
        [Description("HHDV 14")] // tiền chiết khấu
        TienChietKhau,
        [Description("HHDV 15")] // tiền chiết khấu quy đổi
        TienChietKhauQuyDoi,
        [Description("HHDV 16")] // % thuế gtgt
        ThueGTGT,
        [Description("HHDV 17")] // tiền thuế gtgt
        TienThueGTGT,
        [Description("HHDV 18")] // tiền thuế gtgt quy đổi
        TienThueGTGTQuyDoi,
        [Description("HHDV 19")] // số lô
        SoLo,
        [Description("HHDV 20")] // hạn sử dụng
        HanSuDung,
        [Description("HHDV 21")] // số khung
        SoKhung,
        [Description("HHDV 22")] // số máy
        SoMay,
        [Description("HHDV 23")] // xuất bản phí
        XuatBanPhi,
        [Description("HHDV 24")] // ghi chú
        GhiChu,
        [Description("HHDV 25")] // mã nhân viên
        MaNhanVien,
        [Description("HHDV 27")] // trường thông tin bổ sung 1
        TruongMoRongChiTiet1,
        [Description("HHDV 28")] // trường thông tin bổ sung 2
        TruongMoRongChiTiet2,
        [Description("HHDV 29")] // trường thông tin bổ sung 3
        TruongMoRongChiTiet3,
        [Description("HHDV 30")] // trường thông tin bổ sung 4
        TruongMoRongChiTiet4,
        [Description("HHDV 31")] // trường thông tin bổ sung 5
        TruongMoRongChiTiet5,
        [Description("HHDV 32")] // trường thông tin bổ sung 6
        TruongMoRongChiTiet6,
        [Description("HHDV 33")] // trường thông tin bổ sung 7
        TruongMoRongChiTiet7,
        [Description("HHDV 34")] // trường thông tin bổ sung 8
        TruongMoRongChiTiet8,
        [Description("HHDV 35")] // trường thông tin bổ sung 9
        TruongMoRongChiTiet9,
        [Description("HHDV 36")] // trường thông tin bổ sung 10
        TruongMoRongChiTiet10,
        [Description("HHDV 37")] // Mặt hàng giảm
        IsMatHangDuocGiam,
        [Description("HHDV 38")] // tỷ lệ % trên doanh thu
        TyLePhanTramDoanhThu,
        [Description("HHDV 39")] // tiền giảm 20% mức tỷ lệ
        TienGiam,
        [Description("HHDV 40")] // Tiền giảm 20% mức tỷ lệ quy đổi
        TienGiamQuyDoi
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
                case MaTruongDLHDExcel.NhanVienBanHangId:
                case MaTruongDLHDExcel.NgayHoaDon:
                case MaTruongDLHDExcel.MaHang: // mã khách hàng
                    DoRong = 100;
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HoTenNguoiMuaHang: // người mua hàng
                case MaTruongDLHDExcel.TenKhachHang: // tên khách hàng
                case MaTruongDLHDExcel.DiaChi: // địa chỉ
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.MaSoThue: // mã số thuế
                    DoRong = 120;
                    break;
                case MaTruongDLHDExcel.HinhThucThanhToanId: // hình thức thanh toán
                    DoRong = 130;
                    break;
                case MaTruongDLHDExcel.EmailNguoiMuaHang: // email
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.SoDienThoaiNguoiMuaHang: // số điện thoại
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.SoTaiKhoanNganHang: // tài khoản ngân hàng
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.TenNganHang: // tên ngân hàng
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.LoaiTienId:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TyGia:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TenHang: // tên hàng
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TinhChat: // tính chất
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.MaQuyCach: // mã quy cách
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.DonViTinhId: // đvt
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.SoLuong: // số lượng
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.DonGia: // đơn giá
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.ThanhTien: // thành tiền
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.ThanhTienQuyDoi: // thành tiền quy đổi
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.TyLeChietKhau: // tỷ lệ chiết khấu
                    DoRong = 130;
                    break;
                case MaTruongDLHDExcel.TienChietKhau: // tiền chiết khấu
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.TienChietKhauQuyDoi: // tiền chiết khấu quy đổi
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.ThueGTGT: // thuế gtgt
                    DoRong = 130;
                    break;
                case MaTruongDLHDExcel.TienThueGTGT: // tiền thuế
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.TienThueGTGTQuyDoi: // tiền thuế gtgt
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.SoLo:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.HanSuDung:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.SoKhung:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.SoMay:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.XuatBanPhi:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.GhiChu:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.MaNhanVien:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet1:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet2:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet3:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet4:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet5:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet6:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet7:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet8:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet9:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.TruongMoRongChiTiet10:
                    DoRong = 100;
                    break;
                case MaTruongDLHDExcel.IsMatHangDuocGiam: // mặt hàng giảm
                    DoRong = 120;
                    break;
                case MaTruongDLHDExcel.TyLePhanTramDoanhThu: // tỷ lệ % doanh thu
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.TienGiam: // tiền giảm
                    DoRong = 150;
                    break;
                case MaTruongDLHDExcel.TienGiamQuyDoi: // tiền giảm quy đổi
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
