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
        byte[] GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params);
        byte[] GuiThongDiepKiemTraKyThuat(ThongDiepParams @params);
        FileReturn CreateThongDiepPhanHoi(ThongDiepPhanHoiParams model);
        Task<bool> GuiThongDiepDuLieuHDDTAsync(string id);

        Task<ThongDiepChungViewModel> InsertAsync(ThongDiepChungViewModel model);
        Task<bool> UpdateAsync(DuLieuGuiHDDTViewModel model);
        Task<bool> UpdateTrangThaiGuiAsync(DuLieuGuiHDDTViewModel model);
        Task<List<TongHopDuLieuHoaDonGuiCQTViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params);
        string CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params);
        Task<bool> GuiBangDuLieu(string XMLUrl, string thongDiepChungId, string maThongDiep, string mst);
        string LuuDuLieuKy(string encodedContent, string thongDiepId);
    }
}
