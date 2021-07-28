namespace DLL.Entity.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu1 : ThongTinChung
    {
        public string QuyetDinhApDungHoaDonDieu1Id { get; set; }
        public string QuyetDinhApDungHoaDonId { get; set; }
        public string Ten { get; set; }
        public string GiaTri { get; set; }
        public bool? Checked { get; set; }

        public QuyetDinhApDungHoaDon QuyetDinhApDungHoaDon { get; set; }
    }
}
