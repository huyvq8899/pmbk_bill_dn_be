using ManagementServices.Helper;
using Services.Helper;
using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IThongDiepGuiHDDTKhongMaService
    {
        Task<PagedList<ThongDiepGuiHDDTKhongMaViewModel>> GetAllPagingAsync(PagingParams @params);
        Task<ThongDiepGuiHDDTKhongMaViewModel> GetByIdAsync(string id);
        Task<FileReturn> ExportXMLAsync(ThongDiepGuiHDDTKhongMaViewModel model);
        Task<FileReturn> ExportXMLAsync(string id);

        Task<ThongDiepGuiHDDTKhongMaViewModel> InsertAsync(ThongDiepGuiHDDTKhongMaViewModel model);
        Task<bool> UpdateAsync(ThongDiepGuiHDDTKhongMaViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
