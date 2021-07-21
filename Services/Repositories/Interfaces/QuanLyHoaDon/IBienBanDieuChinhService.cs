using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IBienBanDieuChinhService
    {
        Task<BienBanDieuChinhViewModel> GetByIdAsync(string id);

        Task<BienBanDieuChinhViewModel> InsertAsync(BienBanDieuChinhViewModel model);
        Task<bool> UpdateAsync(BienBanDieuChinhViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
