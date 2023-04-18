using Services.Helper.LogHelper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    [Serializable]
    public class ThongTinChungViewModel
    {
        [CheckBox]
        [Display(Name = "Ngừng theo dõi")]
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
