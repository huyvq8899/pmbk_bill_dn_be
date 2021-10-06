using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongBaoDieuChinhThongTinHoaDonService
    {
        Task<PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllPagingAsync(ThongBaoDieuChinhThongTinHoaDonParams @params);
        Task<ThongBaoDieuChinhThongTinHoaDonViewModel> GetByIdAsync(string id);
        List<EnumModel> GetTrangThaiHieuLucs();
        Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetBangKeHoaDonChuaSuDungAsync(string id);
        Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetThongBaoDieuChinhThongTinChiTietByIdAsync(string id);
        Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau, int loai); // Loai: 1: Thông báo điều chỉnh, 2: Bảng kê hóa đơn chưa sử dụng
        Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model);

        Task<ThongBaoDieuChinhThongTinHoaDonViewModel> InsertAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model);
        Task<bool> UpdateAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model);
    }
}
