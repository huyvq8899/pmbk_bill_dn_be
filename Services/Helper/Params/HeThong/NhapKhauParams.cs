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

        public List<ImportExcelModel> PXKVanChuyenNoiBos = new List<ImportExcelModel>
        {
            new ImportExcelModel { MaCot="NGAYHOADON", TenCot = "NgayHoaDon", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 1", TenCot = "CanCuSo", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 2", TenCot = "NgayCanCu", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 3", TenCot = "Cua", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 4", TenCot = "DienGiai", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 5", TenCot = "DiaChiKhoXuatHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 6", TenCot = "HoTenNguoiXuatHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 7", TenCot = "HopDongVanChuyenSo", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 8", TenCot = "TenNguoiVanChuyen", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 9", TenCot = "PhuongThucVanChuyen", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 10", TenCot = "MaKhachHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 11", TenCot = "TenKhachHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 12", TenCot = "MaSoThue", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 13", TenCot = "DiaChiKhoNhanHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 14", TenCot = "HoTenNguoiNhanHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="LOAITIEN", TenCot = "LoaiTienId", LoaiCot = 1 },
            new ImportExcelModel { MaCot="TYGIA", TenCot = "TyGia", LoaiCot = 1 },
            new ImportExcelModel { MaCot="HHDV 2", TenCot = "MaHang", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 3", TenCot = "TenHang", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 4", TenCot = "TinhChat", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 5", TenCot = "MaQuyCach", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 6", TenCot = "DonViTinhId", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 7", TenCot = "SoLuong", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 8", TenCot = "SoLuongNhap", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 9", TenCot = "DonGia", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 10", TenCot = "ThanhTien", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 11", TenCot = "ThanhTienQuyDoi", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 12", TenCot = "SoLo", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 13", TenCot = "HanSuDung", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 14", TenCot = "SoKhung", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 15", TenCot = "SoMay", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 16", TenCot = "XuatBanPhi", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 17", TenCot = "GhiChu", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 18", TenCot = "TruongMoRongChiTiet1", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 19", TenCot = "TruongMoRongChiTiet2", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 20", TenCot = "TruongMoRongChiTiet3", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 21", TenCot = "TruongMoRongChiTiet4", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 22", TenCot = "TruongMoRongChiTiet5", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 23", TenCot = "TruongMoRongChiTiet6", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 24", TenCot = "TruongMoRongChiTiet7", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 25", TenCot = "TruongMoRongChiTiet8", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 26", TenCot = "TruongMoRongChiTiet9", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 27", TenCot = "TruongMoRongChiTiet10", LoaiCot = 2 },
        };

        public List<ImportExcelModel> PXKVGuiBanDaiLys = new List<ImportExcelModel>
        {
            new ImportExcelModel { MaCot="NGAYHOADON", TenCot = "NgayHoaDon", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 1", TenCot = "CanCuSo", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 2", TenCot = "NgayCanCu", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 3", TenCot = "Cua", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 4", TenCot = "DienGiai", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 5", TenCot = "MaKhachHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 6", TenCot = "TenKhachHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 7", TenCot = "MaSoThue", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 8", TenCot = "DiaChiKhoNhanHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 9", TenCot = "HoTenNguoiNhanHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 10", TenCot = "DiaChiKhoXuatHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 11", TenCot = "HoTenNguoiXuatHang", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 12", TenCot = "HopDongVanChuyenSo", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 13", TenCot = "TenNguoiVanChuyen", LoaiCot = 1 },
            new ImportExcelModel { MaCot="NM 14", TenCot = "PhuongThucVanChuyen", LoaiCot = 1 },
            new ImportExcelModel { MaCot="LOAITIEN", TenCot = "LoaiTienId", LoaiCot = 1 },
            new ImportExcelModel { MaCot="TYGIA", TenCot = "TyGia", LoaiCot = 1 },
            new ImportExcelModel { MaCot="HHDV 2", TenCot = "MaHang", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 3", TenCot = "TenHang", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 4", TenCot = "TinhChat", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 5", TenCot = "MaQuyCach", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 6", TenCot = "DonViTinhId", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 7", TenCot = "SoLuong", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 8", TenCot = "DonGia", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 9", TenCot = "ThanhTien", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 10", TenCot = "ThanhTienQuyDoi", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 11", TenCot = "SoLo", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 12", TenCot = "HanSuDung", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 13", TenCot = "SoKhung", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 14", TenCot = "SoMay", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 15", TenCot = "XuatBanPhi", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 16", TenCot = "GhiChu", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 17", TenCot = "TruongMoRongChiTiet1", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 18", TenCot = "TruongMoRongChiTiet2", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 19", TenCot = "TruongMoRongChiTiet3", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 20", TenCot = "TruongMoRongChiTiet4", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 21", TenCot = "TruongMoRongChiTiet5", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 22", TenCot = "TruongMoRongChiTiet6", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 23", TenCot = "TruongMoRongChiTiet7", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 24", TenCot = "TruongMoRongChiTiet8", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 25", TenCot = "TruongMoRongChiTiet9", LoaiCot = 2 },
            new ImportExcelModel { MaCot="HHDV 26", TenCot = "TruongMoRongChiTiet10", LoaiCot = 2 },
        };
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
        TruongThongTinBoSung1,
        [Description("HHDV 28")] // trường thông tin bổ sung 2
        TruongThongTinBoSung2,
        [Description("HHDV 29")] // trường thông tin bổ sung 3
        TruongThongTinBoSung3,
        [Description("HHDV 30")] // trường thông tin bổ sung 4
        TruongThongTinBoSung4,
        [Description("HHDV 31")] // trường thông tin bổ sung 5
        TruongThongTinBoSung5,
        [Description("HHDV 32")] // trường thông tin bổ sung 6
        TruongThongTinBoSung6,
        [Description("HHDV 33")] // trường thông tin bổ sung 7
        TruongThongTinBoSung7,
        [Description("HHDV 34")] // trường thông tin bổ sung 8
        TruongThongTinBoSung8,
        [Description("HHDV 35")] // trường thông tin bổ sung 9
        TruongThongTinBoSung9,
        [Description("HHDV 36")] // trường thông tin bổ sung 10
        TruongThongTinBoSung10,
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
                case MaTruongDLHDExcel.MaKhachHang: // mã khách hàng
                case MaTruongDLHDExcel.MaHang: // mã hàng
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
                case MaTruongDLHDExcel.TruongThongTinBoSung1:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung2:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung3:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung4:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung5:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung6:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung7:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung8:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung9:
                    DoRong = 200;
                    break;
                case MaTruongDLHDExcel.TruongThongTinBoSung10:
                    DoRong = 200;
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

        public TruongDLHDExcel(string tenCot)
        {
            switch (tenCot)
            {
                case "MaKhachHang":
                case "LoaiTienId":
                case "TyGia":
                case "NgayCanCu":
                case "MaHang":
                case "TinhChat":
                case "MaQuyCach":
                case "DonViTinhId":

                case "SoLo":
                case "HanSuDung":
                case "SoKhung":
                case "SoMay":
                case "XuatBanPhi":
                case "GhiChu":
                    DoRong = 100;
                    break;
                case "CanCuSo":
                case "Cua":
                case "DienGiai":
                case "HoTenNguoiMuaHang":
                case "HoTenNguoiNhanHang":
                case "HoTenNguoiXuatHang":
                case "TenKhachHang":
                case "DiaChi":
                case "DiaChiKhoNhanHang":
                case "DiaChiKhoXuatHang":
                case "HopDongVanChuyenSo":
                case "TenNguoiVanChuyen":
                case "PhuongThucVanChuyen":
                case "TenHang":
                    DoRong = 200;
                    break;
                case "NgayHoaDon":
                case "MaSoThue":
                    DoRong = 120;
                    break;
                case "SoLuong":
                case "SoLuongNhap":
                    DoRong = 130;
                    break;
                case "DonGia":
                case "ThanhTien":
                case "ThanhTienQuyDoi":
                    DoRong = 150;
                    break;
                case "TruongMoRongChiTiet1":
                case "TruongMoRongChiTiet2":
                case "TruongMoRongChiTiet3":
                case "TruongMoRongChiTiet4":
                case "TruongMoRongChiTiet5":
                case "TruongMoRongChiTiet6":
                case "TruongMoRongChiTiet7":
                case "TruongMoRongChiTiet8":
                case "TruongMoRongChiTiet9":
                case "TruongMoRongChiTiet10":
                    DoRong = 200;
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
        public string MaTruong { get; set; }


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

    public class ImportExcelModel
    {
        public string MaCot { get; set; }
        public string TenCot { get; set; }
        public int LoaiCot { get; set; } // 1: mua hàng, 2: hhdv
    }
}
