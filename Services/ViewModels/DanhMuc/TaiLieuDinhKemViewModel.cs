using DLL.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class TaiLieuDinhKemViewModel : ThongTinChungViewModel
    {
        public string TaiLieuDinhKemId { get; set; }
        public string NghiepVuId { get; set; }
        public RefType LoaiNghiepVu { get; set; }
        public string TenGoc { get; set; }
        public string TenGuid { get; set; }
        public byte[] Link { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<string> RemovedFileIds { get; set; }
    }
}
