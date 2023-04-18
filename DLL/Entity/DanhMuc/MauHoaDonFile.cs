using DLL.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DLL.Entity.DanhMuc
{
    public class MauHoaDonFile : ThongTinChung
    {
        public string MauHoaDonFileId { get; set; }
        public string MauHoaDonId { get; set; }
        public HinhThucMauHoaDon Type { get; set; }
        public string FileName { get; set; }
        public byte[] Binary { get; set; }

        public MauHoaDon MauHoaDon { get; set; }
        [MaxLength(20)]
        public string MaSoThueGiaiPhap { get; set; }

        public DateTime?  NgayXacThuc { get; set; }

        public DateTime?  NgayNgungSuDung { get; set; }
    }
}
