using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongBaoPhatHanhService
    {
        Task<List<ThongBaoPhatHanhViewModel>> GetAllAsync(ThongBaoPhatHanhParams @params = null);
        Task<PagedList<ThongBaoPhatHanhViewModel>> GetAllPagingAsync(ThongBaoPhatHanhParams @params);
        Task<ThongBaoPhatHanhViewModel> GetByIdAsync(string id);
        List<EnumModel> GetTrangThaiNops();
        Task<int> GetTuSoTiepTheoAsync(ThongBaoPhatHanhChiTietViewModel model);
        Task<List<ThongBaoPhatHanhChiTietViewModel>> GetCacLoaiHoaDonPhatHanhsAsync(string id);
        Task<List<ThongBaoPhatHanhChiTietViewModel>> GetThongBaoPhatHanhChiTietByIdAsync(string id);
        Task<List<ThongBaoPhatHanhChiTietViewModel>> GetListChiTietThongBaoPhatHanhByMauHoaDonIdAsync(string mauHoaDonId);
        Task<string> CheckAllowUpdateDeleteAsync(string id);
        Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau);
        Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model);

        Task<ThongBaoPhatHanhViewModel> InsertAsync(ThongBaoPhatHanhViewModel model);
        Task<bool> UpdateAsync(ThongBaoPhatHanhViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(ThongBaoPhatHanhViewModel model);
    }
}
