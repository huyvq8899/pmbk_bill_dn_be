using DLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.DanhMuc
{
    public class HangHoaDichVuViewModel
    {
        public string HangHoaDichVuId { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public decimal? DonGiaBan { get; set; }
        public bool? IsGiaBanLaDonGiaSauThue { get; set; }
        public ThueGTGT ThueGTGT { get; set; } // %
        public decimal? TyLeChietKhau { get; set; } // %
        public string DiaChi { get; set; }

        // Khóa ngoại
        public string DonViTinhId { get; set; }

        public DonViTinhViewModel DonViTinh { get; set; }
    }
}
