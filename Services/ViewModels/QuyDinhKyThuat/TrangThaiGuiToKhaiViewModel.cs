using DLL.Enums;
using System;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class TrangThaiGuiToKhaiViewModel
    {
        public string Id { get; set; }
        public string IdToKhai { get; set; }
        public string MaThongDiep { get; set; }

        public TrangThaiGuiToKhaiDenCQT TrangThaiGui { get; set; }

        public TrangThaiTiepNhanCuaCoQuanThue TrangThaiTiepNhan { get; set; }

        public DateTime NgayGioGui { get; set; }

        public string FileXMLGui { get; set; }

        public byte[] NoiDungFileGui { get; set; }
    }
}
