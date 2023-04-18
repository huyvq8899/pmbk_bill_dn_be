using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.ViewModels.XML;
using System.Collections.Generic;

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


    public class XacNhanToKhaiParams
    {
        public string ThongDiepId { get; set; }
        public string MaSoThue { get; set; }
        public IList<IFormFile> Files { get; set; }
    }
}
