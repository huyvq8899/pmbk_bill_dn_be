using AutoMapper;
using DLL.Entity;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;

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

            #region Danh mục
            CreateMap<DoiTuong, DoiTuongViewModel>();
            CreateMap<DonViTinh, DonViTinhViewModel>();
            CreateMap<HangHoaDichVu, HangHoaDichVuViewModel>();
            CreateMap<LoaiTien, LoaiTienViewModel>();
            CreateMap<HoSoHDDT, HoSoHDDTViewModel>();
            CreateMap<MauHoaDon, MauHoaDonViewModel>();
            CreateMap<ThongBaoPhatHanh, ThongBaoPhatHanhViewModel>();
            CreateMap<ThongBaoPhatHanhChiTiet, ThongBaoPhatHanhChiTietViewModel>();
            #endregion
            //Hóa đơn điện tử
            CreateMap<HoaDonDienTu, HoaDonDienTuViewModel>();
            CreateMap<HoaDonDienTuChiTiet, HoaDonDienTuChiTietViewModel>();
            CreateMap<LuuTruTrangThaiFileHDDT, LuuTruTrangThaiFileHDDTViewModel>();
            CreateMap<NhatKyThaoTacHoaDon, NhatKyThaoTacHoaDonViewModel>();
            CreateMap<ThongTinChuyenDoi, ThongTinChuyenDoiViewModel>();
            CreateMap<BienBanXoaBo, BienBanXoaBoViewModel>();
        }
    }
}
