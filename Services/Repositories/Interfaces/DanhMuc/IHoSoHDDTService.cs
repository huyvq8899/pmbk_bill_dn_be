using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IHoSoHDDTService
    {
        Task<HoSoHDDTViewModel> GetDetailAsync();
        Task<HoSoHDDTViewModel> InsertAsync(HoSoHDDTViewModel model);
        Task<bool> UpdateAsync(HoSoHDDTViewModel model);
        List<CityParam> GetListCoQuanThueCapCuc();
        List<DistrictsParam> GetListCoQuanThueQuanLy();
        List<CityParam> GetListCity();
        Task<List<ChungThuSoSuDungViewModel>> GetDanhSachChungThuSoSuDung();
        Task<bool> InsertMCCQTToKhaiAsync(DLieu DLieu, string ThongDiepChungId);
        Task<string> GetPhatHanhBoiFromHopDong();

    }
}
