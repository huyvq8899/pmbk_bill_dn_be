using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class PermissionUserMViewModel
    {
        public List<PemissionUserViewModel> Functions { get; set; }
        public List<string> MauHoaDonIds { get; set; }
    }

    public class PemissionUserViewModel
    {
        public string FunctionName { get; set; }
        public List<string> ThaoTacs { get; set; }
    }
}
