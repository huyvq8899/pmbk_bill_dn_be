using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._4
{
    /// <summary>
    /// Định dạng của một thông điệp gửi thông báo về hóa đơn điện tử có sai sót
    /// </summary>
    public partial class TDiep
    {
        public DLieu DLieu { get; set; }
        public TTChungThongDiep TTChung { get; set; }
    }
}
