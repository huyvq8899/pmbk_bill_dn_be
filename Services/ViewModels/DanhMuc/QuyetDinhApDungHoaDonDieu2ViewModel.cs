namespace Services.ViewModels.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu2ViewModel : ThongTinChungViewModel
    {
        public string QuyetDinhApDungHoaDonDieu2Id { get; set; }
        public string QuyetDinhApDungHoaDonId { get; set; }
        public string MauHoaDonId { get; set; }
        public string MucDichSuDung { get; set; }

        public string TenLoaiHoaDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }

        public bool? Checked { get; set; }
    }
}
