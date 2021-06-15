using AutoMapper;
using DLL.Entity;
using DLL.Entity.Config;
using Services.ViewModels;
using Services.ViewModels.Config;

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
        }
    }
}
