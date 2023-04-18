using System.Collections.Generic;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class DuLieuGuiHDDT : ThongTinChung
    {
        public string DuLieuGuiHDDTId { get; set; }

        // TTChung
        public string HoaDonDienTuId { get; set; }
        ///
        public List<DuLieuGuiHDDTChiTiet> DuLieuGuiHDDTChiTiets { get; set; }

        ///trường hợp gửi thông điệp 206
        public string ThongDiepChungId { get; set; }
    }
}
