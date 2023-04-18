using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            this.Status = true;
        }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Status { get; set; }
    }
}
