using DLL.Entity.NghiepVu;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.ViewModels;
using Services.ViewModels.ModelTemp;
using Services.ViewModels.NghiepVu;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IHoaDonDienTuService
    {
        Task<HoaDonDienTuViewModel> InsertAsync(HoaDonDienTuViewModel model);
        Task<bool> UpdateAsync(HoaDonDienTuViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<HoaDonDienTuViewModel> GetByIdAsync(string id);
        Task<List<HoaDonDienTuViewModel>> GetAllAsync();
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingAsync(PagingParams pagingParams);
        Task<string> CreateSoChungTuAsync();
        Task<bool> CheckSoHoaDonAsync(string SoHoaDon); // 1: nvk, 2: qttu
        string ExportExcelChungTuKeToan(ChungTuNghiepVuKhacViewModel model);
        Task<string> PrintChungTuKeToanAsync(ChungTuNghiepVuKhacViewModel model);
        Task<string> PrintQuyetToanTamUngAsync(ChungTuNghiepVuKhacViewModel model);
        Task<List<ChungTuNghiepVuKhacViewModelTemp>> ImportCTNVKAsync(IList<IFormFile> files, UserViewModel ActionUser);
        Task<string> CreateFileImportCTNVKError(List<ChungTuNghiepVuKhacViewModelTemp> list);
        Task<List<ChungTuNghiepVuKhacViewModelTemp>> ImportCTQTTUAsync(IList<IFormFile> files, UserViewModel ActionUser);
        Task<string> CreateFileImportCTQTTUError(List<ChungTuNghiepVuKhacViewModelTemp> list);
        Task<ChungTuNghiepVuKhacViewModel> HelpPreviewMutipleById(string id);
        //Task<string> PreviewMultiplePDFQuyetToanTamUng(PreviewMultipleViewModel previewMultipleVM);
        //Task<string> PreviewMultiplePDFChungTuKeToan(PreviewMultipleViewModel previewMultipleVM);
        Task<bool> DeleteFilePDF(string fileName);
        Task<string> ExportExcelBangKe(PagingParams pagingParams);
        Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model);
    }
}
