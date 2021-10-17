using ManagementServices.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IThongDiepGuiHDDTKhongMaService
    {
        Task<PagedList<ThongDiepGuiHDDTKhongMaViewModel>> GetAllPagingAsync(PagingParams @params);
        Task<ThongDiepGuiHDDTKhongMaViewModel> GetByIdAsync(string id);
        Task<string> ExportXMLGuiDiAsync(string id);
        Task<string> ExportXMLKetQuaAsync(string id);
        byte[] GuiThongDiep(ThongDiepParams @params);
        Task<bool> NhanPhanHoiAsync(ThongDiepParams @params);
        Task<TDiep> XemKetQuaTuCQTAsync(string id);

        Task<ThongDiepGuiHDDTKhongMaViewModel> InsertAsync(ThongDiepGuiHDDTKhongMaViewModel model);
        Task<bool> UpdateAsync(ThongDiepGuiHDDTKhongMaViewModel model);
        Task<bool> UpdateTrangThaiGuiAsync(ThongDiepGuiHDDTKhongMaViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
