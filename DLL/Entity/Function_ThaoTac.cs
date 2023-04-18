using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity
{
    public class Function_ThaoTac
    {
        [Key]
        public string FTID { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string FunctionId { get; set; }
        public string ThaoTacId { get; set; }
        public string PermissionId { get; set; }
        public bool Active { get; set; }

        public virtual User User { get; set; }
        public virtual Function Function { get; set; }
        public virtual ThaoTac ThaoTac { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
