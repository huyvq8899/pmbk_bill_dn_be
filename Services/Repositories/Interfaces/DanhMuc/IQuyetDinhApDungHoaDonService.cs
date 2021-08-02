using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IQuyetDinhApDungHoaDonService
    {
        Task<PagedList<QuyetDinhApDungHoaDonViewModel>> GetAllPagingAsync(QuyetDinhApDungHoaDonParams @params);
        Task<QuyetDinhApDungHoaDonViewModel> GetByIdAsync(string id);
        Task<List<QuyetDinhApDungHoaDonDieu2ViewModel>> GetMauCacLoaiHoaDonAsync(string id);
        Task<List<QuyetDinhApDungHoaDonDieu2ViewModel>> GetListMauHoaDonByIdAsync(string id);
        Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau);

        Task<QuyetDinhApDungHoaDonViewModel> InsertAsync(QuyetDinhApDungHoaDonViewModel model);
        Task<bool> UpdateAsync(QuyetDinhApDungHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(QuyetDinhApDungHoaDonViewModel model);
    }
}
