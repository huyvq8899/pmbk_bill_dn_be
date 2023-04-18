using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6
{
    /// <summary>
    /// Định dạng của một thông điệp phản hồi kỹ thuật
    /// </summary>
    public partial class TDiep
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }
}
