using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ToKhaiDangKyThongTin
    {
        public string Id { get; set; }
        public bool IsThemMoi { get; set; }
        public bool NhanUyNhiem { get; set; }
        public int? LoaiUyNhiem { get; set; }
        public string FileXMLChuaKy { get; set; }
        public byte[] ContentXMLChuaKy { get; set; }
        public bool SignedStatus { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual List<DuLieuKyToKhai> DuLieuKys { get; set; }
        public virtual List<TrangThaiGuiToKhai> TrangThaiGuiToKhais { get; set; }
    }
}
