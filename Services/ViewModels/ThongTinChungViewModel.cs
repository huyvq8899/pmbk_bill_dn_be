using Services.Helper.LogHelper;
using System;

namespace Services.ViewModels
{
    public class ThongTinChungViewModel
    {
        [CheckBox]
        public bool? Status { get; set; }
        [IgnoreLogging]
        public DateTime? CreatedDate { get; set; }
        [IgnoreLogging]
        public string CreatedBy { get; set; }
        [IgnoreLogging]
        public DateTime? ModifyDate { get; set; }
        [IgnoreLogging]
        public string ModifyBy { get; set; }
        [IgnoreLogging]
        public int? STT { get; set; }
    }
}
