namespace DLL.Entity.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu2 : ThongTinChung
    {
        public string QuyetDinhApDungHoaDonDieu2Id { get; set; }
        public string QuyetDinhApDungHoaDonId { get; set; }
        public string MauHoaDonId { get; set; }
        public string MucDichSuDung { get; set; }

        public QuyetDinhApDungHoaDon QuyetDinhApDungHoaDon { get; set; }
    }
}
