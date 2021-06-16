using ManagementServices.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface ILoaiTienService
    {
        Task<List<LoaiTienViewModel>> GetAllAsync(LoaiTienParams @params = null);
        Task<PagedList<LoaiTienViewModel>> GetAllPagingAsync(LoaiTienParams @params);
        Task<LoaiTienViewModel> GetByIdAsync(string id);

        Task<LoaiTienViewModel> InsertAsync(LoaiTienViewModel model);
        Task<bool> UpdateAsync(LoaiTienViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
