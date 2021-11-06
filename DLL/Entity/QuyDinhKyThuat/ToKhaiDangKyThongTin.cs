using System;

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
    }
}
