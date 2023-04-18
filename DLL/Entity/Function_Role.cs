using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity
{
    public class Function_Role
    {
        public string FRID { get; set; }
        public string FunctionId { get; set; }
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public bool Active { get; set; } //code cũ có trường active

        public Function Function { get; set; }
        public Permission Permission { get; set; }
        public Role Role { get; set; }
    }
}
