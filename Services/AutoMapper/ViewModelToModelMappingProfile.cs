using AutoMapper;
using DLL.Entity;
using DLL.Entity.BaoCao;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Entity.Ticket;
using DLL.Entity.TienIch;
using Services.ViewModels;
using Services.ViewModels.BaoCao;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.Ticket;
using Services.ViewModels.TienIch;

namespace Services.AutoMapper
{
    public class ViewModelToModelMappingProfile : Profile
    {
        public ViewModelToModelMappingProfile()
        {
            #region Hệ thống
            CreateMap<UserViewModel, User>();
            CreateMap<RoleViewModel, Role>();
            CreateMap<FunctionViewModel, Function>();
            CreateMap<Function_RoleViewModel, Function_Role>();
            CreateMap<PermissionViewModel, Permission>();
            CreateMap<Function_UserViewModel, Function_User>();
            CreateMap<User_RoleViewModel, User_Role>();
            CreateMap<ThaoTacViewModel, ThaoTac>();
            CreateMap<Function_ThaoTacViewModel, Function_ThaoTac>();
            CreateMap<KyKeToanViewModel, KyKeToan>();
            CreateMap<TuyChonViewModel, TuyChon>();
            CreateMap<ConfigNoiDungEmailViewModel, ConfigNoiDungEmail>();
            CreateMap<ThietLapTruongDuLieuViewModel, ThietLapTruongDuLieu>();
            CreateMap<PhanQuyenMauHoaDonViewModel, PhanQuyenMauHoaDon>();
            #endregion

            #region Danh mục
            CreateMap<CoQuanThueViewModel, CoQuanThue>();
            CreateMap<DoiTuongViewModel, DoiTuong>();
            CreateMap<HangHoaDichVuViewModel, HangHoaDichVu>();
            CreateMap<DonViTinhViewModel, DonViTinh>();
            CreateMap<LoaiTienViewModel, LoaiTien>();
            CreateMap<MauHoaDonViewModel, MauHoaDon>();
            CreateMap<HoSoHDDTViewModel, HoSoHDDT>();
            CreateMap<MauHoaDonViewModel, MauHoaDon>();
            CreateMap<MauHoaDonThietLapMacDinhViewModel, MauHoaDonThietLapMacDinh>();
            CreateMap<MauHoaDonTuyChinhChiTietViewModel, MauHoaDonTuyChinhChiTiet>();
            CreateMap<MauHoaDonFileViewModel, MauHoaDonFile>();
            CreateMap<AlertStartupViewModel, AlertStartup>();
            #endregion

            #region Hóa đơn điện tử
            CreateMap<HoaDonDienTuViewModel, HoaDonDienTu>();
            CreateMap<HoaDonDienTuChiTietViewModel, HoaDonDienTuChiTiet>();
            CreateMap<LuuTruTrangThaiFileHDDTViewModel, LuuTruTrangThaiFileHDDT>();
            CreateMap<LuuTruTrangThaiBBXBViewModel, LuuTruTrangThaiBBXB>();
            CreateMap<LuuTruTrangThaiBBDTViewModel, LuuTruTrangThaiBBDT>();
            CreateMap<NhatKyThaoTacHoaDonViewModel, NhatKyThaoTacHoaDon>();
            CreateMap<ThongTinChuyenDoiViewModel, ThongTinChuyenDoi>();
            CreateMap<BienBanXoaBoViewModel, BienBanXoaBo>();
            CreateMap<BienBanDieuChinhViewModel, BienBanDieuChinh>();
            #endregion

            #region Thông điệp
            CreateMap<ThongDiepGuiCQTViewModel, ThongDiepGuiCQT>();
            CreateMap<ThongDiepChiTietGuiCQTViewModel, ThongDiepChiTietGuiCQT>();
            CreateMap<ThongBaoHoaDonRaSoatViewModel, ThongBaoHoaDonRaSoat>();
            CreateMap<ThongBaoChiTietHoaDonRaSoatViewModel, ThongBaoChiTietHoaDonRaSoat>();
            #endregion

            #region Tiện ích
            CreateMap<NhatKyTruyCapViewModel, NhatKyTruyCap>();
            CreateMap<NhatKyGuiEmailViewModel, NhatKyGuiEmail>();
            CreateMap<NhatKyThaoTacLoiViewModel, NhatKyThaoTacLoi>();
            #endregion

            #region Báo cáo
            CreateMap<NghiepVuViewModel, NghiepVu>();
            CreateMap<TruongDuLieuViewModel, TruongDuLieu>();
            CreateMap<BaoCaoTinhHinhSuDungHoaDonViewModel, BaoCaoTinhHinhSuDungHoaDon>();
            CreateMap<BaoCaoTinhHinhSuDungHoaDonChiTietViewModel, BaoCaoTinhHinhSuDungHoaDonChiTiet>();
            #endregion

            #region Quy định kỹ thuật
            CreateMap<ToKhaiDangKyThongTinViewModel, ToKhaiDangKyThongTin>();
            CreateMap<DuLieuGuiHDDTViewModel, DuLieuGuiHDDT>();
            CreateMap<DuLieuGuiHDDTChiTietViewModel, DuLieuGuiHDDTChiTiet>();
            CreateMap<ThongDiepChungViewModel, ThongDiepChung>();
            CreateMap<DangKyUyNhiemViewModel, DangKyUyNhiem>();
            CreateMap<ChungThuSoSuDungViewModel, ChungThuSoSuDung>();
            CreateMap<BangTongHopDuLieuHoaDonViewModel, BangTongHopDuLieuHoaDon>();
            CreateMap<BangTongHopDuLieuHoaDonChiTietViewModel, BangTongHopDuLieuHoaDonChiTiet>();
            #endregion

            #region Quản lý
            CreateMap<BoKyHieuHoaDonViewModel, BoKyHieuHoaDon>();
            CreateMap<NhatKyXacThucBoKyHieuViewModel, NhatKyXacThucBoKyHieu>();
            CreateMap<QuanLyThongTinHoaDonViewModel, QuanLyThongTinHoaDon>();
            #endregion

            #region Ticket
            CreateMap<TuyenDuongViewModel, TuyenDuong>();
            CreateMap<XeViewModel, Xe>();
            #endregion
        }
    }
}
