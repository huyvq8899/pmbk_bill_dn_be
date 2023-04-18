using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._6
{
    /// <summary>
    /// Định dạng của một thông điệp thông báo về hóa đơn điện tử cần rà soát
    /// </summary>
    public partial class TDiep
    {
        public DLieu DLieu { get; set; }
        public TTChungThongDiep TTChung { get; set; }
    }
}
