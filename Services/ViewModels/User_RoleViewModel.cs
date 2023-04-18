using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class User_RoleViewModel
    {
        public string URID { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }

    public class User_Role_ByUserViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public bool? IsNodeAdmin { get; set; }
    }
}
