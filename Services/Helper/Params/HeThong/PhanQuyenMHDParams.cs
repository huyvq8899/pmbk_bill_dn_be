using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HeThong
{
    public class PhanQuyenMHDParams
    {
        public string RoleId { get; set; }
        public List<PhanQuyenMauHoaDonViewModel> ListPQ { get; set; }
    }
}
