using DLL.Enums;

namespace Services.ViewModels.TienIch
{
    public class NhatKyThaoTacLoiViewModel : ThongTinChungViewModel
    {
        public string NhatKyThaoTacLoiId { get; set; }
        public string MoTa { get; set; }
        public string HuongDanXuLy { get; set; }
        public ThaoTacLoi ThaoTacLoi { get; set; }
        public string RefId { get; set; }
    }
}
