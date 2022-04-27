using DLL.Enums;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IDuLieuGuiHDDTService
    {
        Task<ThongDiepChungViewModel> GetByIdAsync(string id);
        Task<PagedList<ThongDiepChungViewModel>> GetByHoaDonDienTuIdAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetAllThongDiepTraVeInTransLogsAsync(string maThongDiep);
        List<ThongDiepChungViewModel> GetThongDiepTraVeInTransLogsAsync(string maThongDiep);
        byte[] GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params);
        byte[] GuiThongDiepKiemTraKyThuat(ThongDiepParams @params);
        FileReturn CreateThongDiepPhanHoi(ThongDiepPhanHoiParams model);
        Task<TrangThaiQuyTrinh> GuiThongDiepDuLieuHDDTAsync(string id);
        Task GuiThongDiepDuLieuHDDTBackgroundAsync();

        Task<ThongDiepChungViewModel> InsertAsync(ThongDiepChungViewModel model);
        Task<bool> UpdateAsync(DuLieuGuiHDDTViewModel model);
        Task<bool> UpdateTrangThaiGuiAsync(DuLieuGuiHDDTViewModel model);
    }
}
