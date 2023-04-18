using System;

namespace Services.ViewModels.QuanLy
{
    public class SinhSoHDMayTinhTienViewModels : ThongTinChungViewModel
    {
        public Guid SinhSoHDMayTinhTienId { get; set; }
        public int NamPhatHanh { get; set; }
        public long SoBatDau { get; set; }
        public long SoKetThuc { get; set; }
        public bool IsUpdateSoBatDau { get; set; }
        public bool IsUpdateSoKetThuc { get; set; }
    }
}

