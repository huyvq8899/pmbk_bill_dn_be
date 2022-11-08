using DLL.Entity.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity
{
    public class User
    {
        public string UserId { get; set; }
        [StringLength(200)]
        public string Password { get; set; }
        [StringLength(200)]
        public string ConfirmPassword { get; set; }
        [StringLength(200)]
        public string UserName { get; set; } // lấy làm tag
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(200)]
        public string FullName { get; set; }
        public int? Gender { get; set; } // 1 nam 0 nữ còn lại không xác định
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        [StringLength(200)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Title { get; set; }

        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsNodeAdmin { get; set; }
        public IList<Function_User> Function_Users { get; set; }
        public IList<User_Role> User_Roles { get; set; }
        public List<User_Xe> User_Xes { get; set; }

        // Chat
        public bool? IsOnline { get; set; }
        public int? LoginCount { get; set; }

    }
}
