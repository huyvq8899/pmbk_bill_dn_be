using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity
{
    public class Function
    {
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ParentFunctionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public int? STT { get; set; }
        public IList<Function_User> Function_Users { get; set; }
        public IList<Function_Role> Function_Roles { get; set; }
    }
}
