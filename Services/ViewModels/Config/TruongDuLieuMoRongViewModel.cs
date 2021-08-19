using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Config
{
    public class TruongDuLieuMoRongViewModel
    {
        public string Id { get; set; }
        public string DataId { get; set; }
        public string TenTruong { get; set; }
        public string TenTruongHienThi { get; set; }
        public string DuLieu { get; set; }
        public bool HienThi { get; set; }
    }
}
