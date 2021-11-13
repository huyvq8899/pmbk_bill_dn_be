using ManagementServices.Helper;
using Services.Helper.Params.QuanLy;
using Services.ViewModels.QuanLy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLy
{
    public interface IBoKyHieuHoaDonService
    {
        Task<List<BoKyHieuHoaDonViewModel>> GetAllAsync();
        Task<PagedList<BoKyHieuHoaDonViewModel>> GetAllPagingAsync(BoKyHieuHoaDonParams @params);
        Task<BoKyHieuHoaDonViewModel> GetByIdAsync(string id);
        Task<bool> CheckTrungKyHieuAsync(BoKyHieuHoaDonViewModel model);
        Task<List<BoKyHieuHoaDonViewModel>> GetListByMauHoaDonIdAsync(string mauHoaDonId);
        Task<List<NhatKyXacThucBoKyHieuViewModel>> GetListNhatKyXacThucByIdAsync(string id);
        string CheckSoSeriChungThu(BoKyHieuHoaDonViewModel model);
        Task<List<BoKyHieuHoaDonViewModel>> GetListForHoaDonAsync(BoKyHieuHoaDonViewModel model);
        Task<bool> CheckDaHetSoLuongHoaDonAsync(string boKyHieuHoaDonId, string soHoaDon);
        Task UpdateRangeTrangThaiHetHieuLucAsync();

        Task<BoKyHieuHoaDonViewModel> InsertAsync(BoKyHieuHoaDonViewModel model);
        Task<bool> UpdateAsync(BoKyHieuHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> XacThucBoKyHieuHoaDonAsync(NhatKyXacThucBoKyHieuViewModel model);
    }
}
