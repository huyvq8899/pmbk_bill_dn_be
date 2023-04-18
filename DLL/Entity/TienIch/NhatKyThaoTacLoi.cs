using DLL.Enums;

namespace DLL.Entity.TienIch
{
    public class NhatKyThaoTacLoi : ThongTinChung
    {
        public string NhatKyThaoTacLoiId { get; set; }
        public string MoTa { get; set; }
        public string HuongDanXuLy { get; set; }
        public ThaoTacLoi ThaoTacLoi { get; set; }
        public string RefId { get; set; }
    }
}
