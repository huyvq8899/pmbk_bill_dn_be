using AutoMapper;
using DLL.Entity;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using DLL.Entity.TienIch;
using DLL.Entity.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.TienIch;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.BaoCao;
using DLL.Entity.BaoCao;
using DLL.Entity.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using DLL.Entity.QuanLy;
using Services.ViewModels.QuanLy;

namespace Services.AutoMapper
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            #region Hệ thống
            CreateMap<User, UserViewModel>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<Function_Role, Function_RoleViewModel>();
            CreateMap<Permission, PermissionViewModel>();
            CreateMap<Function_User, Function_UserViewModel>();
            CreateMap<User_Role, User_RoleViewModel>();
            CreateMap<ThaoTac, ThaoTacViewModel>();
            CreateMap<Function_ThaoTac, Function_ThaoTacViewModel>();
            CreateMap<KyKeToan, KyKeToanViewModel>();
            CreateMap<TuyChon, TuyChonViewModel>();
            CreateMap<ConfigNoiDungEmail, ConfigNoiDungEmailViewModel>();
            CreateMap<ThietLapTruongDuLieu, ThietLapTruongDuLieuViewModel>();
            CreateMap<PhanQuyenMauHoaDon, PhanQuyenMauHoaDonViewModel>();
            #endregion

            #region Danh mục
            CreateMap<CoQuanThue, CoQuanThueViewModel>();
            CreateMap<DoiTuong, DoiTuongViewModel>();
            CreateMap<DonViTinh, DonViTinhViewModel>();
            CreateMap<HangHoaDichVu, HangHoaDichVuViewModel>();
            CreateMap<LoaiTien, LoaiTienViewModel>();
            CreateMap<HoSoHDDT, HoSoHDDTViewModel>();
            CreateMap<MauHoaDon, MauHoaDonViewModel>();
            CreateMap<MauHoaDonThietLapMacDinh, MauHoaDonThietLapMacDinhViewModel>();
            CreateMap<MauHoaDonTuyChinhChiTiet, MauHoaDonTuyChinhChiTietViewModel>();
            CreateMap<MauHoaDonFile, MauHoaDonFileViewModel>();
            CreateMap<TaiLieuDinhKem, TaiLieuDinhKemViewModel>();
            #endregion
            CreateMap<AlertStartup, AlertStartupViewModel>();
            CreateMap<HopDongHoaDon, HopDongHoaDonViewModel>();

            #region Tiện ích
            CreateMap<NhatKyGuiEmail, NhatKyGuiEmailViewModel>();
            CreateMap<NhatKyTruyCap, NhatKyTruyCapViewModel>();
            CreateMap<NhatKyThaoTacLoi, NhatKyThaoTacLoiViewModel>();
            #endregion

            #region Hóa đơn
            CreateMap<HoaDonDienTu, HoaDonDienTuViewModel>();
            CreateMap<HoaDonDienTuChiTiet, HoaDonDienTuChiTietViewModel>();
            CreateMap<LuuTruTrangThaiBBXB, LuuTruTrangThaiBBXBViewModel>();
            CreateMap<LuuTruTrangThaiBBDT, LuuTruTrangThaiBBDTViewModel>();
            CreateMap<LuuTruTrangThaiFileHDDT, LuuTruTrangThaiFileHDDTViewModel>();
            CreateMap<NhatKyThaoTacHoaDon, NhatKyThaoTacHoaDonViewModel>();
            CreateMap<ThongTinChuyenDoi, ThongTinChuyenDoiViewModel>();
            CreateMap<BienBanXoaBo, BienBanXoaBoViewModel>();
            CreateMap<BienBanDieuChinh, BienBanDieuChinhViewModel>();
            #endregion

            #region Thông điệp
            CreateMap<ThongDiepGuiCQT, ThongDiepGuiCQTViewModel>();
            CreateMap<ThongDiepChiTietGuiCQT, ThongDiepChiTietGuiCQTViewModel>();
            CreateMap<ThongBaoHoaDonRaSoat, ThongBaoHoaDonRaSoatViewModel>();
            CreateMap<ThongBaoChiTietHoaDonRaSoat, ThongBaoChiTietHoaDonRaSoatViewModel>();
            #endregion

            #region Báo cáo
            CreateMap<NghiepVu, NghiepVuViewModel>();
            CreateMap<TruongDuLieu, TruongDuLieuViewModel>();
            CreateMap<BaoCaoTinhHinhSuDungHoaDon, BaoCaoTinhHinhSuDungHoaDonViewModel>();
            CreateMap<BaoCaoTinhHinhSuDungHoaDonChiTiet, BaoCaoTinhHinhSuDungHoaDonChiTietViewModel>();
            #endregion

            #region Quy định kỹ thuật
            CreateMap<ToKhaiDangKyThongTin, ToKhaiDangKyThongTinViewModel>();
            CreateMap<DuLieuGuiHDDT, DuLieuGuiHDDTViewModel>();
            CreateMap<DuLieuGuiHDDTChiTiet, DuLieuGuiHDDTChiTietViewModel>();
            CreateMap<ThongDiepChung, ThongDiepChungViewModel>();
            CreateMap<DangKyUyNhiem, DangKyUyNhiemViewModel>();
            CreateMap<ChungThuSoSuDung, ChungThuSoSuDungViewModel>();
            CreateMap<BangTongHopDuLieuHoaDon, BangTongHopDuLieuHoaDonViewModel>();
            CreateMap<BangTongHopDuLieuHoaDonChiTiet, BangTongHopDuLieuHoaDonChiTietViewModel>();
            #endregion

            #region Quản lý
            CreateMap<BoKyHieuHoaDon, BoKyHieuHoaDonViewModel>();
            CreateMap<NhatKyXacThucBoKyHieu, NhatKyXacThucBoKyHieuViewModel>();
            CreateMap<QuanLyThongTinHoaDon, QuanLyThongTinHoaDonViewModel>();
            CreateMap<SinhSoHDMayTinhTien, SinhSoHDMayTinhTienViewModels>();
            #endregion
        }
    }
}
