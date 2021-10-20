using DLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class TrangThaiGuiToKhai
    {
        public string Id { get; set; }
        public string IdToKhai { get; set; }
        
        public string MaThongDiep { get; set; }
        public TrangThaiGuiToKhaiDenCQT TrangThaiGui { get; set; }

        public TrangThaiTiepNhanCuaCoQuanThue TrangThaiTiepNhan { get; set; }

        public string FileXMLGui { get; set; }

        public byte[] NoiDungFileGui { get; set; }
        public DateTime? NgayGioGui { get; set; }
    }
}
