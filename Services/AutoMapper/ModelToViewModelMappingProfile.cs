using AutoMapper;
using DLL.Entity;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using Services.ViewModels;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;

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

            #region Danh mục
            CreateMap<DoiTuong, DoiTuongViewModel>();
            CreateMap<DonViTinh, DonViTinhViewModel>();
            CreateMap<HangHoaDichVu, HangHoaDichVuViewModel>();
            CreateMap<LoaiTien, LoaiTienViewModel>();
            CreateMap<HoSoHDDT, HoSoHDDTViewModel>();
            CreateMap<MauHoaDon, MauHoaDonViewModel>();
            CreateMap<ThongBaoPhatHanh, ThongBaoPhatHanhViewModel>();
            CreateMap<ThongBaoPhatHanhChiTiet, ThongBaoPhatHanhChiTietViewModel>();
            CreateMap<ThongBaoKetQuaHuyHoaDon, ThongBaoKetQuaHuyHoaDonViewModel>();
            CreateMap<ThongBaoKetQuaHuyHoaDonChiTiet, ThongBaoKetQuaHuyHoaDonChiTietViewModel>();
            CreateMap<ThongBaoDieuChinhThongTinHoaDon, ThongBaoDieuChinhThongTinHoaDonViewModel>();
            CreateMap<ThongBaoDieuChinhThongTinHoaDonChiTiet, ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>();
            #endregion
        }
    }
}
