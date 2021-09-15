using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    public enum TCTBao
    {
        [Description("Mới")]
        TCTBao0 = 0,
        [Description("Hủy")]
        TCTBao1 = 1,
        [Description("Điều chỉnh")]
        TCTBao2 = 2,
        [Description("Thay thế")]
        TCTBao3 = 3,
        [Description("Giải trình")]
        TCTBao4 = 4,
        [Description("Sai sót do tổng hợp")]
        TCTBao5 = 5,
    }
}
