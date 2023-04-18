using System.Collections.Generic;

namespace DLL.Entity.DanhMuc
{
    public class DonViTinh : ThongTinChung
    {
        public string DonViTinhId { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }

        public List<HangHoaDichVu> HangHoaDichVus { get; set; }
    }
}
