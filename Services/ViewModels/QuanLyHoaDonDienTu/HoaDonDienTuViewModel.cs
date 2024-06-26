﻿using DLL.Enums;
using Services.Enums;
using Services.Helper;
using Services.Helper.LogHelper;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string HoaDonDienTuId { get; set; }

        [Display(Name = "Ngày gửi thông báo sai sót không phải lập hóa đơn")]
        public DateTime? NgayGuiTBaoSaiSotKhongPhaiLapHD { get; set; }

        [Display(Name = "Ngày hóa đơn")]
        public DateTime? NgayHoaDon { get; set; }

        [Display(Name = "Số hóa đơn")]
        public long? SoHoaDon { get; set; }

        [IgnoreLogging]
        public string MauHoaDonId { get; set; }

        [Display(Name = "Mẫu hóa đơn")]
        public int? LoaiThue { get; set; } // 1: một thuế suất, 2: nhiều thuế suất

        [Display(Name = "Mẫu số")]
        public string MauSo { get; set; }

        [Display(Name = "Ký hiệu")]
        public string KyHieu { get; set; }

        [IgnoreLogging]
        public string KyHieuHoaDon { get; set; }
        public string KyHieuMauSoHoaDon { get; set; }
        public string KyHieu1 { get; set; }
        public string KyHieu23 { get; set; }
        public string KyHieu4 { get; set; }
        public string KyHieu56 { get; set; }


        [IgnoreLogging]
        public MauHoaDonViewModel MauHoaDon { get; set; }

        [IgnoreLogging]
        public string BoKyHieuHoaDonId { get; set; }

        [IgnoreLogging]
        public BoKyHieuHoaDonViewModel BoKyHieuHoaDon { get; set; }

        [IgnoreLogging]
        public string KhachHangId { get; set; }

        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TenKhachHang { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }

        [Display(Name = "Người mua hàng")]
        public string HoTenNguoiMuaHang { get; set; }

        [Display(Name = "Căn cước công dân")]
        public string CanCuocCongDan { get; set; }

        [IgnoreLogging]
        public string TenKhachHangToSort { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SoDienThoaiNguoiMuaHang { get; set; }

        [Display(Name = "Email")]
        public string EmailNguoiMuaHang { get; set; }

        [Display(Name = "Tên ngân hàng")]
        public string TenNganHang { get; set; }

        [Display(Name = "Người nhận HĐ")]
        public string HoTenNguoiNhanHD { get; set; }

        [Display(Name = "Email người nhận HĐ")]
        public string EmailNguoiNhanHD { get; set; }

        [Display(Name = "Số điện thoại người nhận HĐ")]
        public string SoDienThoaiNguoiNhanHD { get; set; }

        [Display(Name = "Số tài khoản ngân hàng")]
        public string SoTaiKhoanNganHang { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel KhachHang { get; set; }

        [IgnoreLogging]
        public string HinhThucThanhToanId { get; set; }

        [Display(Name = "Hình thức thanh toán")]
        public string TenHinhThucThanhToan { get; set; }

        [IgnoreLogging]
        public string NhanVienBanHangId { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel NhanVienBanHang { get; set; }

        [Display(Name = "Mã nhân viên")]
        public string MaNhanVienBanHang { get; set; }

        [Display(Name = "Tên nhân viên")]
        public string TenNhanVienBanHang { get; set; }

        [IgnoreLogging]
        public string LoaiTienId { get; set; }

        [IgnoreLogging]
        public LoaiTienViewModel LoaiTien { get; set; }

        [Display(Name = "Tỷ giá")]
        public decimal? TyGia { get; set; }

        [IgnoreLogging]
        public int? TrangThai { get; set; } // DLL.Enums.TrangThaiHoaDon

        [IgnoreLogging]
        public int? TrangThaiQuyTrinh { get; set; } // DLL.Enums.TrangThaiQuyTrinh

        [IgnoreLogging]
        public string MaTraCuu { get; set; }

        [IgnoreLogging]
        public int? TrangThaiGuiHoaDon { get; set; } // DLL.Enums.TrangThaiGuiHoaDon
        [IgnoreLogging]
        public int? TrangThaiGuiHoaDonNhap { get; set; } // DLL.Enums.TrangThaiGuiHoaDon
        [IgnoreLogging]
        public int? TrangThaiGuiHoaDonMTT { get; set; }

        [IgnoreLogging]
        public bool? DaGuiThongBaoXoaBoHoaDon { get; set; }

        [IgnoreLogging]
        public bool? KhachHangDaNhan { get; set; }

        [IgnoreLogging]
        public int? SoLanChuyenDoi { get; set; }

        [IgnoreLogging]
        public DateTime? NgayXoaBo { get; set; }

        [IgnoreLogging]
        public string SoCTXoaBo { get; set; }

        [IgnoreLogging]
        public int TrangThaiBienBanXoaBo { get; set; } = 0;

        [IgnoreLogging]
        public string LyDoXoaBo { get; set; }

        [IgnoreLogging]
        public string LyDoHuyDieuChinhThayThe { get; set; }

        [IgnoreLogging]
        public int? LoaiHoaDon { get; set; } // DLL.Enums.LoaiHoaDon

        [IgnoreLogging]
        public DateTime? NgayLap { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel NguoiLap { get; set; }

        [IgnoreLogging]
        public int? LoaiChungTu { get; set; }

        [IgnoreLogging]
        public string TenLoaiChungTu { get; set; }

        [Display(Name = "Thời hạn thanh toán")]
        public DateTime? ThoiHanThanhToan { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiaoHang { get; set; }

        [IgnoreLogging]
        public List<HoaDonDienTuChiTietViewModel> HoaDonChiTiets { get; set; }

        [IgnoreLogging]
        public UserViewModel ActionUser { get; set; }

        [IgnoreLogging]
        public string ThamChieu { get; set; }

        [IgnoreLogging]
        public string TaiLieuDinhKem { get; set; }
        //public List<ThamChieuV2ViewModel> ThamChieus { get; set; }

        [IgnoreLogging]
        public string FileChuaKy { get; set; }

        [IgnoreLogging]
        public string FileDaKy { get; set; }

        [IgnoreLogging]
        public string XMLChuaKy { get; set; }

        [IgnoreLogging]
        public string XMLDaKy { get; set; }

        [IgnoreLogging]
        public string MaCuaCQT { get; set; }

        [IgnoreLogging]
        public bool? IsCapMa { get; set; } // = true khi nhận được thông điệp phản hồi 202 (Không dùng trong getbyid, vv...)

        [IgnoreLogging]
        public DateTime? NgayKy { get; set; }

        [IgnoreLogging]
        public string DataXML { get; set; }

        [IgnoreLogging]
        public bool? IsPhatHanh { get; set; }

        [IgnoreLogging]
        public bool? IsReloadSignedPDF { get; set; } // reload pdf đã ký nếu bị lỗi hiển thị

        [IgnoreLogging]
        public bool? IsCoSoHoaDon { get; set; }

        /// Thay thế
        [IgnoreLogging]
        public string ThayTheChoHoaDonId { get; set; }

        [IgnoreLogging]
        public bool? DaLapHoaDonThayThe { get; set; }

        [IgnoreLogging]
        public string LyDoThayThe { get; set; }

        [IgnoreLogging]
        public string DieuChinhChoHoaDonId { get; set; }

        [IgnoreLogging]
        public int? LoaiDieuChinh { get; set; } // DLL\Enums\LoaiDieuChinhHoaDon.cs

        [IgnoreLogging]
        public string Loai { get; set; }

        [IgnoreLogging]
        public string LyDoDieuChinh { get; set; }

        [IgnoreLogging]
        public bool DaBiDieuChinh { get; set; }

        [IgnoreLogging]
        public bool DaLapDieuChinh { get; set; }

        [IgnoreLogging]
        public int? TrangThaiLanDieuChinhGanNhat { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiLanDieuChinhGanNhat { get; set; }

        [IgnoreLogging]
        public int MauSoHoaDonLanDieuChinhGanNhat { get; set; }

        [IgnoreLogging]
        public string KyHieuHoaDonLanDieuChinhGanNhat { get; set; }

        [IgnoreLogging]
        public long? SoHoaDonLanDieuChinhGanNhat { get; set; }

        [IgnoreLogging]
        public DateTime? NgayHoaDonLanDieuChinhGanNhat { get; set; }

        [IgnoreLogging]
        public string BienBanDieuChinhId { get; set; }

        public string BienBanDieuChinhIdTmp { get; set; }

        [IgnoreLogging]
        public string BienBanXoaBoId { get; set; }

        [IgnoreLogging]
        public DateTime? NgayBienBanXoaBo { get; set; }

        [IgnoreLogging]
        public bool? IsHoaDonCoMa { get; set; }

        [IgnoreLogging]
        public string HinhThucDieuChinh { get; set; }

        public bool? BlockPhatHanhLai { get; set; }

        [IgnoreLogging]
        public string TrangThaiThoaThuan { get; set; }

        [IgnoreLogging]
        public bool? IsLapVanBanThoaThuan { get; set; }

        [IgnoreLogging]
        public DateTime? NgayLapBienBanDieuChinh { get; set; }

        [IgnoreLogging]
        public DateTime? NgayLapBienBanDieuChinhTmp { get; set; }

        [IgnoreLogging]
        public LoaiChietKhau LoaiChietKhau { get; set; }

        [IgnoreLogging]
        public decimal? TyLeChietKhau { get; set; }

        [IgnoreLogging]
        public bool? IsSentCQT { get; set; }

        [IgnoreLogging]
        public bool? IsBuyerSigned { get; set; }

        public bool BuyerSigned { get; set; }

        public DateTime? NgayNguoiMuaKy { get; set; }

        [IgnoreLogging]
        public int? SoLanGuiCQT { get; set; }

        [IgnoreLogging]
        public bool? IsHoaDonChoTCCNTKPTQ { get; set; } // hóa đơn dành cho tổ chức cá nhân trong khu phi thuế quan

        [IgnoreLogging]
        public bool? IsLapBienBanDieuChinh { get; set; }

        [IgnoreLogging]
        public bool? IsLapHoaDonDieuChinh { get; set; }

        [IgnoreLogging]
        public bool? IsLapHoaDonThayThe { get; set; }

        [IgnoreLogging]
        public string RefIdHoaDonXoaBoLanDau { get; set; }
        //////////////////////////////////////////////
        // Phiếu xuất kho
        public string CanCuSo { get; set; }
        [Display(Name = "Ngày")]
        public DateTime? NgayCanCu { get; set; }
        [Display(Name = "Của")]
        public string Cua { get; set; }
        public string DienGiai { get; set; }
        [Display(Name = "Địa chỉ kho nhận hàng")]
        public string DiaChiKhoNhanHang { get; set; }
        [Display(Name = "Họ tên người nhận hàng")]
        public string HoTenNguoiNhanHang { get; set; }
        [Display(Name = "Địa chỉ kho xuất hàng")]
        public string DiaChiKhoXuatHang { get; set; }
        [Display(Name = "Họ tên người xuất hàng")]
        public string HoTenNguoiXuatHang { get; set; }
        [Display(Name = "Hợp đồng vận chuyển số")]
        public string HopDongVanChuyenSo { get; set; }
        [Display(Name = "Tên người vận chuyển")]
        public string TenNguoiVanChuyen { get; set; }
        [Display(Name = "Phương thức vận chuyển")]
        public string PhuongThucVanChuyen { get; set; }
        ////////////////////////////////////////////////
        [Currency]
        [Display(Name = "Tổng tiền hàng")]
        public decimal? TongTienHang { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền chiết khấu")]
        public decimal? TongTienChietKhau { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thuế GTGT")]
        public decimal? TongTienThueGTGT { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thanh toán")]
        public decimal? TongTienThanhToan { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền hàng")]
        public decimal? TongTienHangQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền chiết khấu")]
        public decimal? TongTienChietKhauQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thuế GTGT")]
        public decimal? TongTienThueGTGTQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tổng tiền thanh toán")]
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        [Currency]
        public decimal? TongTienGiam { get; set; }

        [Currency]
        public decimal? TongTienGiamQuyDoi { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiHoaDon { get; set; }

        [Display(Name = "Loại tiền")]
        public string MaLoaiTien { get; set; }

        [IgnoreLogging]
        public int? LoaiApDungHoaDonCanThayThe { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDonCanThayThe { get; set; }

        [IgnoreLogging]
        public int? LoaiApDungHoaDonDieuChinh { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDonBiDieuChinh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiBienBanXoaBo { get; set; }

        [IgnoreLogging]
        public string Key { get; set; }

        [IgnoreLogging]
        public int? HinhThucHoaDon { get; set; }

        [IgnoreLogging]
        public string TenHinhThucHoaDon { get; set; }

        [IgnoreLogging]
        public int? UyNhiemLapHoaDon { get; set; }

        [IgnoreLogging]
        public string TenUyNhiemLapHoaDon { get; set; }

        [IgnoreLogging]
        public List<HoaDonDienTuViewModel> Children { get; set; }

        [IgnoreLogging]
        public List<HoaDonDienTuViewModel> HoaDons { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiQuyTrinh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiPhatHanh { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiGuiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiDieuChinh { get; set; }

        [IgnoreLogging]
        public int? TrangThaiBienBanDieuChinh { get; set; }
        public int? TrangThaiBienBanDieuChinhTmp { get; set; }
        [IgnoreLogging]
        public string TenTrangThaiBienBanDieuChinh { get; set; }
        public string TenTrangThaiBienBanDieuChinhTmp { get; set; }

        [IgnoreLogging]
        public string TenNguoiTao { get; set; }
        [IgnoreLogging]
        public DateTime? NgayTao { get; set; }
        [IgnoreLogging]
        public string TenNguoiCapNhat { get; set; }
        [IgnoreLogging]
        public DateTime? NgayCapNhat { get; set; }

        [IgnoreLogging]
        public bool? DaDieuChinh { get; set; }

        [IgnoreLogging]
        public string HoaDonDieuChinhId { get; set; }

        [IgnoreLogging]
        public bool? LapTuPMGP { get; set; }


        [IgnoreLogging]
        public string StrSoHoaDon { get; set; }

        [IgnoreLogging]
        public bool? IsVND { get; set; }

        [IgnoreLogging]
        public string SoTienBangChu { get; set; }

        [IgnoreLogging]
        public string ThongTinTao { get; set; }

        [IgnoreLogging]
        public string ThongTinCapNhat { get; set; }

        [IgnoreLogging]
        public string LyDoBiDieuChinh { get; set; }

        [IgnoreLogging]
        public LyDoDieuChinhModel LyDoDieuChinhModel { get; set; }

        public LyDoDieuChinhModel LyDoDieuChinhModelTmp { get; set; }

        [IgnoreLogging]
        public LyDoThayTheModel LyDoThayTheModel { get; set; }

        [IgnoreLogging]
        public TTChungThongDiep TTChungThongDiep { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }

        [IgnoreLogging]
        public ThongTinNguoiBanTrenMauHoaDonViewModel ThongTinNguoiBan { get; set; }
        public HoSoHDDTViewModel HoSoHDDT { get; set; }

        [Display(Name = "Trường thông tin bổ sung 1")]
        public string TruongThongTinBoSung1 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 2")]
        public string TruongThongTinBoSung2 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 3")]
        public string TruongThongTinBoSung3 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 4")]
        public string TruongThongTinBoSung4 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 5")]
        public string TruongThongTinBoSung5 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 6")]
        public string TruongThongTinBoSung6 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 7")]
        public string TruongThongTinBoSung7 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 8")]
        public string TruongThongTinBoSung8 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 9")]
        public string TruongThongTinBoSung9 { get; set; }

        [Display(Name = "Trường thông tin bổ sung 10")]
        public string TruongThongTinBoSung10 { get; set; }

        [Display(Name = "Giảm theo nghị quyết")]
        public bool? IsGiamTheoNghiQuyet { get; set; }

        [Display(Name = "Tỷ lệ % doanh thu")]
        public decimal? TyLePhanTramDoanhThu { get; set; }

        [Display(Name = "Thông tin người bán hoặc người mua")]
        public bool? IsThongTinNguoiBanHoacNguoiMua { get; set; }
        [Display(Name = "Thể hiện lý do")]
        public bool? IsTheHienLyDoTrenHoaDon { get; set; }

        public bool? IsNopThueTheoThongTu1032014BTC { get; set; }

        public string MCCQT { get; set; }

        [IgnoreLogging]
        public int? ImportType { get; set; } // 1: bổ sung, 2 thay thế

        public bool? IsDaLapThongBao04 { get; set; }
        public int? TrangThaiGui04 { get; set; }
        public int? LanGui04 { get; set; }
        public bool IsSended { get; set; }//đánh dấu hóa đơn được chọn gửi khi phát hành
        public bool? IsNotCreateThayThe { get; set; }//đánh dấu Hóa đơn xóa bỏ không cần lập thay thế
        public int? HinhThucXoabo { get; set; }
        public int? BackUpTrangThai { get; set; }
        public string PhuongThucChuyenDL { get; set; }

        [IgnoreLogging]
        public int? IsPos { get; set; } // có là hóa đơn tạo từ pos không

        [IgnoreLogging]
        public long? BienLaiId { get; set; } // Tương ứng với DonHangId của POS

        [IgnoreLogging]
        public bool? IsGuiTungHoaDon { get; set; }
        public int? TrangThaiGuiBangTongHop { get; set; }
        public string NoiDungGuiBangTongHop { get; set; }

        [IgnoreLogging]
        public bool? HoaDonNgoaiHeThong { get; set; } //đánh dấu là hóa đơn ngoài hệ thống

        [IgnoreLogging]
        public string DienGiaiTrangThaiHoaDon { get; set; } //diễn giải thêm về trạng thái hóa đơn

        [IgnoreLogging]
        public string MyProperty { get; set; }

        [IgnoreLogging]
        public bool IsTBaoHuyKhongDuocChapNhan { get; set; }

        [IgnoreLogging]
        public DateTime? NgayCapMa { get; set; }
        [IgnoreLogging]
        public bool? FromGP { get; set; } = false;
        public string PosCustomerURL { get; set; }

        public string HoSoDiaChi { get; set; }
        public string HoSoMaSoThue { get; set; }
        public string HoSoTenDonVi { get; set; }

        public void GetMoTaBienBanDieuChinh(string path)
        {
            Document document = new Document();
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph();

            TextRange text = paragraph.AppendText("Hai bên thống nhất lập biên bản này để điều chỉnh hóa đơn có mẫu số ");
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            text = paragraph.AppendText(MauSo);
            text.CharacterFormat.Bold = true;
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            text = paragraph.AppendText(" ký hiệu ");
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            text = paragraph.AppendText(KyHieu);
            text.CharacterFormat.Bold = true;
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            text = paragraph.AppendText(" số ");
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 11;

            text = paragraph.AppendText(SoHoaDon.ToString());
            text.CharacterFormat.Bold = true;
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 11;

            text = paragraph.AppendText(" ngày ");
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            text = paragraph.AppendText(NgayHoaDon.Value.ToString("dd/MM/yyyy"));
            text.CharacterFormat.Bold = true;
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            if (!string.IsNullOrEmpty(MaTraCuu))
            {
                text = paragraph.AppendText(" mã tra cứu ");
                text.CharacterFormat.FontName = "Times New Roman";
                text.CharacterFormat.FontSize = 12;

                text = paragraph.AppendText(MaTraCuu);
                text.CharacterFormat.Bold = true;
                text.CharacterFormat.FontName = "Times New Roman";
                text.CharacterFormat.FontSize = 12;
            }

            text = paragraph.AppendText(" theo quy định.");
            text.CharacterFormat.FontName = "Times New Roman";
            text.CharacterFormat.FontSize = 12;

            document.SaveToFile(path);
            document.Close();
        }

        [IgnoreLogging]
        public CotThongBaoSaiSotViewModel ThongBaoSaiSot { get; set; }

        [IgnoreLogging]
        public string ThongDiepGuiCQTId { get; set; }

        [IgnoreLogging]
        public bool? IsChildThayThe { get; set; }

        [IgnoreLogging]
        public bool? HoaDonThayTheDaDuocCapMa { get; set; }

        [IgnoreLogging]
        public bool? HoaDonDieuChinhDaDuocCapMa { get; set; }

        [IgnoreLogging]
        public string MaThongDiep { get; set; }

        [IgnoreLogging]
        public string IdHoaDonSaiSotBiThayThe { get; set; }

        [IgnoreLogging]
        public string GhiChuThayTheSaiSot { get; set; }

        [IgnoreLogging]
        public string FilterThongBaoSaiSot { get; set; }

        [IgnoreLogging]
        public bool? IsKemGuiEmail { get; set; }

        [IgnoreLogging]
        public string EmailNhanKemTheo { get; set; }

        [IgnoreLogging]
        public string TenNguoiNhanKemTheo { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn có liên quan (Ký hiệu mẫu số hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)(Chú thích: KHMSHDon.cs)</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn)</para>
        /// </summary>
        [MaxLength(11)]
        public string MauSoHoaDonLienQuan { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn có liên quan (Ký hiệu hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn)</para>
        /// </summary>
        [MaxLength(8)]
        public string KyHieuHoaDonLienQuan { get; set; }

        /// <summary>
        /// <para>Số hóa đơn có liên quan (Số hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn) </para>
        /// </summary>
        [MaxLength(8)]
        public string SoHoaDonLienQuan { get; set; }

        [IgnoreLogging]
        public DateTime? NgayHoaDonLienQuan { get; set; }

        [IgnoreLogging]
        public string ErrorMessage { get; set; }

        [IgnoreLogging]
        public bool HasError { get; set; }

        [IgnoreLogging]
        public bool HasHoaDonNhoHon { get; set; }

        [IgnoreLogging]
        public bool HasHoaDonKhacNhau { get; set; }

        [IgnoreLogging]
        public int? Count { get; set; }

        [IgnoreLogging]
        public string CMND { get; set; }

        [IgnoreLogging]
        public bool IsViewPDFWithoutSaveChange { get; set; }

        [Currency]
        [Display(Name = "Tổng giảm trừ không chịu thuế")]
        public decimal? TGTKCThue { get; set; }


        [Currency]
        [Display(Name = "Tổng giảm trừ khác")]
        public decimal? TGTKhac { get; set; }

        [IgnoreLogging]
        public int? HDNTGia { get; set; }

        [IgnoreLogging]
        public string SHChieu { get; set; }

        [IgnoreLogging]
        public DateTime? NCHChieu { get; set; }

        [IgnoreLogging]
        public DateTime? NHHHChieu { get; set; }

        [IgnoreLogging]
        public string QTich { get; set; }

        [IgnoreLogging]
        public ThongDiepChungViewModel ThongDiep { get; set; }
        [IgnoreLogging]
        public int? PhatHanhNgayPos { get; set; }
        [IgnoreLogging]
        public DateTime? TimeUpdateByNSD { get; set; }

        [IgnoreLogging]
        public bool? OptionEmailProcessHdMtt { get; set; }

        [IgnoreLogging]
        public bool? IsXacNhanDaGuiHDChoKhachHang { get; set; } = false;

        [IgnoreLogging]
        public bool IsTicket { get; set; }

        [IgnoreLogging]
        public string NguoiLapId { get; set; }
    }

    public class CotThongBaoSaiSotViewModel
    {
        public int? TrangThaiLapVaGuiThongBao { get; set; } //trạng thái đã lập và gửi thông báo 04
        public string LanGui { get; set; }
        public string DienGiaiChiTietTrangThai { get; set; }
        public bool? IsTrongHan { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
        public bool? IsCoGuiEmailSaiThongTin { get; set; }
        public bool? IsHoaDonDieuChinh { get; set; }
        public bool? IsHoaDonNgoaiHeThong { get; set; } //là những hóa đơn được nhập từ phần mềm khác
        public string TenTrangThai { get; set; }
    }

    public class KetQuaKiemTraLapTBao04ViewModel
    {
        public bool? IsDaLapThongBao { get; set; }
        public bool? IsDaGuiThongBao { get; set; }
        public ThongTinHoaDonRutGonViewModel HoaDonDieuChinh { get; set; }
    }

    public class CayThayTheViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string HoaDonDienTuChaId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class ThongKeSoLuongHoaDonCoSaiSotViewModel
    {
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public int SoLuong { get; set; }
        public bool IsDaLapThongBao04 { get; set; }
    }

    public class HoaDonKhongHopLeViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public bool IsTBaoHuyKhongDuocChapNhan { get; set; } = false;
    }

    public class HoaDonDaLapThongBao04ViewModel
    {
        public DLL.Entity.QuanLyHoaDon.ThongTinHoaDon HoaDonNgoaiHeThong { get; set; }
        public DLL.Entity.QuanLyHoaDon.HoaDonDienTu HoaDon { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
    }

    public class ThongTinHoaDonRutGonViewModel
    {
        public string MauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string NgayHoaDon { get; set; }
    }

    public class KetQuaThucHienPhatHanhDongLoat
    {
        public int? TrangThaiQuyTrinh { get; set; }
        public string TenTrangThaiQuyTrinh { get; set; }
        public List<string> HoaDonDienTuIds { get; set; }
    }
}
