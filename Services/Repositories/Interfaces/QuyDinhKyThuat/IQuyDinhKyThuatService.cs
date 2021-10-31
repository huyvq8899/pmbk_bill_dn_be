using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.XmlModel;
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
        Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id);
        Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> XoaToKhai(string Id);
        Task<bool> GuiToKhai(string XMLUrl, string idThongDiep, string maThongDiep, string mst);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep ConvertToThongDiepTiepNhan(string encodedContent);
        Task<string> NhanPhanHoiCQT(string fileXML, string idTKhai);
        Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id);
        Task<ThongDiepChungViewModel> InsertThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> UpdateThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> DeleteThongDiepChung(string Id);
        Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string ThongDiepGocId);
        Task<int> GetLanThuMax(int MaLoaiThongDiep);
        Task<ThongDiepChungViewModel> GetThongDiepByThamChieu(string ThamChieuId);
        List<LoaiThongDiep> GetListLoaiThongDiepNhan();
        List<LoaiThongDiep> GetListLoaiThongDiepGui();
        Task<int> GetLanGuiMax(ThongDiepChungViewModel td);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep ConvertToThongDiepKUNCQT(string encodedContent);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep ConvertToThongDiepUNCQT(string encodedContent);
        Task<bool> ThongDiepDaGui(ThongDiepChungViewModel td);
        Task<bool> InsertThongDiepNhanAsync(ThongDiepPhanHoiParams @params);
        Task<string> GetXMLDaKy(string ToKhaiId);
        Task<ThongDiepChiTiet> ShowThongDiepFromFileByIdAsync(string id);
        Task<FileReturn> ExportBangKeAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhai();
    }
}
