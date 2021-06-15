namespace DLL.Entity.DanhMuc
{
    public class LoaiTien : ThongTinChung
    {
        public string LoaiTienId { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public decimal? TyGiaQuyDoi { get; set; }
        public int? SapXep { get; set; }
    }
}
