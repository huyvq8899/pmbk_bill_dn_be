using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Helper.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface ITraCuuService
    {
        Task<HoaDonDienTuViewModel> TraCuuByMa(string strMaTraCuu);
        Task<KetQuaTraCuuXML> GetMaTraCuuInXml(IFormFile file);
        Task<HoaDonDienTuViewModel> TraCuuBySoHoaDon(KetQuaTraCuuXML input);
        byte[] FindSignatureElement(string xmlFilePath, int Type);
    }
}
