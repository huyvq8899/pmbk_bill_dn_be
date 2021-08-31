using Microsoft.AspNetCore.Http;
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
        Task<string> GetMaTraCuuInXml(IFormFile file);
    }
}
