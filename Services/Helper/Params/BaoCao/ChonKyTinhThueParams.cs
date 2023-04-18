using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.BaoCao
{
    public class ChonKyTinhThueParams
    {
        public int? Thang { get; set; }
        public int Nam { get; set; }
        public int? Quy { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public UserViewModel ActionUser { get; set; }
    }
}
