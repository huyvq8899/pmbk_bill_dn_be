using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity
{
    public class User_Role
    {
        public string URID { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
