using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class ThongTinChungViewModel
    {
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public int? STT { get; set; }
    }
}
