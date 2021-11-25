using ManagementServices.Helper;
using Services.ViewModels.XML;

namespace Services.Helper.Params.QuyDinhKyThuat
{
    public class ThongDiepParams : PagingParams
    {
        public string FileUrl { get; set; }
        public string ThongDiepId { get; set; }
        public byte[] FileByte { get; set; }
        public Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._8.TDiep ThongDiepKhongUyNhiem { get; set; }
        public Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9.TDiep ThongDiepUyNhiem { get; set; }
        public MLTDiep LoaiThongDiep { get; set; }
    }
}
