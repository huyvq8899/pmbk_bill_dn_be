using System.Collections.Generic;

namespace Services.ViewModels.Config
{
    public class TuyChonViewModel
    {
        public string Ma { get; set; } // Tiền tố của mã nên đặt là kiểu dữ liệu để gợi nhớ

        public string Ten { get; set; }

        public string GiaTri { get; set; }

        public List<TuyChonViewModel> NewList { get; set; }
        public List<TuyChonViewModel> OldList { get; set; }
    }
}
