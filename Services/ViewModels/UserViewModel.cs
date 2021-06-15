using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            this.Status = true;
            this.IsOnline = false;
        }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; } // 1 nam 0 nữ còn lại không xác định
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsNodeAdmin { get; set; }


        public bool? IsOnline { get; set; }
        public int? LoginCount { get; set; }
        //public string DatabaseName { get; set; }
        //public string ConnectionString { get; set; }

        public string KyKeToanFromDate { get; set; }
        public string KyKeToanToDate { get; set; }
    }
}
