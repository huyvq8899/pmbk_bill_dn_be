using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._5
{
    /// <summary>
    /// Định dạng của một thông điệp gửi thông báo về việc tiếp nhận và kết quả xử lý về việc hóa đơn điện tử đã lập có sai sót
    /// </summary>
    public partial class TDiep
    {
        public DLieu DLieu { get; set; }
        public TTChungThongDiep TTChung { get; set; }
    }
}
