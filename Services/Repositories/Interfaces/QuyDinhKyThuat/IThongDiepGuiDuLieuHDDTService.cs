using ManagementServices.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IThongDiepGuiDuLieuHDDTService
    {
        Task<PagedList<ThongDiepGuiDuLieuHDDTViewModel>> GetAllPagingAsync(ThongDiepParams @params);
        Task<ThongDiepGuiDuLieuHDDTViewModel> GetByIdAsync(string id);
        Task<string> ExportXMLGuiDiAsync(string id);
        Task<string> ExportXMLKetQuaAsync(string id);
        byte[] GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params);
        byte[] GuiThongDiepKiemTraKyThuat(ThongDiepParams @params);
        Task<bool> NhanPhanHoiThongDiepKiemTraDuLieuHoaDonAsync(ThongDiepParams @params);
        Task<bool> NhanPhanHoiThongDiepKyThuatAsync(ThongDiepParams @params);
        Task<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep> KetQuaKiemTraDuLieuHoaDonAsync(string id);
        Task<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep> KetQuaPhanHoiKyThuatAsync(string id);
        Task<bool> GuiThongDiepDuLieuHDDTKhongMaAsync(string id);

        Task<ThongDiepGuiDuLieuHDDTViewModel> InsertAsync(ThongDiepGuiDuLieuHDDTViewModel model);
        Task<bool> UpdateAsync(ThongDiepGuiDuLieuHDDTViewModel model);
        Task<bool> UpdateTrangThaiGuiAsync(ThongDiepGuiDuLieuHDDTViewModel model);
        Task<bool> DeleteAsync(string id);
    }
}
