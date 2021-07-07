namespace DLL.Entity.DanhMuc
{
    public class MauHoaDonThietLapMacDinh : ThongTinChung
    {
        public string MauHoaDonThietLapMacDinhId { get; set; }
        public string MauHoaDonId { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string GiaTri { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Opacity { get; set; }

        public MauHoaDon MauHoaDon { get; set; }
    }
}
