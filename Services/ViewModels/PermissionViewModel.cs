using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class PermissionViewModel
    {
        public PermissionViewModel()
        {
            this.Status = true;
        }
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool? Status { get; set; }
    }
}
