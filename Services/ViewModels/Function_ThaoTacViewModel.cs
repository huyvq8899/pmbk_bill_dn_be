using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class Function_ThaoTacViewModel
    {
        public string FTID { get; set; }
        public string RoleId { get; set; }
        public string FunctionId { get; set; }
        public string ThaoTacId { get; set; }
        public string PermissionId { get; set; }
        public bool Active { get; set; }

        public FunctionViewModel Function { get; set; }
        public ThaoTacViewModel ThaoTac { get; set; }
        public PermissionViewModel Permission { get; set; }
    }
}
