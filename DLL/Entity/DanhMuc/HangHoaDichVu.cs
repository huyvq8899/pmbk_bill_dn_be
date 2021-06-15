using DLL.Enums;

namespace DLL.Entity.DanhMuc
{
    public class HangHoaDichVu : ThongTinChung
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

        public DonViTinh DonViTinh { get; set; }
    }
}
