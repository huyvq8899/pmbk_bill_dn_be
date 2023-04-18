using Services.ViewModels;
using System.Collections.Generic;

namespace Services.Helper
{
    //Param này được dùng khi phân quyền cho role
    public class FunctionRoleParams
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] FunctionIds { get; set; }
        public List<ThaoTacViewModel> ThaoTacs { get; set; }
    }

    //Param này được dùng khi phân quyền cho user
    public class UserRoleParams
    {
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsNodeAdmin { get; set; }
        public string[] RoleIds { get; set; }
        public string[] FunctionIds { get; set; }
    }
}
