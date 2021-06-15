using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class Function_RoleViewModel
    {
        public Function_RoleViewModel()
        {
            this.Active = true;
        }
        public string FRID { get; set; }
        public string FunctionId { get; set; }
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public bool? Active { get; set; } //code cũ có trường active

        public string FunctionName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Type { get; set; }
        public string RoleName { get; set; }
    }
}
