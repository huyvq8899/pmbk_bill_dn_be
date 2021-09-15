using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2
{
    /// <summary>
    /// Định dạng thông điệp gửi bảng tổng hợp dữ liệu hóa đơn điện tử không có mã tới cơ quan thuế
    /// </summary>
    public partial class TDiep
    {
        public DLieu DLieu { get; set; }
        public TTChungThongDiep TTChung { get; set; }
    }
}
