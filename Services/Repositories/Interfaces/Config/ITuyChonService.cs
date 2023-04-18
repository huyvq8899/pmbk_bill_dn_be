using Services.ViewModels.BaoCao;
using Services.ViewModels.Config;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Config
{
    public interface ITuyChonService
    {
        Task<List<TuyChonViewModel>> GetAllAsync(string keyword = null);
        Task<TuyChonViewModel> GetDetailAsync(string ma);
        TuyChonViewModel GetDetail(string ma);

        Task<bool> LayLaiThietLapEmailMacDinh(int LoaiEmail);
        Task<bool> UpdateAsync(TuyChonViewModel model);
        Task<List<ConfigNoiDungEmailViewModel>> GetAllNoiDungEmail();
        Task<bool> UpdateRangeNoiDungEmailAsync(List<ConfigNoiDungEmailViewModel> models);
        Task<List<TruongDuLieuViewModel>> GetThongTinHienThiTruongDL(string tenChucNang);
        Task<bool> UpdateHienThiTruongDuLieu(List<TruongDuLieuViewModel> datas);
        Task<bool> CheckCoPhatSinhNgoaiTeAsync();
        Task<bool> GetTypeLoaiChuKi();
        Task<bool> UpdateLoaiChuKi(TuyChonViewModel model);
        Task<List<TuyChonViewModel>> GetListByHoaDonIdAsync(string hoaDonId);

    }
}
