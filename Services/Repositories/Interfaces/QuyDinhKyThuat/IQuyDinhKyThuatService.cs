using ManagementServices.Helper;
using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IQuyDinhKyThuatService
    {
        Task<bool> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai);
        Task<bool> LuuTrangThaiGuiToKhai(TrangThaiGuiToKhaiViewModel tThai);
        Task<PagedList<ToKhaiDangKyThongTinViewModel>> GetPagingAsync(PagingParams @params);
        Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id);
        Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> XoaToKhai(string Id);
        Task<bool> GuiToKhai(string XMLUrl, string idTKhai);
        Task<bool> NhanPhanHoiCQT(string fileXML, string idTKhai);
    }
}
