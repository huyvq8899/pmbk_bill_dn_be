using DLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLy
{
    public class NhatKyXacThucBoKyHieuViewModel : ThongTinChungViewModel
    {
        public string NhatKyXacThucBoKyHieuId { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public TrangThaiSuDung TrangThaiSuDung { get; set; }
        public string NoiDung { get; set; }
        public string MauHoaDonId { get; set; }
        public string ToKhaiId { get; set; }

        public BoKyHieuHoaDonViewModel BoKyHieuHoaDon { get; set; }
    }
}
