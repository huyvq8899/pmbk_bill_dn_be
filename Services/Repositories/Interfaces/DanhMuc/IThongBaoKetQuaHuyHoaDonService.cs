using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongBaoKetQuaHuyHoaDonService
    {
        Task<List<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllAsync(ThongBaoKetQuaHuyHoaDonParams @params = null);
        Task<PagedList<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllPagingAsync(ThongBaoKetQuaHuyHoaDonParams @params);
        Task<ThongBaoKetQuaHuyHoaDonViewModel> GetByIdAsync(string id);

        Task<ThongBaoKetQuaHuyHoaDonViewModel> InsertAsync(ThongBaoKetQuaHuyHoaDonViewModel model);
        Task<bool> UpdateAsync(ThongBaoKetQuaHuyHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(ThongBaoKetQuaHuyHoaDonViewModel model);
    }
}
