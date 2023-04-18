using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity
{
    public class Permission
    {
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool? Status { get; set; }

        public IList<Function_User> Function_Users { get; set; }
        public IList<Function_Role> Function_Roles { get; set; }
    }
}
