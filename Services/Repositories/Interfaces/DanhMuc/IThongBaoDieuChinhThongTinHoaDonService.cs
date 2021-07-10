using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongBaoDieuChinhThongTinHoaDonService
    {
        Task<List<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllAsync(ThongBaoDieuChinhThongTinHoaDonParams @params = null);
        Task<PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllPagingAsync(ThongBaoDieuChinhThongTinHoaDonParams @params);
        Task<ThongBaoDieuChinhThongTinHoaDonViewModel> GetByIdAsync(string id);
        List<EnumModel> GetTrangThaiHieuLucs();
        Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetBangKeHoaDonChuaSuDungAsync(string id);

        Task<ThongBaoDieuChinhThongTinHoaDonViewModel> InsertAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model);
        Task<bool> UpdateAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model);
    }
}
