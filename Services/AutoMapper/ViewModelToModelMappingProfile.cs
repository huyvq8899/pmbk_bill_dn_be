using AutoMapper;
using DLL.Entity;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using Services.ViewModels;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;

namespace Services.AutoMapper
{
    public class ViewModelToModelMappingProfile : Profile
    {
        public ViewModelToModelMappingProfile()
        {
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

            //Danh mục
            CreateMap<DoiTuongViewModel, DoiTuong>();
            CreateMap<HangHoaDichVuViewModel, HangHoaDichVu>();
            CreateMap<DonViTinhViewModel, DonViTinh>();
            CreateMap<LoaiTienViewModel, LoaiTien>();
            CreateMap<MauHoaDonViewModel, MauHoaDon>();
            CreateMap<HoSoHDDTViewModel, HoSoHDDT>();
            CreateMap<HinhThucThanhToanViewModel, HinhThucThanhToan>();
            CreateMap<MauHoaDonViewModel, MauHoaDon>();
            CreateMap<ThongBaoPhatHanhViewModel, ThongBaoPhatHanh>();
            CreateMap<ThongBaoPhatHanhChiTietViewModel, ThongBaoPhatHanhChiTiet>();

            //Hóa đơn điện tử
            CreateMap<HoaDonDienTuViewModel, HoaDonDienTu>();
            CreateMap<HoaDonDienTuChiTietViewModel, HoaDonDienTuChiTiet>();
            CreateMap<LuuTruTrangThaiFileHDDTViewModel, LuuTruTrangThaiFileHDDT>();
            CreateMap<LuuTruTrangThaiBBXBViewModel, LuuTruTrangThaiBBXB>();
            CreateMap<NhatKyThaoTacHoaDonViewModel, NhatKyThaoTacHoaDon>();
            CreateMap<ThongTinChuyenDoiViewModel, ThongTinChuyenDoi>();
            CreateMap<BienBanXoaBoViewModel, BienBanXoaBo>();
            CreateMap<ThongBaoKetQuaHuyHoaDonViewModel, ThongBaoKetQuaHuyHoaDon>();
            CreateMap<ThongBaoKetQuaHuyHoaDonChiTietViewModel, ThongBaoKetQuaHuyHoaDonChiTiet>();
            CreateMap<ThongBaoDieuChinhThongTinHoaDonViewModel, ThongBaoDieuChinhThongTinHoaDon>();
            CreateMap<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel, ThongBaoDieuChinhThongTinHoaDonChiTiet>();
            CreateMap<TaiLieuDinhKemViewModel, TaiLieuDinhKem>();
        }
    }
}
