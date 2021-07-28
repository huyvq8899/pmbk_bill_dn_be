using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu1ViewModel : ThongTinChungViewModel
    {
        public string QuyetDinhApDungHoaDonDieu1Id { get; set; }
        public string QuyetDinhApDungHoaDonId { get; set; }
        public string Ten { get; set; }
        public string GiaTri { get; set; }
        public bool? Checked { get; set; }
        public LoaiDieu1 LoaiDieu1 { get; set; }
    }
}
