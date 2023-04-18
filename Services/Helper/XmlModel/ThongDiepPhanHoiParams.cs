using Services.ViewModels;

namespace Services.Helper.XmlModel
{
    public class ThongDiepPhanHoiParams
    {
        public string ThongDiepId { get; set; }
        public string MST { get; set; }
        public int MLTDiep { get; set; }
        public string MTDiep { set; get; }
        public string MTDTChieu { get; set; }
        public string DataXML { get; set; }
        public int MLTDiepPhanHoi { get; set; }
        public bool SelfConfirm { get; set; } = false;
        public UserViewModel ActionUser { get; set; }
        public string Url { get; set; }

    }
}
