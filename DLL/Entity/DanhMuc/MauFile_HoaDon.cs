using System.ComponentModel.DataAnnotations;

namespace DLL.Entity.DanhMuc
{
    public class MauFile_HoaDon : ThongTinChung
    {
        [Key]
        [MaxLength(36)]
        public string MauFile_HoaDonId { get; set; }
        [MaxLength(36)]
        public string HoaDonDienTuId { get; set; }
        [MaxLength(36)]
        public string MauHoaDonFileId { get; set; }
    }
}
