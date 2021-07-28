using Services.Helper.LogHelper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Status = true;
            IsOnline = false;
        }

        [IgnoreLogging]
        public string UserId { get; set; }

        [IgnoreLogging]
        public string Password { get; set; }

        [IgnoreLogging]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [IgnoreLogging]
        public int? Gender { get; set; } // 1 nam 0 nữ còn lại không xác định

        [IgnoreLogging]
        public string Avatar { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        [IgnoreLogging]
        public DateTime? CreatedDate { get; set; }

        [IgnoreLogging]
        public DateTime? ModifyDate { get; set; }

        [IgnoreLogging]
        public string CreatedBy { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Chức danh")]
        public string Title { get; set; }

        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [IgnoreLogging]
        public bool? Status { get; set; }

        [IgnoreLogging]
        public string RoleId { get; set; }

        [IgnoreLogging]
        public string RoleName { get; set; }

        [IgnoreLogging]
        public bool? IsAdmin { get; set; }

        [IgnoreLogging]
        public bool? IsNodeAdmin { get; set; }

        [IgnoreLogging]
        public bool? IsOnline { get; set; }

        [IgnoreLogging]
        public int? LoginCount { get; set; }
    }
}
