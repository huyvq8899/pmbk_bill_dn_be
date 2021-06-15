using System;

namespace DLL.Entity
{
    public class ThongTinChung
    {
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public int? STT { get; set; }
    }
}
