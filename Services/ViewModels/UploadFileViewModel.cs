using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Services.ViewModels
{
    public class UploadFileViewModel
    {
        public MenuType MenuType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameGuid { get; set; }
        public string Link { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<string> RemovedFiles { get; set; }
    }

    public enum MenuType
    {
        ThongBaoKetQuaHuyHoaDon,
        ThongBaoDieuChinhThongTinHoaDon
    }
}
