using ManagementServices.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IQuyetDinhApDungHoaDonService
    {
        Task<PagedList<QuyetDinhApDungHoaDonViewModel>> GetAllPagingAsync(QuyetDinhApDungHoaDonParams @params);
        Task<QuyetDinhApDungHoaDonViewModel> GetByIdAsync(string id);

        Task<QuyetDinhApDungHoaDonViewModel> InsertAsync(QuyetDinhApDungHoaDonViewModel model);
        Task<bool> UpdateAsync(QuyetDinhApDungHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(QuyetDinhApDungHoaDonViewModel model);
    }
}
