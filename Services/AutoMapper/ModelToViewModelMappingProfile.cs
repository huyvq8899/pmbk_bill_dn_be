using AutoMapper;
using DLL.Entity;
using DLL.Entity.Config;
using Services.ViewModels;
using Services.ViewModels.Config;

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
        }
    }
}
