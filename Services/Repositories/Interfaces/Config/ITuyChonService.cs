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
        //Task<List<TruongDuLieuHoaDonViewModel>> GetThongTinHienThiTruongDLHoaDon(bool isChiTiet, int LoaiHoaDon);
        //Task<List<TruongDuLieuHoaDonViewModel>> GetThongTinHienThiTruongDLHoaDon(bool isChiTiet);
        //Task<List<ThietLapTruongDuLieuViewModel>> GetThongTinHienThiTruongDLMoRong(int loaiHoaDon);
        //Task<bool> UpdateHienThiTruongDuLieuHoaDon(List<TruongDuLieuHoaDonViewModel> datas);
        //Task<bool> UpdateThietLapTruongDuLieuMoRong(List<ThietLapTruongDuLieuViewModel> datas);
    }
}
