using System;
using System.Collections.Generic;

namespace Services.ViewModels.Ticket
{
    public class TuyenDuongViewModel : ThongTinChungViewModel
    {
        public Guid? TuyenDuongId { get; set; }
        public string TenTuyenDuong { get; set; }
        public string BenDi { get; set; }
        public string BenDen { get; set; }
        public string ThoiGianKhoiHanh { get; set; }
        public string SoXe { get; set; }
        public string SoTuyen { get; set; }

        public List<XeViewModel> Xes { get; set; }
    }
}
