using DLL.Enums;
using ManagementServices.Helper;
using Services.ViewModels.QuanLy;
using System.Collections.Generic;

namespace Services.Helper.Params.QuanLy
{
    public class BoKyHieuHoaDonParams : PagingParams
    {
        public List<string> KyHieus { get; set; }
        public List<TrangThaiSuDung> TrangThaiSuDungs { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
    }
}
