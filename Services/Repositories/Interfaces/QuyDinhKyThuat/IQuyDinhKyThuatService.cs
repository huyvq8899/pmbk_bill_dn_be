using ManagementServices.Helper;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IQuyDinhKyThuatService
    {
        Task<ToKhaiDangKyThongTinViewModel> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai);
        Task<bool> LuuTrangThaiGuiToKhai(TrangThaiGuiToKhaiViewModel tThai);
        Task<PagedList<ToKhaiDangKyThongTinViewModel>> GetPagingAsync(PagingParams @params);
        Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id);
        Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> XoaToKhai(string Id);
        Task<string> GuiToKhai(string XMLUrl, string idTKhai);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep ConvertToThongDiepTiepNhan(string encodedContent);
        Task<string> NhanPhanHoiCQT(string fileXML, string idTKhai);
        Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id);
        Task<bool> InsertThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> UpdateThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> DeleteThongDiepChung(string Id);
        Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string ThongDiepGocId);
        Task<int> GetLanThuMax(int MaLoaiThongDiep);
        Task<ThongDiepChungViewModel> GetThongDiepByThamChieu(string ThamChieuId);
        List<LoaiThongDiep> GetListLoaiThongDiepNhan();
        Task<int> GetLanGuiMax(ThongDiepChungViewModel td);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep ConvertToThongDiepKUNCQT(string encodedContent);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep ConvertToThongDiepUNCQT(string encodedContent);
        Task<bool> ThongDiepDaGui(ThongDiepChungViewModel td);
    }
}
