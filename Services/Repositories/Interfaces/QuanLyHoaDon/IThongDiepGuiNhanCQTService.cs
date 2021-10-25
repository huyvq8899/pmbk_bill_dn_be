using ManagementServices.Helper;
using Services.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML.ThongDiepGuiNhanCQT;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IThongDiepGuiNhanCQTService
    {
        Task<List<HoaDonSaiSotViewModel>> GetListHoaDonSaiSotAsync(HoaDonSaiSotParams @params);
        Task<KetQuaLuuThongDiep> InsertThongBaoGuiHoaDonSaiSotAsync(ThongDiepGuiCQTViewModel model);
        Task<string> GateForWebSocket(FileXMLThongDiepGuiParams @params);
        Task<bool> DeleteAsync(string id);
        List<DiaDanhParam> GetDanhSachDiaDanh();
        Task<bool> GuiThongDiepToiCQTAsync(DuLieuXMLGuiCQTParams @params);
    }
}
