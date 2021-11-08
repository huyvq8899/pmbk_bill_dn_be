using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params
{
    public class ExportParams
    {
        public ThongDiepChungViewModel ThongDiep { get; set; }
        public bool Signed { get; set; }
    }
}
