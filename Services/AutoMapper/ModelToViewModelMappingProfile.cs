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

namespace Services.AutoMapper
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
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
            CreateMap<TruongDuLieuMoRong, TruongDuLieuMoRongViewModel>();
            CreateMap<ThietLapTruongDuLieuMoRong, ThietLapTruongDuLieuMoRongViewModel>();
            CreateMap<PhanQuyenMauHoaDon, PhanQuyenMauHoaDonViewModel>();

            #region Danh mục
            CreateMap<DoiTuong, DoiTuongViewModel>();
            CreateMap<DonViTinh, DonViTinhViewModel>();
            CreateMap<HangHoaDichVu, HangHoaDichVuViewModel>();
            CreateMap<LoaiTien, LoaiTienViewModel>();
            CreateMap<HoSoHDDT, HoSoHDDTViewModel>();
            CreateMap<MauHoaDon, MauHoaDonViewModel>();
            CreateMap<MauHoaDonThietLapMacDinh, MauHoaDonThietLapMacDinhViewModel>();
            CreateMap<MauHoaDonTuyChinhChiTiet, MauHoaDonTuyChinhChiTietViewModel>();
            CreateMap<HinhThucThanhToan, HinhThucThanhToanViewModel>();
            CreateMap<ThongBaoPhatHanh, ThongBaoPhatHanhViewModel>();
            CreateMap<ThongBaoPhatHanhChiTiet, ThongBaoPhatHanhChiTietViewModel>();
            CreateMap<ThongBaoKetQuaHuyHoaDon, ThongBaoKetQuaHuyHoaDonViewModel>();
            CreateMap<ThongBaoKetQuaHuyHoaDonChiTiet, ThongBaoKetQuaHuyHoaDonChiTietViewModel>();
            CreateMap<ThongBaoDieuChinhThongTinHoaDon, ThongBaoDieuChinhThongTinHoaDonViewModel>();
            CreateMap<ThongBaoDieuChinhThongTinHoaDonChiTiet, ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>();
            CreateMap<QuyetDinhApDungHoaDon, QuyetDinhApDungHoaDonViewModel>();
            CreateMap<QuyetDinhApDungHoaDonDieu1, QuyetDinhApDungHoaDonDieu1ViewModel>();
            CreateMap<QuyetDinhApDungHoaDonDieu2, QuyetDinhApDungHoaDonDieu2ViewModel>();
            CreateMap<TaiLieuDinhKem, TaiLieuDinhKemViewModel>();
            #endregion

            #region Tiện ích
            CreateMap<NhatKyGuiEmail, NhatKyGuiEmailViewModel>();
            CreateMap<NhatKyTruyCap, NhatKyTruyCapViewModel>();
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
            CreateMap<TruongDuLieuHoaDon, TruongDuLieuHoaDonViewModel>();
            #endregion

            #region Báo cáo
            CreateMap<NghiepVu, NghiepVuViewModel>();
            CreateMap<TruongDuLieu, TruongDuLieuViewModel>();
            CreateMap<BaoCaoTinhHinhSuDungHoaDon, BaoCaoTinhHinhSuDungHoaDonViewModel>();
            CreateMap<BaoCaoTinhHinhSuDungHoaDonChiTiet, BaoCaoTinhHinhSuDungHoaDonChiTietViewModel>();
            #endregion
        }
    }
}
