using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongBaoKetQuaHuyHoaDonService
    {
        Task<PagedList<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllPagingAsync(ThongBaoKetQuaHuyHoaDonParams @params);
        Task<ThongBaoKetQuaHuyHoaDonViewModel> GetByIdAsync(string id);
        Task<List<ThongBaoKetQuaHuyHoaDonChiTietViewModel>> GetThongBaoKetQuaHuyChiTietByIdAsync(string id);
        Task<bool> CheckAllowDeleteWhenChuaNopAsync(string id);
        Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau);
        Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model);

        Task<ThongBaoKetQuaHuyHoaDonViewModel> InsertAsync(ThongBaoKetQuaHuyHoaDonViewModel model);
        Task<bool> UpdateAsync(ThongBaoKetQuaHuyHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(ThongBaoKetQuaHuyHoaDonViewModel model);
    }
}
