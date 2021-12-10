using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IDonViTinhService
    {
        Task<List<DonViTinhViewModel>> GetAllAsync(DonViTinhParams @params = null);
        Task<PagedList<DonViTinhViewModel>> GetAllPagingAsync(DonViTinhParams @params);
        Task<DonViTinhViewModel> GetByIdAsync(string id);
        Task<FileReturn> ExportExcelAsync(DonViTinhParams @params);
        DonViTinhViewModel CheckTenOutObject(string ten, List<DonViTinh> models);

        Task<DonViTinhViewModel> InsertAsync(DonViTinhViewModel model);
        Task<bool> UpdateAsync(DonViTinhViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(DonViTinhViewModel model);
    }
}
