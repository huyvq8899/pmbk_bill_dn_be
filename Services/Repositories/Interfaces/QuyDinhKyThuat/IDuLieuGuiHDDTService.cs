using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IDuLieuGuiHDDTService
    {
        Task<PagedList<DuLieuGuiHDDTViewModel>> GetAllPagingAsync(ThongDiepParams @params);
        Task<ThongDiepChungViewModel> GetByIdAsync(string id);
        Task<string> ExportXMLGuiDiAsync(string id);
        Task<string> ExportXMLKetQuaAsync(string id);
        byte[] GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params);
        byte[] GuiThongDiepKiemTraKyThuat(ThongDiepParams @params);
        Task<bool> NhanPhanHoiThongDiepKiemTraDuLieuHoaDonAsync(ThongDiepParams @params);
        Task<bool> NhanPhanHoiThongDiepKyThuatAsync(ThongDiepParams @params);
        Task<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep> KetQuaKiemTraDuLieuHoaDonAsync(string id);
        Task<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep> KetQuaPhanHoiKyThuatAsync(string id);
        FileReturn CreateThongDiepPhanHoi(ThongDiepPhanHoiParams model);
        Task<bool> GuiThongDiepDuLieuHDDTAsync(string id);

        Task<ThongDiepChungViewModel> InsertAsync(ThongDiepChungViewModel model);
        Task<bool> UpdateAsync(DuLieuGuiHDDTViewModel model);
        Task<bool> UpdateTrangThaiGuiAsync(DuLieuGuiHDDTViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
