﻿using Services.ViewModels.QuanLyHoaDonDienTu;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class ThongDiepGuiHDDTKhongMaDuLieuViewModel : ThongTinChungViewModel
    {
        public string ThongDiepGuiHDDTKhongMaDuLieuId { get; set; }
        public string ThongDiepGuiHDDTKhongMaId { get; set; }
        public string HoaDonDienTuId { get; set; }
        //
        public HoaDonDienTuViewModel HoaDonDienTu { get; set; }
    }
}
