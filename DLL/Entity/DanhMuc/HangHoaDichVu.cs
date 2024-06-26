﻿using DLL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entity.DanhMuc
{
    public class HangHoaDichVu : ThongTinChung
    {
        public string HangHoaDichVuId { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DonGiaBan { get; set; }
        public bool? IsGiaBanLaDonGiaSauThue { get; set; }
        public string ThueGTGT { get; set; } // %
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TyLeChietKhau { get; set; } // %
        public string MoTa { get; set; }

        // Khóa ngoại
        public string DonViTinhId { get; set; }

        public DonViTinh DonViTinh { get; set; }
    }
}
