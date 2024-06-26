﻿using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IBienBanDieuChinhService
    {
        Task<BienBanDieuChinhViewModel> GetByIdAsync(string id);
        Task<string> PreviewBienBanAsync(string id);

        Task<BienBanDieuChinhViewModel> GateForWebSocket(ParamPhatHanhBBDC @param);
        Task<BienBanDieuChinhViewModel> InsertAsync(BienBanDieuChinhViewModel model);
        Task<bool> UpdateAsync(BienBanDieuChinhViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
