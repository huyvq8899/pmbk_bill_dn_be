using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity
{
    public class Role
    {
        public string RoleId { get; set; }
        [StringLength(200)]
        public string RoleName { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; }
        public IList<User> Users { get; set; }
        public IList<Function_Role> Function_Roles { get; set; }
        public IList<User_Role> User_Roles { get; set; }
    }
}
