using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity
{
    public class Function_User
    {
        public string FUID { get; set; }
        public string UserId { get; set; }
        public string FunctionId { get; set; }
        public string PermissionId { get; set; }

        public Function Function { get; set; }
        public Permission Permission { get; set; }
        public User User { get; set; }
    }
}
