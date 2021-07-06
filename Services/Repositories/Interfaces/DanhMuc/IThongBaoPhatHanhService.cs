using ManagementServices.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
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

        Task<ThongBaoPhatHanhViewModel> InsertAsync(ThongBaoPhatHanhViewModel model);
        Task<bool> UpdateAsync(ThongBaoPhatHanhViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(ThongBaoPhatHanhViewModel model);
    }
}
